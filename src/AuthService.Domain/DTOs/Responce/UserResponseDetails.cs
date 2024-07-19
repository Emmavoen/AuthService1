using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.DTOs.Responce
{
    public class UserResponseDetails
    {

        public string Message { get; set; }
        public ResponceRegistationDto ResponseDetails { get; set; }
        public bool IsSuccess { get; set; }
    }
}
