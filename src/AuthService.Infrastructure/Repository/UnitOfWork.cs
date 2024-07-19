using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;
       public IUserRepository Users {  get; }
        public UnitOfWork(AppDbContext _appDbContext, IUserRepository userRepository)
        {
            appDbContext = _appDbContext;
            Users = userRepository;
        }

        public async  Task<int> Save()
        {
            return await appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            appDbContext.Dispose();
        }
    }
}
