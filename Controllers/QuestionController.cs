using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;

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
        [Route("[Controller]/[Action]/{id}/{type}")]
        public IActionResult GetById(string id, string type)
        {
            if (type.Contains("array") || type.Contains("string"))
            {
                if (id is null) return NotFound("Id is null");
                List<Question> questions = db.Questions.Where(q => q.PartId == id && q.AnswerType == type).OrderByDescending(q => q.CreatedAt).ToList();
                return Ok(questions);

            }
            return NotFound("AnswerType not found");
        }
        [HttpGet]
        [Route("[Controller]/[Action]/{name}")]
        public Boolean Dupplicated(string name, string cateId)
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
            if (question.AnswerType is null) return NotFound("AnswerType cannot be null");
            if (question.PartId is null) return NotFound("PartId cannot be null");
            if (question.Answer is null && question.AnswerType.Contains("array")) return NotFound("Answer cannot be null");
            if (question.AnswerArray is null && question.AnswerType.Contains("array")) return NotFound("AnswerArray cannot be null");
            if (question.Point < 1) return NotFound("Point cannot be null");

            if (!ExitCatePart(question.PartId))
            {
                return NotFound("Catepart is not exit");
            }
            DateTime currentDate = DateTime.Now;
            Guid myGuidsx = Guid.NewGuid();
            string guidStrings = myGuidsx.ToString();
            qs.Id = guidStrings;
            qs.QuestionName = question.QuestionName;
            if (!question.AnswerType.Contains("string"))
            {
                qs.Answer = question.Answer;
            }

            qs.AnswerType = question.AnswerType;
            qs.AnswerArray = question.AnswerArray;
            qs.PartId = question.PartId;
            qs.Point = question.Point;
            qs.CreatedAt = currentDate;
            db.Add(qs);
            db.SaveChanges();
            return Ok("ok");
        }
        [HttpDelete]
        [Route("[Controller]/[Action]/{id}/{partId}")]
        public IActionResult Delete(string id, string partId)
        {
            Question question = db.Questions.Single(q => q.Id == id && q.PartId == partId);
            if (question is null) { return NotFound(); };
            db.Questions.Remove(question);
            db.SaveChanges();
            return Ok(true);

        }
        [HttpPut]
        [Route("[Controller]/[Action]")]
        public IActionResult Update([FromBody] UpdataQuestion updataQuestion)
        {
            bool isOc = db.Occupations.Any(o => o.Id == updataQuestion.OccupationId && o.userId == updataQuestion.UserId);
            if (isOc)
            {
                bool isCate = db.CateParts.Any(c => c.Id == updataQuestion.PartId);
                if (isCate)
                {
                    Question question = db.Questions.SingleOrDefault(q => q.Id == updataQuestion.Id);
                    if (question != null)
                    {
                        DateTime currentDate = DateTime.Now;
                        question.QuestionName = updataQuestion.QuestionName;
                        if (updataQuestion.Type == "array")
                        {
                            question.AnswerArray = updataQuestion.AnswerArray;
                            question.Answer = updataQuestion.Answer;
                        }
                        question.Point = updataQuestion.Point;
                        question.UpdatedAt = currentDate;
                        db.Questions.Update(question);
                        db.SaveChanges();
                        return Ok(true);
                    }
                    return NotFound("Question not found");
                }
                return NotFound("Catepart is not exit");
            }
            return NotFound("Occupation is not exit");

        }
    }
}
