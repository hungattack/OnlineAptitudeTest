using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAptitudeTest.Model;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class RolesController : ControllerBase
    {
        public readonly AptitudeTestDbText db;
        public RolesController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IEnumerable<Roles> List()
        {
            IEnumerable<Roles> roles = db.Roles.ToArray();
            return roles;
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Add(Roles roles)
        {
            Guid myGuids = Guid.NewGuid(); // generate ids
            string guidStrings = myGuids.ToString();
            if (guidStrings.Length > 50) return BadRequest("Id must be from 1 to 50 characters");
            bool isRole = await db.Roles.AnyAsync(r => r.Id == guidStrings);
            if (isRole) return NotFound("This Role already exists!");
            Roles _role = new Roles();
            _role.Id = guidStrings;
            _role.Name = roles.Name;
            _role.Description = roles.Description;
            _role.Permissions = roles.Permissions;
            db.Roles.AddAsync(_role);
            db.SaveChangesAsync();
            return Ok("Add Roles is success!");
        }
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult FindById(string id)
        {
            Roles roles = db.Roles.FirstOrDefault(r => r.Id == id);
            if (roles == null)
            {
                return Ok(NotFound());
            }
            return Ok(roles);
        }
        [HttpPut]
        [Route("/api/[Controller]/[Action]")]
        public IActionResult Update(Roles roles)
        {
            Roles _role = db.Roles.Find(roles.Id);
            if (_role is null)
            {
                return NotFound("Role not found");
            }
            _role.Name = roles.Name;
            _role.Description = roles.Description;
            _role.Permissions = roles.Permissions;
            db.Roles.Update(_role);
            db.SaveChanges();
            return Ok("Update complete!");
        }
    }
}
