using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Request;
using AuthService.Domain.Entity;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Configuration;
using AuthService.Infrastructure.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service.Implementation
{
    public class AuthRegistrationService : IAuthRegistrationService
    {
        private readonly UserManager<AppUser> _userManager;

        
        private readonly IConfiguration _configuration;

        public AuthRegistrationService(UserManager<AppUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
           
        }

        public async Task<string> Register(RegistrationDTOs RegDtos)
        {
            //check if email already exists
            var user_exist = await _userManager.FindByEmailAsync(RegDtos.Email);


            if (user_exist != null)
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
                Title= RegDtos.Title,
                AccountType = RegDtos.AccountType,
                LandMark = RegDtos.LandMark,
                Nin = RegDtos.Nin,
                HasBvn = RegDtos.HasBvn,
                Bvn = RegDtos.Bvn,




            };

            var result = await _userManager.CreateAsync(newuser, RegDtos.Password);

            if (!result.Succeeded)
            {
                var error =  result.Errors.Select(e => e.Description);
                var errorstring = string.Join("; ", error);
                return $"Unable to register user. Errors :{errorstring}" ;
            }

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

       
    }



}
