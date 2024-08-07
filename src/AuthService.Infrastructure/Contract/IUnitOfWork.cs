﻿using AuthService.Infrastructure.Contract.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ILocalGovernmentAreaRepository LocalGovernmentArea { get; }

        IStatesRepository statesRepository{ get; }
        IVerificationTokenRepository VerificationTokens { get; }
        Task<int> Save();
    }
}
