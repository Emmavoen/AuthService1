using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract.Repository
{
    public interface ILoginRepository
    {
        Task<string> Login(LoginDTOs loginDTOs);
    }
}
