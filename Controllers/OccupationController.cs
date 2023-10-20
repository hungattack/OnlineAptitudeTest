using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAptitudeTest.Model;
using OnlineAptitudeTest.Validation;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                if (cateParts.Any())
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
        [Route("{id}")]
        public IActionResult GetInfo(string id)
        {
            if (id is null) return NotFound("Id is empty");

            List<Info> infos = db.infos.Where(u => u.occupationId == id).ToList();
            // Serialize your object using the JsonSerializerOptions
            return Ok(infos);
        }
        [HttpGet]
        public Boolean Duppicated(string name, string id)
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
            if (occupation is null) { return NotFound("occupation is empty!"); };

            Occupation oc = new Occupation();
            User isUser = db.Users.SingleOrDefault(u => u.Id == occupation.userId);
            if (isUser is null) return NotFound("User is not existing!");
            Roles roles = db.Roles.SingleOrDefault(r => r.Id == isUser.RoleId);
            if (roles is null) return NotFound("You have no any permission");
            if (roles.Name != "admin") return NotFound("You're not admin");
            if (!roles.Permissions.Contains("create")) return NotFound("You have no any permission to create");

            if (!ModelState.IsValid)
            {
                return Ok(occupation.Name);
            }
            if (Duppicated(occupation.Name, occupation.userId))
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

            catePart.Id = guidStringsCate;
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

            return Ok(new { id = oc.Id, id_f = catePart.Id, name_f = catePart.Name, id_s = catePartq.Id, name_s = catePartq.Name, id_t = catePartqs.Id, name_t = catePartqs.Name });
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id is null) return NotFound("Id is empty");
            Occupation occupation = await db.Occupations.SingleAsync(u => u.Id == id);
            if (occupation == null) return NotFound("Occupation is null");
            List<CateParts> cateParts = await db.CateParts.Where(c => c.OccupationId == occupation.Id).ToListAsync();
            if (cateParts.Any())
            {
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
            return Ok(new { id = id });
        }
        [HttpDelete]
        [Route("{id}/{occupationId}/{managerId}")]
        public async Task<IActionResult> DeleteInfo(int id, string occupationId, string managerId)
        {
            if (id < 0) return NotFound("Id is empty");
            if (occupationId is null) return NotFound("occupationId is empty");
            if (managerId is null) return NotFound("managerId is empty");
            Occupation occupation = await db.Occupations.SingleAsync(u => u.Id == occupationId);
            if (occupation == null) return NotFound("Occupation is null");
            Info info = await db.infos.FirstOrDefaultAsync(i => i.Id == id && i.occupationId == occupationId && i.managerId == managerId);
            if (info == null) return NotFound("info is null");
            db.infos.Remove(info); db.SaveChanges();
            return Ok("ok");
        }
        [HttpPost]
        public IActionResult AddInfo([FromBody] Info info)
        {
            if (info.occupationId == null) return NotFound("occupationId not found");
            if (info.Requirement == null) return NotFound("Requirement not found");
            if (info.Introduction == null) return NotFound("Introduction not found");
            if (info.Position == null) return NotFound("Position no found");
            if (info.Address == null) return NotFound("Address no found");
            if (info.Contact == null) return NotFound("Contact no found");
            if (info.Name == null) return NotFound("Name no found");
            if (info.Company == null) return NotFound("Company no found");
            if (info.managerId == null) return NotFound("managerId no found");
            ValidateOn validate = new ValidateOn(db);
            if (!validate.rule(info.managerId, "create")) return NotFound("You have no any permissions to create");
            Occupation oc = db.Occupations.SingleOrDefault(o => o.Id == info.occupationId);
            if (oc == null) return NotFound("Occupation not found");
            Info ifo = new Info();
            ifo.managerId = info.managerId;
            ifo.occupationId = info.occupationId;
            ifo.Position = info.Position;
            ifo.Address = info.Address;
            ifo.Company = info.Company;
            ifo.Contact = info.Contact;
            ifo.Name = info.Name;
            ifo.Requirement = info.Requirement;
            ifo.Introduction = info.Introduction;
            db.infos.Add(ifo);
            db.SaveChanges();
            return Ok("ok");
        }
        [HttpGet]
        [Route("{name}")]
        public IActionResult GetListing(string? name)
        {
            List<Occupation> occupations = new List<Occupation>();
            bool oo = name == "null" ? true : false;
            List<Occupation> occupation = db.Occupations.Where(o => oo == false ? o.Name.Contains(name) && o.Active == true : o.Active == true).Include(u => u.user).Select(o => new Occupation
            {
                // Include other properties of Occupation that you need
                Id = o.Id,
                Name = o.Name,
                CreatedAt = o.CreatedAt,
                user = new User
                {
                    Name = o.user.Name,
                    Gender = o.user.Gender,
                    Id = o.user.Id,

                    // Add more user properties if needed
                }
            })
                 .ToList();
            foreach (Occupation o in occupation)
            {
                List<Info> infos = db.infos.Where(i => i.occupationId == o.Id).ToList();
                o.infos = infos;
                occupations.Add(o);
            }

            return Ok(occupations);
        }
        [HttpPatch]
        [Route("{occupationId}/{userId}")]
        public IActionResult Active(string occupationId, string userId)
        {
            Occupation isO = db.Occupations.SingleOrDefault(o => o.Id == occupationId && o.userId == userId);
            if (isO is not null)
            {
                isO.Active = !isO.Active;
                db.Occupations.Update(isO); db.SaveChanges();
                return Ok(new { status = true, active = isO.Active });
            }
            return NotFound("Occupation is not found when update active");
        }

    }
}
