using Microsoft.AspNetCore.Mvc;

namespace BLA.UserFlow.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok("");
        }
    }
}
