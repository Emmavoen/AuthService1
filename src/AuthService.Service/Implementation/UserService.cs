﻿using AuthService.Domain.DTOs.Request;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service.Implementation
{
    public class UserService : IUserService
    {
        public IUnitOfWork UnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public UserService(IUnitOfWork _unitOfWork, IConfiguration configuration, UserManager<AppUser> userManager)
        {
            UnitOfWork = _unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> Register(RegistrationDTOs RegDtos)
        {

            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, RegDtos.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = GetToken(authClaims);

            var finalToken = new JwtSecurityTokenHandler().WriteToken(token);
            //check if email already exists
            //var user_exist = await _userManager.FindByEmailAsync(RegDtos.Email);
            var user_exist = await UnitOfWork.Users.UserExist(RegDtos);

            if (user_exist)
            {
                return $"User with the email {RegDtos.Email} already exists. please Login";

            }

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

            };

            var add = await UnitOfWork.Users.Add(newuser);
            var save = await UnitOfWork.Save();

            if (save <= 0)
            {
                return "Unable to register user";

            }

            var response = new ResponceRegistationDto()
            {
                LastLogin = "Now",
                Token = finalToken,
                DailyLimitBalance = "",
                AccountNumber = newuser.AccountNumber,
                UserName = newuser.UserName,
                AccountName = newuser.FirstName + "" + newuser.MiddleName + "" + newuser.LastName,
                Title = newuser.Title,
                Gender = newuser.Gender,
                AccountType = newuser.AccountType,
                Bvn = newuser.Bvn,
                Nin = newuser.Nin,

                Status = "",


            };




            var outcome = "User successfully Created";
            return outcome;


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

    }
}
