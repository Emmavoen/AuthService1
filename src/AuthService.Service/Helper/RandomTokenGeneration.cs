using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Service.Helper
{
    public class RandomTokenGeneration
    {
        public static string GenerateRandomToken()
        {
            Random random = new Random();
            int tokenNumber = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
            return tokenNumber.ToString();
        }
    }
}