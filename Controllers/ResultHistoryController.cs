using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;

namespace OnlineAptitudeTest.Controllers

{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class ResultHistoryController : ControllerBase
    {
        private readonly AptitudeTestDbText? db;
        public ResultHistoryController(AptitudeTestDbText? db)
        {
            this.db = db;
        }
        [HttpPost]
        public IActionResult AddNew([FromBody] UpdataQuestion questions)
        {
            if (questions.OccupationId is null) return NotFound("OccupationId is empty");
            if (questions.UserId is null) return NotFound("UserId is empty");
            if (questions.CatePartId is null) return NotFound("CatePartId is empty");
            if (questions.Answer is null) return NotFound("Answer is empty");
            if (questions.QuestionId is null) return NotFound("QuestionId is empty");
            QuestionHistory userHistory = db.QuestionHistories.FirstOrDefault(q => q.userId == questions.UserId && q.occupationId == questions.OccupationId);
            if (userHistory is null)
            {
                Guid myGuids = Guid.NewGuid(); // generate ids
                string guidStrings = myGuids.ToString();
                DateTime currentDate = DateTime.Now;
                QuestionHistory history = new QuestionHistory();
                history.Id = guidStrings;
                history.userId = questions.UserId;
                history.occupationId = questions.OccupationId;
                history.UpdatedAt = currentDate;
                db.QuestionHistories.Add(history);
                db.SaveChanges();
                ResultHistory resultHistory = db.resultHistories.SingleOrDefault(r => r.catePartId == questions.CatePartId && r.occupaionId == questions.OccupationId);
                if (resultHistory is null)
                {
                    ResultHistory result = new ResultHistory();
                    result.Answer = questions.Answer;
                    result.questionHisId = history.Id;
                    result.questionId = questions.QuestionId;
                    result.occupaionId = questions.OccupationId;
                    result.catePartId = questions.CatePartId;
                    db.resultHistories.Add(result);
                    db.SaveChanges();
                    return Ok("ok");

                }
                return NotFound("resultHistory had existed");
            }
            Condidate candidate = db.Condidates.SingleOrDefault(c => c.userId == questions.UserId && c.ReTest == true);
            ResultHistory resultHistoryr = db.resultHistories.SingleOrDefault(r => r.catePartId == questions.CatePartId && r.questionId == questions.QuestionId && r.occupaionId == questions.OccupationId);
            if (resultHistoryr is null)
            {
                ResultHistory result = new ResultHistory();
                result.Answer = questions.Answer;
                result.questionHisId = userHistory.Id;
                result.questionId = questions.QuestionId;
                result.occupaionId = questions.OccupationId;
                result.catePartId = questions.CatePartId;
                db.resultHistories.Add(result);
                db.SaveChanges();
                return Ok("ok");

            }
            if (candidate is not null)
            {
                resultHistoryr.Answer = questions.Answer; // second test again 
                db.resultHistories.Update(resultHistoryr);
                db.SaveChanges();
                return Ok("reTest");
            }
            return NotFound(new { status = 0, message = "resultHistory had existed" });
        }
    }
}
