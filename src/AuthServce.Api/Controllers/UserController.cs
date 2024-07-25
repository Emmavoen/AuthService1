using AuthService.Domain.DTOs.Request;
using AuthService.Infrastructure.Contract;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegistrationDTOs> Validator;
        public UserController(IUserService userService, IValidator<RegistrationDTOs> validator)
        {
            _userService = userService;
            Validator = validator;
        }

        

        [HttpPost]
       [ Route("Regisrer")]
        public async Task<IActionResult> Register(RegistrationDTOs RegDtos)
        {

            var validationResult = await Validator.ValidateAsync(RegDtos);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);



            }

            var result = await _userService.Register(RegDtos);
            return Ok( result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTOs request)
        {
            var result = await _userService.Login(request);
            return Ok( result );
        }

        [HttpPost]
        [Route("UpdatePassword")]
        public async Task<IActionResult> Updatepassword(UpdatePasswordDTOs request)
        {
            var result = await _userService.UpdatePassword(request);
            return Ok( result );
        }
    }
}
