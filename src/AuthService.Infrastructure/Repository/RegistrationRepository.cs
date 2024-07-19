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
    public class RegistrationRepository :IRegistrationRepository
    {
        private readonly IAuthRegistrationService authRegistration;
        public RegistrationRepository(IAuthRegistrationService _authRegistration)
        {
            authRegistration = _authRegistration;
        }

        public async Task<string> Register(RegistrationDTOs RegDtos)
        {
            var result = await authRegistration.Register(RegDtos);
            return result;
        }
    }
}
