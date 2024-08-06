using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Infrastructure.Contract;

namespace AuthService.Service.Helper
{
    public static class TokenConfirmation
    {
        
        public static async Task<bool> ConfirmToken(IUnitOfWork unitOfWork,string token, string Email, string actionType)
        {
            var userWithToken = await unitOfWork.VerificationTokens.GetByColumnAsync(x => x.Email == Email && x.ActionType == actionType);
            if (userWithToken == null)
            {
                return false;
            }
            // Define the expiration duration
            var expirationDuration = TimeSpan.FromMinutes(3);

            // Calculate the time elapsed since the token was created
            var timeElapsed = DateTime.Now - userWithToken.DateCreated;
            if (userWithToken.Email == Email && userWithToken.Token == token && timeElapsed < expirationDuration)
            {
                return true;
            }
            return false;
        }
    }
}