using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Request;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IAuthLoginService authLoginService;
        public LoginRepository(IAuthLoginService _authLoginService)
        {
            authLoginService = _authLoginService;
        }

       

        public async Task<string> Login(LoginDTOs loginDTOs)
        {
            var result = await authLoginService.Login(loginDTOs);
            return result;
        }
    }
}
