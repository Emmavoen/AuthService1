using AuthService.Domain.DTOs.Request;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<bool> UserExist(RegistrationDTOs request)
        {
            return await appDbContext.Users.AnyAsync(x => x.Email == request.Email);

        }

        /*public async Task<bool> UserExist(RegistrationDTOs request)
        {
           return await appDbContext.Users.AnyAsync(x => x.Email == request.Email);
        }
    }*/
    }

}
