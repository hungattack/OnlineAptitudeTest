using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAptitudeTest.Model;
using OnlineAptitudeTest.Validation;

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
        [HttpPut]
        [Route("{id}")]
        public IActionResult SetRule(string id, [FromBody] User user)
        {
            if (id is null) return NotFound("Id is empty");
            if (user.roles == null) return NotFound("User is empty");
            ValidateOn validate = new ValidateOn(db);

            if (validate.rule(id, "update", "roof"))
            {
                User u = db.Users.SingleOrDefault(u => u.Id == user.Id);
                if (u != null)
                {
                    Roles role = db.Roles.SingleOrDefault(r => r.Id == u.RoleId);
                    if (role is null) return NotFound("Role is no found");
                    if (user.roles.Name is not null)
                    {
                        role.Name = user.roles.Name;
                    }
                    if (user.roles.Description != null)
                    {
                        role.Description = user.roles.Description;
                    }
                    if (user.roles.Permissions != null)
                    {
                        role.Permissions = user.roles.Permissions;
                    }
                    db.Roles.Update(role);
                    db.SaveChanges();
                    return Ok("Update successful");

                }
                return NotFound("User doesn't exist");
            }
            return NotFound("Authorization");
        }
        [HttpGet]
        [Route("{userID}/{offset}/{limit}/{type}/{search}")]
        public IActionResult ListingInfoAdmin(string userId, int offset, int limit, string type, string? search = null)
        {
            ValidateOn validate = new ValidateOn(db);
            if (validate.rule(userId, "read", "roof"))
            {
                if (search == "null")
                {
                    var query = from User in db.Users
                                join Roles in db.Roles
                                on User.RoleId equals Roles.Id
                                where Roles.Name == type
                                select new
                                {
                                    id = User.Id,
                                    name = User.Name,
                                    gender = User.Gender,
                                    email = User.Email,
                                    Role = Roles,
                                };

                    var results = query.Skip(offset).Take(limit).ToList();
                    return Ok(results);
                }
                else
                {
                    var query = from User in db.Users
                                join Roles in db.Roles
                                on User.RoleId equals Roles.Id
                                where Roles.Name == type && User.Name.Contains(search)
                                select new
                                {
                                    id = User.Id,
                                    name = User.Name,
                                    gender = User.Gender,
                                    email = User.Email,
                                    Role = Roles,
                                };

                    var results = query.Skip(offset).Take(limit).ToList();
                    return Ok(results);
                }
            }
            return NotFound("Authorization");
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
                Gender = u.Gender,
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
