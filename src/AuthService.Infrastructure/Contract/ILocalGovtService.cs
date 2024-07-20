using AuthService.Domain.DTOs;
using AuthService.Domain.DTOs.Responce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract
{
    public interface ILocalGovtService
    {
        Task<List<ResponceLocalGovtDto>> GetAllLocalGovtById(int id);
    }
}
