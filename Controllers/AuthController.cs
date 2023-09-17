using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class AuthController : ControllerBase
    {
        private readonly AptitudeTestDbText db;
        public AuthController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult Register([FromBody] User user)
        {
            Guid myGuids = Guid.NewGuid();
            string guidStrings = myGuids.ToString();
            Roles roles = new Roles();

            roles.Id = guidStrings;
            roles.Name = "user";
            roles.Description = "User is only acessed test";
            roles.Permissions = "[read]";
            user.RoleId = roles.Id;
            

            Guid myGuid = Guid.NewGuid();
            string guidString = myGuid.ToString();
            user.Id = guidString;
           
            return Ok(new {user , roles});
        }
    }
}
