using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetById(string id)
        {
            if (id is null) return BadRequest("Authorization");
            User u = await db.Users.Where(u => u.Id == id).Include(u => u.roles).Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                RoleId = u.RoleId,
                roles = new Roles
                {
                    Name = u.roles.Name,
                    Description = u.roles.Description,
                    Permissions = u.roles.Permissions
                }
            }).FirstOrDefaultAsync();
            Roles t = await db.Roles.SingleOrDefaultAsync(r => r.Id == u.RoleId);
            if (t is null) return NotFound("Role is not found");
            u.roles = t;
            return Ok(u);
        }
    }
}
