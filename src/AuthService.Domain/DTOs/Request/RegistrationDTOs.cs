using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.DTOs.Request
{
    public class RegistrationDTOs
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }

        public string Username { get; set; }
        public string Bvn { get; set; }
        public bool HasBvn { get; set; }
        public string Nin { get; set; }
        public string LandMark { get; set; }
        public string AccountType { get; set; }
        
        public string Title { get; set; }
                






    }
}
