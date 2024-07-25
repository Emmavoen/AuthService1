using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.DTOs.Request
{
    public class UpdatePasswordDTOs
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
