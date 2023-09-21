using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly AptitudeTestDbText db;
        public UserController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("{id}")]
        public async  Task<IActionResult> GetById(string id)
        {

            
            return Ok(id);
        }
    }
}
