using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class RoomController : ControllerBase
    {
        readonly private AptitudeTestDbText db;
        public RoomController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult getRoom(string id)
        {
            if (id == null) return NotFound("Room code is not found!");

            Occupation occupation = db.Occupations.Where(o => o.Id == id).Select(o => new Occupation
            {
                Id = o.Id,
                Name = o.Name,
                userId = o.userId
            }).FirstOrDefault();
            if (occupation == null) return NotFound(" Occupation is not found");
            User user = db.Users.Where(u => u.Id == occupation.userId).Select(o => new User
            {
                Id = o.Id,
                Name = o.Name,
                Gender = o.Gender
            }).FirstOrDefault();
            List<CateParts> cateParts = db.CateParts.Where(c => c.OccupationId == occupation.Id).ToList();
            foreach (var catePart in cateParts)
            {
                List<Question> questions = db.Questions.Where(q => q.PartId == catePart.Id).ToList();
                catePart.Questions = questions;
            }
            occupation.user = user;
            occupation.Cates = cateParts;
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Add other serialization options as needed
            };

            // Serialize your object using the JsonSerializerOptions
            string json = JsonSerializer.Serialize(occupation, options);
            return Ok(json);
        }
    }
}
