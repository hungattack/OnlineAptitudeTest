using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class OccupationController : ControllerBase
    {
        private readonly AptitudeTestDbText db;
        public OccupationController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(string id)
        {
            if (id is null) return NotFound("Id is empty");
            List<Occupation> occupations = db.Occupations.Where(u => u.userId == id).ToList();
            return Ok(occupations);
        }
        [HttpGet]
        public Boolean Duppicated(string name,string id)
        {
            Boolean flag = false;
            Occupation occupation = db.Occupations.Where(cat => cat.userId == id && cat.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (occupation is not null) { flag = true; }
            return flag;
        }
        [HttpPost]
        public IActionResult AddNew([FromBody] Occupation occupation)
        {
            Guid myGuids = Guid.NewGuid(); // generate ids
            string guidStrings = myGuids.ToString();
            DateTime currentDate = DateTime.Now;
            if (occupation is  null) { return NotFound("occupation is empty!"); };

            Occupation oc = new Occupation();
            bool isUser = db.Users.Any(u => u.Id == occupation.userId);
            if (!isUser) return NotFound("User is not existing!");
            if (!ModelState.IsValid)
            {
                return Ok(occupation.Name);
            }
            if (Duppicated(occupation.Name,occupation.userId))
            {
                ModelState.AddModelError("DupplicateCatePartsName", "CateParts Name is dupplicate");
                return Ok(ModelState);
            }
            if (occupation.Name is null) return NotFound("The field Name cannot be null!");
            oc.Id = guidStrings;
            oc.Name = occupation.Name;
            oc.userId = occupation.userId;
            oc.CreatedAt = currentDate;
            db.Add(oc);
            db.SaveChanges();
            return Ok(1);
        }
    }
}
