﻿using AuthService.Domain.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract
{
    public interface IUserService
    {
        Task<string> Register(RegistrationDTOs RegDtos);

    }
}