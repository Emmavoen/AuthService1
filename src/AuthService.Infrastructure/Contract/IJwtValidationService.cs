using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract
{
    public interface IJwtValidationService
    {
         Task<bool> ValidateJwtToken(string token);
    }
}