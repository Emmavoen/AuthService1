using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Request;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service.Implementation
{
    public class AuthLoginService : IAuthLoginService
    {
        private readonly SignInManager<AppUser> SignInManager;
        private readonly IConfiguration _configuration;

        public AuthLoginService(SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            SignInManager = signInManager;
            _configuration = configuration;
        }



        public async Task<string> Login(LoginDTOs loginDTOs)
        {

            var result = await SignInManager.PasswordSignInAsync(loginDTOs.Username, loginDTOs.Password, false, false);

            if (!result.Succeeded)
            {
                return "failed";
            }

            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, loginDTOs.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature));

            return token;
        }

        // throw new NotImplementedException();
    }
}



