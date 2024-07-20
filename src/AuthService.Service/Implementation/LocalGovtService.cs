using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Request;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
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
        public IUnitOfWork _unitOfWork;
        public LocalGovtService(AppDbContext _appDbContext, IUnitOfWork unitOfWork)
        {
            appDbContext = _appDbContext;
            _unitOfWork = unitOfWork;
        }
        /*public List<ResponceLocalGovtDto> GetAllLocalGovtById(int id)
        {
            var result = appDbContext.LocalGovernmentAreas.Where(x => x.StateId == id).Select(x => new ResponceLocalGovtDto
            {
                Id = x.Id,
                Name = x.Name,

            }).ToList();
                return result;
        }*/

        public async Task<List<ResponceLocalGovtDto>> GetAllLocalGovtById(int id)
        {
            List<ResponceLocalGovtDto> listResponse = new();
            var result = await _unitOfWork.LocalGovernmentArea.GetAllByColumnAsync(x=> x.StateId == id);

            if (result == null)
            {
                return listResponse;
            }

            foreach (var item in result) { 

            var response = new ResponceLocalGovtDto {
                
                Id = item.Id ,
                Name = item.Name,
            };

                listResponse.Add(response);

            }

            return listResponse;


        }


    }
}
