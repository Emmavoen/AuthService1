using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class LocalGovtAreaRepository : ILocalGovernmentAreaRepository
    {
        private readonly ILocalGovtService localGovtService;

        public LocalGovtAreaRepository(ILocalGovtService _localGovtService)
        {
            localGovtService = _localGovtService;
        }


        public List<ResponceLocalGovtDto> GetAllLocalGovtById(int id)
        {


            var result = localGovtService.GetAllLocalGovtById(id);

            return result;
            /*var record = appDbContext.LocalGovernmentAreas.Where(x => x.StateId == id).ToList();
            return record;*/
        }
    }
}
