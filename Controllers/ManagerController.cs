using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineAptitudeTest.Model;
using OnlineAptitudeTest.Validation;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class ManagerController : ControllerBase
    {
        private readonly AptitudeTestDbText db;
        public ManagerController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] RegisterManager manager)
        {
            try
            {
                if (manager == null) return NotFound();
                if(!ValidateOn.ForStringLength(manager.Name,5,40)) return BadRequest("The field name must be from 5 to 40 characters");
                if (manager.userId is null) return NotFound("Id can't be null!");
                if (!ValidateOn.ForEmail(manager.Email)) return BadRequest("The field Email is not valid!");
                if (manager.PhoneNumber.ToString().Length > 11 || manager.PhoneNumber.ToString().Length < 9) return BadRequest($"{manager.PhoneNumber.ToString().Length} The field Phone number must be from 10 to 11 characters");
                if (manager.Address is null) return NotFound("Address can't be null!");
                bool u = await db.Users.AnyAsync(u => u.Id == manager.userId);
                if (!u) return NotFound();
                bool isMa = await db.Managers.AnyAsync(m => m.userId == manager.userId);
                if (!isMa)
                {
                    DateTime currentDate = DateTime.Now;
                    RegisterManager register = new RegisterManager();
                    register.Email = manager.Email;
                    register.Name = manager.Name;
                    register.PhoneNumber = manager.PhoneNumber;
                    register.Address = manager.Address;
                    register.userId = manager.userId;
                    register.CreatedAt = currentDate;
                    await db.Managers.AddAsync(register);
                    await db.SaveChangesAsync();
                    return Ok(new {  message = "Send request succeed" });
                }
                return Ok(new { message = "You have sent a request before!" });
            }
            catch (DbUpdateException ex)
            {
                // Check the inner exception for more details
                if (ex.InnerException is SqlException sqlException)
                {
                    // Log or handle the SQL-specific exception
                    Console.WriteLine($"SQL Exception: {sqlException.Message}");
                }
                else
                {
                    // Handle other types of exceptions
                    Console.WriteLine($"Error: {ex.Message}");
                }

                // Return an appropriate error response
                return StatusCode(500, "An error occurred while saving the entity changes.");
            }
        }
    }
}
