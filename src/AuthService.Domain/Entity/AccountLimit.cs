using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entity
{
    public class AccountLimit
    {
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string DailyLimit { get; set; }
    }
}
