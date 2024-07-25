using AuthService.Domain.DTOs.Request;
using AuthService.Domain.DTOs.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract
{
    public interface IUserService
    {
        Task<UserResponseDetails> Register(RegistrationDTOs RegDtos);
        Task<string> Login(LoginDTOs loginDTOs);

        Task<string> UpdatePassword(UpdatePasswordDTOs loginDTOs);

    }
}
