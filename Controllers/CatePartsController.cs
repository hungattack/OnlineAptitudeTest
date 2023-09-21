using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using OnlineAptitudeTest.Model;
using System.Collections;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/Api/")]
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
            CateParts catepart = db.CateParts.Find(id);
            if (catepart == null) { return NotFound(); }
            return Ok(catepart);
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{name}")]
        public Boolean Duppicated(string name)
        {
            Boolean flag = false;
            CateParts catepart = db.CateParts.Where(cat => cat.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (catepart is not null) { flag = true; }
            return flag;
        }
        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult AddNew(CateParts catepart)
        {
            CateParts cateParts = db.CateParts.Find(catepart.Id);
            bool isUser = db.Users.Any(u => u.Id == catepart.userId);
            if (!isUser) return NotFound("User is not existing!");
            if (cateParts is not null) { return NotFound("Catepart Id is exit"); };
            if (!ModelState.IsValid)
            {
                return Ok(catepart.Name);
            }
            if (Duppicated(catepart.Name))
            {
                ModelState.AddModelError("DupplicateCatePartsName", "CateParts Name is dupplicate");
                return Ok(ModelState);
            }
            db.Add(catepart);
            db.SaveChanges();
            return Ok(catepart);
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
