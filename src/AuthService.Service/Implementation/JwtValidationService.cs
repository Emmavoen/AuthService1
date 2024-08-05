using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.Infrastructure.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Service.Implementation
{
    public class JwtValidationService : IJwtValidationService
    {
        private readonly IConfiguration _configuration;

        public JwtValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> ValidateJwtToken(string token)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Optionally, you can extract claims and other information from the validatedToken here
                var jwtToken = (JwtSecurityToken)validatedToken;
                //var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                var userNameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                var userName = userNameClaim != null ? userNameClaim.Value : "No Name claim";

                // Return true if validation passes
                return true;
            }
            catch
            {
                // Return false if validation fails
                return false;
            }

        }
    }
}