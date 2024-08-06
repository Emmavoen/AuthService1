using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class StatesRepository : GenericRepository<State>,IStatesRepository
    {
       
        public StatesRepository(AppDbContext context) : base(context)
        {

        }

        
    }

    
}