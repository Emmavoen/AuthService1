using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class LocalGovtAreaRepository : GenericRepository<LocalGovernmentArea>,ILocalGovernmentAreaRepository
    {
       // private readonly ILocalGovtService localGovtService;

        public LocalGovtAreaRepository( AppDbContext context) : base(context) 
        {
            //localGovtService = _localGovtService;
        }

       
      
    }
}
