using AuthService.Infrastructure.Contract;
using AuthService.Infrastructure.Contract.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalGovtAreaController : ControllerBase
    {
        private readonly ILocalGovtService area;
        public LocalGovtAreaController(ILocalGovtService _area)
        {
            area = _area;
        }




        [HttpGet("{id}")]
        
        public async  Task<IActionResult> GetAllLocalGovtById(int id)
        {

            var result = await area.GetAllLocalGovtById(id);
            return Ok(result);

        }
    }
}
