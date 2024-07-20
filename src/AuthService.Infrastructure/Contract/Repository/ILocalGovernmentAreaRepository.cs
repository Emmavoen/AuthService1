using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Responce;
using AuthService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract.Repository
{
    public interface ILocalGovernmentAreaRepository: IGenericRepository<LocalGovernmentArea>
    {
        //List<ResponceLocalGovtDto> GetAllLocalGovtById(int id);
    }
}
