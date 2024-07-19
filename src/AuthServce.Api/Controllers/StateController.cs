using AuthService.Infrastructure.Contract.Repository;
using AuthService.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStatesRepository states;

        public StateController(IStatesRepository _states)
        {
            states = _states;
        }


        [HttpGet]
        [Route("{id}")]
        public IActionResult GetStateByCountryid(int id)
        {
            if (id == 0)
            {
                return BadRequest();

            }
            var result = states.StateNamesByCountryId(id);
            return Ok(result);
        }

        

    }
}
