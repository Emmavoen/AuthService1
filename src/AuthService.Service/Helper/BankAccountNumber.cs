using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Service.Helper
{
    public class BankAccountNumber
    {
         public static string Generate11DigitRandomNumber()
        {
            Random random = new Random();
            string result = string.Empty;

            // Generate the first 6 digits
            result += random.Next(100000, 1000000).ToString("D6");

            // Generate the remaining 5 digits
            result += random.Next(10000, 100000).ToString("D5");

            return result;
        }
    }
}