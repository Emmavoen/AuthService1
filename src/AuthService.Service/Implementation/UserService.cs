using AuthService.Domain.DTOs.Request;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using AuthService.Domain.Enums;
using AuthService.Infrastructure.Contract;
using AuthService.Service.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
//using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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


                var token = JWTGenerator.GetToken(_configuration,RegDtos.Username);

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
                    Age = CalculateAge.CalculateAgeFromDateOfBirth(RegDtos.Dob),
                    UserName = RegDtos.Username,
                    Title = RegDtos.Title,
                    AccountType = RegDtos.AccountType,
                    LandMark = RegDtos.LandMark,
                    Nin = RegDtos.Nin,
                    HasBvn = RegDtos.HasBvn,
                    Bvn = RegDtos.Bvn,
                    AccountNumber = BankAccountNumber.Generate11DigitRandomNumber(),
                    PasswordHash = Hash.HashPassword(RegDtos.Password),

                };

                var add = await UnitOfWork.Users.Add(newuser);
                var emailObject = new SendEmailConfirmation
                {
                    UserEmail = newuser.Email,
                    Token = RandomTokenGeneration.GenerateRandomToken(),
                    FirstName = newuser.FirstName,
                    Subject = "Account Registration"
                };


                var send = await SendEmail.SendConfirmationEmail(_httpClientFactory, emailObject);
                if (!send)
                {
                    return new UserResponseDetails()
                    {
                        IsSuccess = false,
                        Message = "User registration failed"
                    };
                }
                await SaveVerificationToken.SaveToken(UnitOfWork, emailObject.UserEmail, emailObject.Token, ActionTypeEnum.EmailConfirmation.ToString());
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
            var confirm = await TokenConfirmation.ConfirmToken(UnitOfWork, request.Token, request.Email, ActionTypeEnum.ResetPassword.ToString());

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
                Token = RandomTokenGeneration.GenerateRandomToken(),
                FirstName = user.FirstName
            };
            var send = await SendEmail.SendConfirmationEmail(_httpClientFactory, emailObject);
            if (send)
            {
                await SaveVerificationToken.SaveToken(UnitOfWork, emailObject.UserEmail, emailObject.Token, ActionTypeEnum.ResetPassword.ToString());
                await UnitOfWork.Save();
                return "Email successfully sent";
            }

            return " server error";
        }
        public async Task<string> EmailConfirmation(ResetPasswordDtos request)
        {
            var user = await UnitOfWork.Users.GetByColumnAsync(x => x.Email == request.Email);

            if (await TokenConfirmation.ConfirmToken(UnitOfWork, request.Token, request.Email, ActionTypeEnum.EmailConfirmation.ToString()))
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

            if (user == null)
            {
                return "Invalid Login Credentials";
            }

            if (user.Status == StatusEnum.Locked.ToString())
            {
                return "Account is Currently locked. Please Reset Your Password.";
            }

            if (user.PasswordHash != hashPassword)
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

            if (!user.EmailConfirmed)
            {
                var emailModel = new SendEmailConfirmation
                {
                    UserEmail = loginDTOs.Email,
                    Subject = "Account Registration",
                    Token = RandomTokenGeneration.GenerateRandomToken(),
                    FirstName = user.FirstName

                };
                var sendToken = await SendEmail.SendConfirmationEmail(_httpClientFactory, emailModel);
                if (sendToken)
                {
                    await SaveVerificationToken.SaveToken(UnitOfWork, emailModel.UserEmail, emailModel.Token, ActionTypeEnum.EmailConfirmation.ToString());
                    await UnitOfWork.Save();
                    return "Your Email has not been confirmed. A token has been sent for you to confirm your Email.";
                }

                return "Unknown error";
            }
            var token = JWTGenerator.GetToken(_configuration,loginDTOs.Username);


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


    }


}
