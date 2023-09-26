using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;
using OnlineAptitudeTest.Validation;
using System.Runtime.Intrinsics.X86;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly AptitudeTestDbText db;
                    DateTime currentDate = DateTime.Now;
        public UserController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IEnumerable<User> List()
        {
            IEnumerable<User> users = db.Users.ToArray();
            //if (!Question.Any()) {return NotFound();}
            return users;
        }
        [HttpGet]
        [Route("{id}")]
        public async  Task<IActionResult> GetById(string id)
        {

            
            return Ok(id);
        }
        [HttpPut]
        [Route("/api/[Controller]/[Action]")]
        public async Task<IActionResult> Update(User user)
        {
            DateTime currentDate = DateTime.Now;
            User _user = db.Users.Find(user.Id);

            if (_user is null)
            {
                return NotFound("User not found");
            }
            Guid myGuid = Guid.NewGuid();
            string guidString = myGuid.ToString();
            if (guidString.Length > 50) return BadRequest("Id must be from 1 to 50 characters");
            _user.Gender = user.Gender;
            _user.Email = user.Email;
            if (!ValidateOn.ForEmail(_user.Email))
            {
                return BadRequest("The field Email is not valid!");
            }

            _user.Name = user.Name;
            if (!ValidateOn.ForStringLength(_user.Name, 5, 40))
            {
                return BadRequest("The field name must be from 5 to 40 characters");
            }

            _user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _user.RoleId = user.RoleId;
            _user.roles = user.roles;
            _user.UpdatedAt = currentDate;

            db.Users.Update(_user);
            db.SaveChanges();
            return Ok("User update complete");
        }
    }
}
