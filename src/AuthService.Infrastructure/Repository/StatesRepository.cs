using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository
{
    public class StatesRepository : IStatesRepository
    {
        // private readonly AppDbContext appDbContext;
        private readonly  IStateService stateservice;
        public StatesRepository(IStateService _stateService )
        {
            //appDbContext = _appDbContext;
            stateservice = _stateService;

        }


        public List<ResponceStateDto> StateNamesByCountryId(int id)
        {

            var result = stateservice.StateNamesByCountryId(id);
            return result;
        }
        /* public List<StateDto> StateNamesByCountryId(int id)
        {
            List<StateDto> state = new();

            var result = appDbContext.States.Where(x => x.CountryId == id).ToList();
            if (result == null)
            {
                return state;
            }
            foreach (var req in result)
            {
                var st = new StateDto
                {
                    Id = req.Id,
                    Name = req.Name
                };
                state.Add(st);
            }
            return state;
        }*/
    }

    /*public class StateDto
    { 
        public int Id { get; set;}
        public string Name { get; set;}
    }*/
}