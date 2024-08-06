using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract;

namespace AuthService.Service.Helper
{
    public class SaveVerificationToken
    {
          public static async Task SaveToken(IUnitOfWork unitOfWork,string email, string token, string actionType)
        {

            var existingToken = await unitOfWork.VerificationTokens.GetByColumnAsync(x => x.Email == email && x.ActionType == actionType);
            if (existingToken == null)
            {
                var verificationToken = new VerificationToken
                {
                    Email = email,
                    Token = token,
                    ActionType = actionType,
                    DateCreated = DateTime.Now
                };
                await unitOfWork.VerificationTokens.Add(verificationToken);
            }
            else
            {
                existingToken.Token = token;
                existingToken.DateCreated = DateTime.Now;
                await unitOfWork.VerificationTokens.Update(existingToken);
            }


        }
    }
}