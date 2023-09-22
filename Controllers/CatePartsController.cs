using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OnlineAptitudeTest.Model;
using System.Collections;

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
        public bool Duppicated(string name,string id)
        {
            bool flag = false;
            CateParts catepart = db.CateParts.Where(cat => cat.OccupationId == id && cat.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (catepart is not null) { flag = true; }
            return flag;
        }
        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult AddNew([FromBody] CateParts catepart)
        {
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
                    ModelState.AddModelError("DupplicateCatePartsName", "CateParts Name is dupplicate");
                    return Ok(ModelState);
                }
                if (catepart.Name is null) return NotFound("The field Name cannot be null!");
                cate.Id = guidStrings;
                cate.Name = catepart.Name;
                cate.OccupationId = catepart.OccupationId;
                cate.CreatedAt = currentDate;
                db.Add(cate);
                db.SaveChanges();
                return Ok(1);
            }
            return Ok(0);
           
        }
        [HttpDelete]
        [Route("[Controller]/[Action]/{id}")]
        public IActionResult Delete(string id)
        {
            CateParts cateParts = db.CateParts.Find(id);
            if (cateParts is null) { return NotFound(); };
            var question = db.Questions.Where(c => c.PartId == cateParts.Id).ToList();    
            if(question is null) { return NotFound(); } 
            foreach (var question1 in question)
            {
                db.Questions.Remove(question1);
            }
            db.CateParts.Remove(cateParts);
            db.SaveChanges();
            return Ok("Delete cpmplete");

        }
        [HttpPut]
        [Route("[Controller]/[Action]")]
        public IActionResult Update(CateParts catepart)
        {
            CateParts cateparts = db.CateParts.Find(catepart.Id);
            if (cateparts is null)
            {
                return NotFound("Catepart not found");
            }
            cateparts.Name = catepart.Name;
            cateparts.CreatedAt = DateTime.Now;

            db.CateParts.Update(cateparts);
            db.SaveChanges();
            return Ok("Catepart update complete");
        }
    }
}
