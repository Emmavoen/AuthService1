using AuthService.Domain.DTOs.Request;
using AuthService.Infrastructure.Contract;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IValidator<RegistrationDTOs> Validator;
        public UserController(IUserService _userService, IValidator<RegistrationDTOs> validator)
        {
            userService = _userService;
            Validator = validator;
        }

        

        [HttpPost]
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

            var result = await userService.Register(RegDtos);
            return Ok( result);
        }
    }
}
