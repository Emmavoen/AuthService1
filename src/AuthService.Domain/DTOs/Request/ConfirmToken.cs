using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.DTOs.Request
{
    public class ConfirmToken
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
    }
}
