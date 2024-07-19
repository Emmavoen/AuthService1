using AuthService.Domain.DTOs.Request;
using AuthService.Domain.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Validator
{
    public class UserValidation : AbstractValidator<RegistrationDTOs>
    {
        public UserValidation() {
        
        RuleFor(x =>x.Bvn).Matches(@"^\d{11}$").WithMessage("Phone number must be exactly 11 digits long and contain only numbers");
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\d{11}$").WithMessage("Phone number must be exactly 11 digits long and contain only numbers");
            RuleFor(x => x.Nin).NotEmpty().Matches(@"^\d{11}$").WithMessage("Phone number must be exactly 11 digits long and contain only numbers");
            RuleFor(x=>x.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required").Equal(x=>x.Password);
            RuleFor(x => x.LandMark).NotEmpty().WithMessage("LandMark is required"); ;
            RuleFor(x => x.AccountType).NotEmpty().WithMessage("AccountType is required"); ;
            RuleFor(x => x.Dob).NotEmpty().WithMessage("Dob is required"); ;
            RuleFor(x => x.LGA).NotEmpty().WithMessage("LGA is required"); ;
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required"); ;
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required"); ;
            RuleFor(x => x.MiddleName).NotEmpty().WithMessage("MiddleName is required"); ;
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required"); ;
            RuleFor(x => x.AccountType).NotEmpty().WithMessage("AccountType is required"); ;
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required"); ;









        }
    }
}
