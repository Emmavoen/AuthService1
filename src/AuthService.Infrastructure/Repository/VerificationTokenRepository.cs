using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class VerificationTokenRepository : GenericRepository<VerificationToken>, IVerificationTokenRepository
    {
        public VerificationTokenRepository(AppDbContext context) : base(context)
        {

        }
    }
}
