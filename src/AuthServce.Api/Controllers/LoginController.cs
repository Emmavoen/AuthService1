using AuthService.Domain.DTOs.Request;
using AuthService.Infrastructure.Contract.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository loginRepository;
        public LoginController(ILoginRepository _loginRepository)
        {
            loginRepository = _loginRepository;
        }


        [HttpPost]
        public async Task<string> Login(LoginDTOs request)
        {
            var result = await loginRepository.Login(request);
            return result;
        }
        
    }
}
