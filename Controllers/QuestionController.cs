using OnlineAptitudeTest.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Text.Json;
using System.ComponentModel;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/")]
   
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
            if (id is null) return NotFound("Id is null");
            List<Question> questions = db.Questions.Where(q => q.PartId == id).ToList();
            return Ok(questions);
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{name}")]
        public Boolean Dupplicated(string name,string cateId)
        {
            Boolean flag = false;
            Question question = db.Questions.Where(que => que.PartId == cateId && que.QuestionName.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (question is not null) { flag = true; }
            return flag;
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public bool ExitCatePart(string id)
        {
            Boolean flag = false;
            bool _exitcate = db.CateParts.Any(exc => exc.Id == id);
           
            return _exitcate;
        }
        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult Addnew(Question question)
        {
            Question qs = new Question();
            if (question.QuestionName is null) return NotFound("Question name cannot be null");
            if (question.PartId is null) return NotFound("PartId cannot be null");
            if (question.Answer is null) return NotFound("Answer cannot be null");
            if (question.AnswerArray is null) return NotFound("AnswerArray cannot be null");
            if (question.Point < 1) return NotFound("Point cannot be null");

            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }
            if (Dupplicated(question.QuestionName,question.PartId))
            {
                ModelState.AddModelError("DupplicateQuestionName", "Question Name is dupplicated!");
                return Ok(ModelState);
            }
            if (!ExitCatePart(question.PartId))
            {
                return NotFound("Catepart is not exit");
            }
            Guid myGuidsx = Guid.NewGuid();
            string guidStrings = myGuidsx.ToString();
            qs.Id = guidStrings;
            qs.QuestionName = question.QuestionName;
            qs.Answer = question.Answer;
            qs.AnswerArray = question.AnswerArray;
            qs.PartId = question.PartId;
            qs.Point = question.Point;
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
            _question.UpdatedAt = DateTime.Now;

            db.Questions.Update(_question);
            db.SaveChanges();
            return Ok("Question update complete");
        }
    }
}
