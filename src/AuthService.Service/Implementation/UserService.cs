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
                var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, RegDtos.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

                var token = GetToken(authClaims);

                var finalToken = new JwtSecurityTokenHandler().WriteToken(token);



                //check if email already exists
                var user_exist = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == RegDtos.Email);

                

                if (user_exist != null)
                {
                    _logger.LogError( "User already Exist");
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

                var emailObject = new EmailConfirmation
                {
                    UserEmail = newuser.Email,
                    Token = await _userManager.GenerateEmailConfirmationTokenAsync(newuser),
                    FirstName = newuser.FirstName
                };

                var add = await UnitOfWork.Users.Add(newuser);
                await SendConfirmationEmail(emailObject);
                var save = await UnitOfWork.Save();

               /* if (save < 1)
                {
                    _logger.LogError( "Notsaved");
                    return new UserResponseDetails()
                    {
                        Message = $"Server Error",
                        IsSuccess = false
                    };

                }*/
              
            


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


                _logger.LogError( "User Successfully created");

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


        public async Task<string> Login(LoginDTOs loginDTOs)
        {
             
            var hashPassword = Hash.HashPassword(loginDTOs.Password);
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == loginDTOs.Email);


            if (user == null)
            {
                return "User does not exist";
            }

            if (user.Email != loginDTOs.Email)
            {
                return "Invalid Email Address";
            }
            if (user.PasswordHash != hashPassword)
            {
                return "Wrong Password";
            }

            var httpclient = _httpClientFactory.CreateClient();
            var emailModel = new
            {
                To = loginDTOs.Email,
                Subject = "Account Signin",
                Body = $"There is a new signin In your account"

            };
            var sendEmail = await httpclient.PostAsJsonAsync("https://localhost:7168/api/Notification", emailModel);

            var authClaims = new List<Claim>
            {
            new(ClaimTypes.Name, loginDTOs.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };

            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> UpdatePassword(UpdatePasswordDTOs updateDTOs)
        {
            var hashOldPassword = Hash.HashPassword(updateDTOs.OldPassword);
            var hashNewPassword = Hash.HashPassword(updateDTOs.NewPassword);
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == updateDTOs.Email);
            if (user == null)
            {
                return  "Invalid Email Address";
            }

           if(user.PasswordHash != hashOldPassword)
            {
                return "Invalid old password";
            }

           user.PasswordHash = hashNewPassword;
           await UnitOfWork.Users.Update(user);
           var result =   await UnitOfWork.Save();

            if(result < 1)
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




        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature));

            return token;
        }
        private async Task SendConfirmationEmail(EmailConfirmation request)
        {// create a object for email confirmation
            
            
            var httpclient = _httpClientFactory.CreateClient();
            var emailModel = new
            {
                To = request.UserEmail,
                Subject = "Account Registration",
                Body = $"Hello {request.FirstName}, here is ur verification token: {request.Token}"

            };
            var sendEmail = await httpclient.PostAsJsonAsync("https://localhost:7168/api/Notification", emailModel);

            var verificationToken = new VerificationToken
            {
                Email = request.UserEmail,
                Token = request.Token,
                ActionType = ActionTypeEnum.EmailConfirmation.ToString()

            };
            await UnitOfWork.VerificationTokens.Add(verificationToken);
           

        }
    }

   /* public  class EmailConfirmation
    {
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string  Token { get; set; }
    }*/
}
