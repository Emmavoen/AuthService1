using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Infrastructure.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AuthServce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtValidationController : ControllerBase
    {
        private readonly IJwtValidationService _jwtValidationService;


        public JwtValidationController(IJwtValidationService jwtValidationService)
        {
            _jwtValidationService = jwtValidationService;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateJwtToken(string token)
        {
            var isValid = await _jwtValidationService.ValidateJwtToken(token);
            if(isValid)
            {
                return Ok();
            }

            return Unauthorized();
        }
    }
}