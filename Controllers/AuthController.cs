using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAptitudeTest.Model;
using OnlineAptitudeTest.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class AuthController : ControllerBase
    {
        private readonly AptitudeTestDbText db;
        public AuthController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            bool isUser = await db.Users.AnyAsync(u => u.Email == user.Email);
            if(isUser) return NotFound("This Email already exists!");
            if(user is null) return NotFound("The Data can not be empty!");
            User user1 = new User();
            Guid myGuids = Guid.NewGuid(); // generate ids
            string guidStrings = myGuids.ToString();
            if (guidStrings.Length > 50) return BadRequest("Id must be from 1 to 50 characters");
            DateTime currentDate = DateTime.Now;
            // roles
            bool isRole = await db.Roles.AnyAsync(r => r.Id == guidStrings);
            if(isRole) return NotFound("This Role already exists!");
            Roles roles = new Roles();
            roles.Id = guidStrings;
            roles.Name = "user";
            roles.Description = "The user is only tested and visited";
            roles.Permissions = "[read]";
            // user
            user1.RoleId = roles.Id;

            Guid myGuid = Guid.NewGuid();
            string guidString = myGuid.ToString();
            if (guidString.Length > 50) return BadRequest("Id must be from 1 to 50 characters");

            user1.Id = guidString;
            
            if (!ValidateOn.ForStringLength(user.Name, 5, 40))
            {
                return BadRequest( "The field name must be from 5 to 40 characters" ); 
            } 
            user1.Name = user.Name;
            if (!ValidateOn.ForEmail(user.Email))
            {
                return BadRequest("The field Email is not valid!");
            }
            user1.Email = user.Email;
            if (!ValidateOn.ForStringLength(user.Password, 5, 50))
            {
                return BadRequest("The field Password must be from 5 to 50 characters"); 
            }
            user1.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user1.Gender = user.Gender;
            user1.CreatedAt = currentDate;
            user1.UpdatedAt = currentDate;
            await db.Roles.AddAsync(roles);
            await db.Users.AddAsync(user1);
            await db.SaveChangesAsync();
            return Ok(1);
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (user.Email is null || user.Password is null) return NotFound("Email or Password is not valid!");
            User u = await db.Users.Include(u => u.roles).SingleOrDefaultAsync(u => u.Email.Equals(user.Email));
            if (u != null)
            {
                if (BCrypt.Net.BCrypt.Verify(user.Password, u.Password))
                {
                   Roles t = db.Roles.Single(r => r.Id == u.RoleId);
                    u.roles = t;
                    u.Password = null;
                    return Ok(u);
                }
                return NotFound("Email or Password is not valid!");

            }
            return NotFound("Email or Password is not valid!");
        }

    }
}
