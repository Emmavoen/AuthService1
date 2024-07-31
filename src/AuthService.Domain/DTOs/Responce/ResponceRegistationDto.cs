using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace AuthService.Domain.DTOs.Responce
{
    public class ResponceRegistationDto
    {
        public string LastLogin{ get; set; }
        public string Token { get; set; }
        public string DailyLimitBalance { get; set; }
        public string AccountNumber { get; set; }
        public string UserName { get; set; }
        public string AccountName { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string AccountType { get; set; }
        public string Bvn { get; set; }
        public string Nin { get; set; }

        public string Status { get; set; }
    }
}
