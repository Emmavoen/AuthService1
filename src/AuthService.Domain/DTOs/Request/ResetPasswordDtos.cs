using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.DTOs.Request
{
    public class ResetPasswordDtos
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


    }
}
