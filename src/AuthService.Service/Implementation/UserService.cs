using AuthService.Domain.DTOs.Request;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using AuthService.Domain.Enums;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
//using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
//using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service.Implementation
{
    public class UserService : IUserService
    {
        public IUnitOfWork UnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;

        public SignInManager<AppUser> _signInManager { get; }

        public UserService(IUnitOfWork _unitOfWork, IConfiguration configuration, ILogger<UserService> logger, SignInManager<AppUser> signInManager, IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager)
        {
            UnitOfWork = _unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserResponseDetails> Register(RegistrationDTOs RegDtos)
        {
            try
            {
                //generating token


                var token = GetToken(RegDtos.Username);

                var finalToken = new JwtSecurityTokenHandler().WriteToken(token);



                //check if email already exists
                var user_exist = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == RegDtos.Email);



                if (user_exist != null)
                {
                    _logger.LogError("User already Exist");
                    return new UserResponseDetails()
                    {
                        Message = $"User with the email {RegDtos.Email} already exists. please Login",
                        IsSuccess = false
                    };
                };
                //return $"User with the email {RegDtos.Email} already exists. please Login";



                //create new user

                var newuser = new AppUser()
                {
                    Email = RegDtos.Email,
                    FirstName = RegDtos.FirstName,
                    MiddleName = RegDtos.MiddleName,
                    Address = RegDtos.Address,
                    LastName = RegDtos.LastName,
                    PhoneNumber = RegDtos.PhoneNumber,
                    Dob = RegDtos.Dob,
                    Gender = RegDtos.Gender,
                    State = RegDtos.State,
                    LocalGovernmentArea = RegDtos.LGA,
                    Age = CalculateAgeFromDateOfBirth(RegDtos.Dob),
                    UserName = RegDtos.Username,
                    Title = RegDtos.Title,
                    AccountType = RegDtos.AccountType,
                    LandMark = RegDtos.LandMark,
                    Nin = RegDtos.Nin,
                    HasBvn = RegDtos.HasBvn,
                    Bvn = RegDtos.Bvn,
                    AccountNumber = Generate11DigitRandomNumber(),
                    PasswordHash = Hash.HashPassword(RegDtos.Password),

                };

                var add = await UnitOfWork.Users.Add(newuser);
                var emailObject = new SendEmailConfirmation
                {
                    UserEmail = newuser.Email,
                    Token = GenerateRandomToken(),
                    FirstName = newuser.FirstName,
                    Subject = "Account Registration"
                };


                var send = await SendConfirmationEmail(emailObject);
                if (!send)
                {
                    return new UserResponseDetails()
                    {
                        IsSuccess = false,
                        Message = "User registration failed"
                    };
                }
                await SaveVerificationToken(emailObject.UserEmail, emailObject.Token, ActionTypeEnum.EmailConfirmation.ToString());
                // await UnitOfWork.VerificationTokens.Update(saveVerificationToken);
                var save = await UnitOfWork.Save();


                var responseDto = new ResponceRegistationDto()
                {
                    LastLogin = "Now",
                    Token = finalToken,
                    DailyLimitBalance = "",
                    AccountNumber = newuser.AccountNumber,
                    UserName = newuser.UserName,
                    AccountName = $"{newuser.FirstName} {newuser.MiddleName} {newuser.LastName}",
                    Title = newuser.Title,
                    Gender = newuser.Gender,
                    AccountType = newuser.AccountType,
                    Bvn = newuser.Bvn,
                    Nin = newuser.Nin,

                    Status = "",


                };


                _logger.LogError("User Successfully created");

                return new UserResponseDetails()
                {
                    Message = "",
                    IsSuccess = true,
                    ResponseDetails = responseDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Err from user registration ==> {ex.Message}; stacktrace error ==> {ex.StackTrace}");
                return new UserResponseDetails()
                {

                    Message = "Unable to register User",
                    IsSuccess = false
                };

            }



        }

        public async Task<string> ResetPassword(ResetPasswordDtos request)
        {
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == request.Email);
            if (user == null)
            {
                return "User Doesnt Exist";
            }
            var hashPassword = Hash.HashPassword(request.Password);
            var confirm = await ConfirmToken(request.Token, request.Email, ActionTypeEnum.ResetPassword.ToString());

            if (confirm)
            {
                user.PasswordHash = hashPassword;
                user.AccessFailedCount = 0;
                user.Status = StatusEnum.Active.ToString();
                await UnitOfWork.Users.Update(user);
                await UnitOfWork.Save();

                return "Password reset successfull";
            }

            return "Password reset failed";

        }

        public async Task<string> RequestResetPassword(string Email)
        {
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == Email);
            if (user == null)
            {
                return "User does not exist";
            }
            var emailObject = new SendEmailConfirmation
            {
                UserEmail = Email,
                Subject = "Reset Password",
                Token = GenerateRandomToken(),
                FirstName = user.FirstName
            };
            var send = await SendConfirmationEmail(emailObject);
            if (send)
            {
                await SaveVerificationToken(emailObject.UserEmail, emailObject.Token, ActionTypeEnum.ResetPassword.ToString());
                await UnitOfWork.Save();
                return "Email successfully sent";
            }

            return " server error";
        }
        public async Task<string> EmailConfirmation(ResetPasswordDtos request)
        {
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == request.Email);

            if (await ConfirmToken(request.Token, request.Email, ActionTypeEnum.EmailConfirmation.ToString()))
            {
                user.EmailConfirmed = true;
                await UnitOfWork.Users.Update(user);
                await UnitOfWork.Save();
                return "Your Email has beem confirmed";
            }
            return "Email Confirmation Failed";
        }

        public async Task<string> Login(LoginDTOs loginDTOs)
        {

            var hashPassword = Hash.HashPassword(loginDTOs.Password);
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == loginDTOs.Email);

            if ( user == null) 
            {
                return "Invalid Login Credentials";
            }

            if ( user.Status == StatusEnum.Locked.ToString()){
                return "Account is Currently locked. Please Reset Your Password.";
            }

            if (user.PasswordHash != hashPassword )
            {
                user.AccessFailedCount++;
                //await UnitOfWork.Users.Update(user);
                if (user.AccessFailedCount >= 3)
                {
                    user.Status = StatusEnum.Locked.ToString();
                    
                }
                await UnitOfWork.Users.Update(user);
                await UnitOfWork.Save();

                return "Invalid Login Credentials";
            }

            if(!user.EmailConfirmed)
            {
                var emailModel = new SendEmailConfirmation
                {
                    UserEmail = loginDTOs.Email,
                    Subject = "Account Registration",
                    Token = GenerateRandomToken(),
                    FirstName = user.FirstName

                };
                var sendToken = await SendConfirmationEmail(emailModel);
                if(sendToken)
                {
                    await SaveVerificationToken(emailModel.UserEmail,emailModel.Token, ActionTypeEnum.EmailConfirmation.ToString());
                    await UnitOfWork.Save();
                    return "Your Email has not been confirmed. A token has been sent for you to confirm your Email.";
                }

                return "Unknown error";
            }
            var token = GetToken(loginDTOs.Username);
            

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> UpdatePassword(UpdatePasswordDTOs updateDTOs)
        {
            var hashOldPassword = Hash.HashPassword(updateDTOs.OldPassword);
            var hashNewPassword = Hash.HashPassword(updateDTOs.NewPassword);
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == updateDTOs.Email);
            if (user == null)
            {
                return "Invalid Email Address";
            }

            if (user.PasswordHash != hashOldPassword)
            {
                return "Invalid old password";
            }

            user.PasswordHash = hashNewPassword;
            await UnitOfWork.Users.Update(user);
            var result = await UnitOfWork.Save();

            if (result < 1)
            {
                return "failed";
            }

            return "Success";
        }



        private string CalculateAgeFromDateOfBirth(DateTime Dob)
        {
            var today = DateTime.Today;
            var age = today.Year - Dob.Year;

            if (Dob.Date > today.AddYears(-age)) age--;


            var Age = age.ToString();
            return Age;
        }

        private static string Generate11DigitRandomNumber()
        {
            Random random = new Random();
            string result = string.Empty;

            // Generate the first 6 digits
            result += random.Next(100000, 1000000).ToString("D6");

            // Generate the remaining 5 digits
            result += random.Next(10000, 100000).ToString("D5");

            return result;
        }




        private JwtSecurityToken GetToken(string userName)
        {
            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature));

            return token;
        }
        private async Task<bool> SendConfirmationEmail(SendEmailConfirmation request)
        {// create a object for email confirmation


            var httpclient = _httpClientFactory.CreateClient();
            var emailModel = new
            {
                To = request.UserEmail,
                Subject = request.Subject,
                Body = $"Hello {request.FirstName}, here is ur verification token: {request.Token}"

            };
            var sendEmail = await httpclient.PostAsJsonAsync("https://localhost:7168/api/Notification", emailModel);

            // Check if the email was sent successfully
            if (sendEmail.IsSuccessStatusCode)
            {
                // Create the verification token object

                return true;
            }

            // Handle the error appropriately (log, throw exception, etc.)
            return false;



        }

        public async Task<bool> ConfirmToken(string token, string Email, string actionType)
        {
            var userWithToken = await UnitOfWork.VerificationTokens.GetByColumnAsync(x => x.Email == Email && x.ActionType == actionType);
            if (userWithToken == null)
            {
                return false;
            }
            // Define the expiration duration
            var expirationDuration = TimeSpan.FromMinutes(3);

            // Calculate the time elapsed since the token was created
            var timeElapsed = DateTime.Now - userWithToken.DateCreated;
            if (userWithToken.Email == Email && userWithToken.Token == token && timeElapsed < expirationDuration)
            {
                return true;
            }
            return false;
        }

        public async Task SaveVerificationToken(string email, string token, string actionType)
        {

            var existingToken = await UnitOfWork.VerificationTokens.GetByColumnAsync(x => x.Email == email && x.ActionType == actionType);
            if (existingToken == null)
            {
                var verificationToken = new VerificationToken
                {
                    Email = email,
                    Token = token,
                    ActionType = actionType,
                    DateCreated = DateTime.Now
                };
                await UnitOfWork.VerificationTokens.Add(verificationToken);
            }
            else
            {
                existingToken.Token = token;
                existingToken.DateCreated = DateTime.Now;
                await UnitOfWork.VerificationTokens.Update(existingToken);
            }


        }

        static string GenerateRandomToken()
        {
            Random random = new Random();
            int tokenNumber = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
            return tokenNumber.ToString();
        }
    }


}
