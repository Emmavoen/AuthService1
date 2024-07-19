using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Request;
using AuthService.Domain.DTOs.Responce;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Service.Implementation
{
    public class LocalGovtService : ILocalGovtService
    {

        private readonly AppDbContext appDbContext;
        public LocalGovtService(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }
        public List<ResponceLocalGovtDto> GetAllLocalGovtById(int id)
        {
            var result = appDbContext.LocalGovernmentAreas.Where(x => x.StateId == id).Select(x => new ResponceLocalGovtDto
            {
                Id = x.Id,
                Name = x.Name,

            }).ToList();
                return result;
        }
        
    }
}
