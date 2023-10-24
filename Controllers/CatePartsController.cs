using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;
using OnlineAptitudeTest.Validation;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class CatePartsController : ControllerBase
    {
        private readonly AptitudeTestDbText db;
        public CatePartsController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IEnumerable<CateParts> List()
        {
            IEnumerable<CateParts> cateparts = db.CateParts.ToArray();
            return cateparts;
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public IActionResult GetById(string id)
        {
            List<CateParts> cateparts = db.CateParts.Where(u => u.OccupationId == id).ToList();
            if (cateparts == null) { return NotFound("CatePart is ampty"); }
            return Ok(cateparts);
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{name}")]
        public bool Duppicated(string name, string id)
        {
            bool flag = false;
            CateParts catepart = db.CateParts.Where(cat => cat.OccupationId == id && cat.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (catepart is not null) { flag = true; }
            return flag;
        }
        [HttpPost]
        [Route("[Controller]/[Action]/${userId}")]
        public IActionResult AddNew(string userId, [FromBody] CateParts catepart)
        {
            ValidateOn validate = new ValidateOn(db);
            if (!validate.rule(userId, "create", "admin")) return NotFound(new { status = 0, message = "Authorization" });
            Guid myGuids = Guid.NewGuid(); // generate ids
            string guidStrings = myGuids.ToString();
            DateTime currentDate = DateTime.Now;
            bool isCate = db.Occupations.Any(o => o.Id == catepart.OccupationId);

            if (isCate)
            {
                CateParts cate = new CateParts();
                bool isOc = db.Occupations.Any(u => u.Id == catepart.OccupationId);
                if (!isOc) return NotFound("Occupation is not existing!");
                if (!ModelState.IsValid)
                {
                    return Ok(catepart.Name);
                }
                if (Duppicated(catepart.Name, catepart.OccupationId))
                {
                    return Ok("CateParts Name is dupplicate");
                }
                if (catepart.Name is null) return NotFound("The field Name cannot be null!");
                cate.Id = guidStrings;
                cate.Name = catepart.Name;
                cate.TimeOut = 20;
                cate.TimeType = "Minute";
                cate.OccupationId = catepart.OccupationId;
                cate.CreatedAt = currentDate;
                db.Add(cate);
                db.SaveChanges();
                return Ok(new { cate, status = 1 });
            }
            return Ok(isCate);

        }
        [HttpDelete]
        [Route("[Controller]/[Action]/{id}/{occupationId}/{userId}")]
        public IActionResult Delete(string id, string occupationId, string userId)
        {
            ValidateOn validate = new ValidateOn(db);
            if (!validate.rule(userId, "delete", "admin")) return NotFound(new { status = 0, message = "Authorization" });
            if (id is null) return NotFound("Id can not be null");
            if (occupationId is null) return NotFound("occupationId can not be null");
            CateParts isCate = db.CateParts.Single(c => c.Id == id && c.OccupationId == occupationId);
            if (isCate is null) { return NotFound("Cate is empty"); };
            var question = db.Questions.Where(q => q.PartId == isCate.Id).ToList();
            db.CateParts.Remove(isCate);
            foreach (var question1 in question)
            {
                db.Questions.Remove(question1);
            }
            db.SaveChanges();
            return Ok(new { id = isCate.Id });

        }
        [HttpPut]
        [Route("[Controller]/[Action]/{userId}")]
        public IActionResult Update(string userId, CateParts catepart)
        {
            ValidateOn validate = new ValidateOn(db);
            if (!validate.rule(userId, "update", "admin")) return NotFound(new { status = 0, message = "Authorization" });
            if (catepart.Id is null) return NotFound("Id not found");
            if (catepart.OccupationId is null) return NotFound("OccupationId not found");
            CateParts cateparts = db.CateParts.SingleOrDefault(c => c.Id == catepart.Id && c.OccupationId == catepart.OccupationId);
            if (cateparts is null)
            {
                return NotFound("Catepart not found");
            }

            if (catepart.Name is not null)
            {
                cateparts.Name = catepart.Name;
            }
            if (catepart.TimeOut is not null)
            {
                cateparts.TimeOut = catepart.TimeOut;
            }
            if (catepart.TimeType is not null)
            {
                cateparts.TimeType = catepart.TimeType;
            }
            cateparts.UpdatedAt = DateTime.Now;

            db.CateParts.Update(cateparts);
            db.SaveChanges();
            return Ok("Catepart update complete");
        }
    }
}
