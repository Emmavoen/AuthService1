using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Responce;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace AuthService.Service.Implementation
{
    public class StatesService :IStateService
    {
        private readonly AppDbContext appDbContext;

        public StatesService(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public List<ResponceStateDto> StateNamesByCountryId(int id)
        {
            List<ResponceStateDto> state = new();

            var result = appDbContext.States.Where(x => x.CountryId == id);
            if (result == null)
            {
                return state;
            }
            foreach (var req in result)
            {
                var st = new ResponceStateDto
                {
                    Id = req.Id,
                    Name = req.Name
                };
                state.Add(st);
            }
            return state;
        }

    }

    
}
