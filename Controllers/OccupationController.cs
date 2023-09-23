using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAptitudeTest.Model;
using System.Text.Json.Serialization;
using System.Text.Json;

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
            foreach (Occupation occupation in occupations)
            {
                List<CateParts> cateParts = db.CateParts.Where(c => c.OccupationId == occupation.Id).ToList();
               if(cateParts.Any())
                {
                    occupation.Cates = cateParts;
                }
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Add other serialization options as needed
            };

            // Serialize your object using the JsonSerializerOptions
            string json = JsonSerializer.Serialize(occupations, options);
            return Ok(json);
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

            CateParts catePart = new CateParts();
            Guid myGuidsq = Guid.NewGuid();
            string guidStringsCate = myGuidsq.ToString();

            catePart.Id  = guidStringsCate;
            catePart.Name = "General knowledge";
            catePart.OccupationId = oc.Id;
            catePart.TimeOut = 20;
            catePart.TimeType = "Minute";
            db.CateParts.Add(catePart);

            CateParts catePartq = new CateParts();
            Guid myGuidsc = Guid.NewGuid();
            string guidStringsCateq = myGuidsc.ToString();

            catePartq.Id = guidStringsCateq;
            catePartq.Name = "Mathematics";
            catePartq.OccupationId = oc.Id;
            catePartq.TimeOut = 20;
            catePartq.TimeType = "Minute";
            db.CateParts.Add(catePartq);

            CateParts catePartqs = new CateParts();
            Guid myGuidsx = Guid.NewGuid();
            string guidStringsCatee = myGuidsx.ToString();

            catePartqs.Id = guidStringsCatee;
            catePartqs.Name = "Computer Technology";
            catePartqs.OccupationId = oc.Id;
            catePartqs.TimeOut = 20;
            catePartqs.TimeType = "Minute";
            db.CateParts.Add(catePartqs);

            db.SaveChanges();
            return Ok(new {id = oc.Id});
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null) return NotFound("Id is empty");
            Occupation occupation = await db.Occupations.SingleAsync(u => u.Id == id);
            if (occupation == null) return NotFound("Occupation is null");
            List<CateParts>  cateParts = await db.CateParts.Where(c => c.OccupationId == occupation.Id).ToListAsync();
            if(cateParts.Any()) {
                foreach (CateParts cate in cateParts)
                {
                    if (cate.Id is not null)
                    {
                        List<Question> questions = await db.Questions.Where(q => q.PartId == cate.Id).ToListAsync();
                        foreach (Question question in questions)
                        {
                             db.Questions.Remove(question);
                            db.SaveChanges();
                        }
                    }
                    db.CateParts.Remove(cate); 
                    db.SaveChanges();
                }
            }
            db.Occupations.Remove(occupation); db.SaveChanges();
            return Ok(new {id = id});
        }
    }
}
