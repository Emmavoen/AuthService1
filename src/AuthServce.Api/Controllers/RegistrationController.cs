using AuthService.Domain.DTOs.Request;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Configuration;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        /* private readonly UserManager<IdentityUser> _userManager;
         private readonly JwtConfig _jwtConfig;*/

        private readonly IRegistrationRepository RegRepo;
        private readonly IValidator<RegistrationDTOs> _validator;



        public RegistrationController(IRegistrationRepository _RegRepo, IValidator<RegistrationDTOs> validator)
        {
         //_jwtConfig = jwtConfig;
         RegRepo = _RegRepo;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Register( RegistrationDTOs requestDto)
        {
            //validate incoming request
            /* if (ModelState.IsValid)
             {*/
            var validationResult = await _validator.ValidateAsync(requestDto);
            if (!validationResult.IsValid)
            {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                


            }

            var result = await RegRepo.Register(requestDto);
            return Ok(result);
        
        /* }


          return BadRequest();*/
    }



    }
}
