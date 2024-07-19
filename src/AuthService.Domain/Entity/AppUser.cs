using Microsoft.AspNetCore.Identity;
using System;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable ClassNeverInstantiated.Global

namespace AuthService.Domain.Entity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime  Dob { get; set; }
        public string Gender { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string LocalGovernmentArea { get; set; }
        public string Age { get; set; }

        public string LastLogin { get; set; }
        public bool HasBvn { get; set; }
        public string LastModified { get; set; }
        public string AccountNumber { get; set; }
       
        public string DateCreated { get; set; }
        public string Title { get; set; }
        public string LandMark { get; set; }
        public string AccountType { get; set; }
        public string Bvn { get; set; }
        public string Nin { get; set; }

        public string Status { get; set; }





    }
}
