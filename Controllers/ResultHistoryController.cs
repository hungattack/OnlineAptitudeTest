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
        public IActionResult AddNew()
        {

            return null;
        }
         public IActionResult Filter(string id,DateTime date)
        {
            var query = from rh in db.resultHistories
                        join qh in db.QuestionHistories on rh.questionHisId equals qh.Id
                        where qh.userId == id && rh.CreatedAt.Date == date.Date
                        select new ResultHistory
                        {
                            Id = rh.Id,
                            occupaionId = rh.occupaionId,
                            questionHisId = rh.questionHisId,
                            catePartId = rh.catePartId,
                            Answer = rh.Answer,
                            CreatedAt = rh.CreatedAt,
                            UpdatedAt = rh.UpdatedAt,
                        };
            var result = query.ToList();
            return Ok(result);
        }
    }
}
