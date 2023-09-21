using OnlineAptitudeTest.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Text.Json;
using System.ComponentModel;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/Api/")]
   
    public class QuestionController : ControllerBase
    {
        private readonly AptitudeTestDbText? db;
        public QuestionController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IEnumerable<Question> List()
        {
            IEnumerable<Question> questions = db.Questions.ToArray();
            //if (!Question.Any()) {return NotFound();}
            return questions;
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public IActionResult GetById(string id)
        {
            Question question = db.Questions.Find(id);
            if (question is null) { return NotFound(); }
            return Ok(question);
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{name}")]
        public Boolean Dupplicated(string name)
        {
            Boolean flag = false;
            Question question = db.Questions.Where(que => que.QuestionName.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (question is not null) { flag = true; }
            return flag;
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public Boolean ExitCatePart(string id)
        {
            Boolean flag = false;
            CateParts _exitcate = db.CateParts.Where(exc => exc.Id.ToLower().Equals(id.ToLower())).FirstOrDefault();
            if (_exitcate is not null) { flag = true; }
            return flag;
        }
        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult Addnew(Question question)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }
            if (Dupplicated(question.QuestionName))
            {
                ModelState.AddModelError("DupplicateQuestionName", "Question Name is dupplicated!");
                return Ok(ModelState);
            }
            if (!ExitCatePart(question.PartId))
            {
                return NotFound("Catepart is not exit");
            }

            db.Add(question);
            db.SaveChanges();
            return Ok(question);
        }
        [HttpDelete]
        [Route("[Controller]/[Action]/{id}")]
        public IActionResult Delete(string id)
        {
            Question question = db.Questions.Find(id);
            if(question is null) { return NotFound(); };
            db.Questions.Remove(question);  
            db.SaveChanges();
            return Ok("Delete Complete");

        }
        [HttpPut]
        [Route("[Controller]/[Action]")]
        public IActionResult Update(Question question)
        {
            Question _question = db.Questions.Find(question.Id);
            
            if (_question is null)
            {
                return NotFound("Question not found");
            }
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }
            if (!ExitCatePart(question.PartId))
            {
                return NotFound("Catepart is not exit");
            }
            _question.PartId = question.PartId;
            _question.QuestionName = question.QuestionName;
            _question.Answer = question.Answer;
            _question.Point = question.Point;
            _question.TimeOut = question.TimeOut;
            _question.UpdatedAt = DateTime.Now;

            db.Questions.Update(_question);
            db.SaveChanges();
            return Ok("Question update complete");
        }
    }
}
