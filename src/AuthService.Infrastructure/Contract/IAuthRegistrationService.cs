using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Request;
using AuthService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract
{
    public interface IAuthRegistrationService
    {
        Task<string> Register(RegistrationDTOs RegDtos);
        //int Saveuser(AppUser user);

    }
}
