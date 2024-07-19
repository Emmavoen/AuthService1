using AuthService.Infrastructure.Contract.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalGovtAreaController : ControllerBase
    {
        private readonly ILocalGovernmentAreaRepository area;
        public LocalGovtAreaController(ILocalGovernmentAreaRepository _area)
        {
            area = _area;
        }




        [HttpGet("{id}")]
        
        public IActionResult GetAllLocalGovtById(int id)
        {

            var result = area.GetAllLocalGovtById(id);
            return Ok(result);

        }
    }
}
