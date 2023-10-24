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
    public class ResultHistoryController : ControllerBase
    {
        private readonly AptitudeTestDbText? db;
        public ResultHistoryController(AptitudeTestDbText? db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("{type}")]
        public IActionResult ListingCate(string type)
        {
            if (type == "new")
            {
                List<QuestionHistory> questionHistorys = db.QuestionHistories.OrderBy(q => q.CreatedAt).ToList();
                foreach (QuestionHistory q in questionHistorys)
                {
                    // haven't finish
                }
            }
            return null;
        }
        [HttpGet]
        [Route("{userId}/{manaId}")]
        public IActionResult ListingExams(string userId, string manaId)
        {
            User user = db.Users.SingleOrDefault(u => u.Id == manaId);
            if (user != null)
            {
                Roles roles = db.Roles.SingleOrDefault(r => r.Id == user.RoleId);

                if (roles != null && roles.Name == "admin" && roles.Permissions.Contains("read"))
                {
                    List<QuestionHistory> questionHistories = db.QuestionHistories.Include(q => q.occupation).Where(q => q.userId == userId && q.occupation.userId == manaId).ToList();
                    foreach (QuestionHistory questionHistory in questionHistories)
                    {
                        List<ResultHistory> resultHistories = db.resultHistories.Where(r => r.occupaionId == questionHistory.occupationId && r.questionHisId == questionHistory.Id).ToList();
                        questionHistory.Results = resultHistories;
                    }
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        // Add other serialization options as needed
                    };

                    // Serialize your object using the JsonSerializerOptions
                    string json = JsonSerializer.Serialize(questionHistories, options);
                    return Ok(json);
                }
            }

            return NotFound("Authorization");

        }
        [HttpGet]
        [Route("{userId}/{manaId}")]
        public IActionResult FirstYearExam(string userId, string manaId)
        {
            ValidateOn validate = new ValidateOn(db);
            User user = db.Users.SingleOrDefault(u => u.Id == manaId);
            if (user != null)
            {
                Roles roles = db.Roles.SingleOrDefault(r => r.Id == user.RoleId);

                if (validate.rule(userId, "read", "admin"))
                {
                    List<QuestionHistory> questionHistories = db.QuestionHistories.Where(q => q.userId == userId && q.occupation.userId == manaId).OrderBy(q => q.CreatedAt).Take(1).ToList();
                    return Ok(questionHistories[0].CreatedAt);
                }
                return NotFound(new { status = 0, masseage = "Authorization" });
            }

            return NotFound("Authorization");

        }
        [HttpGet]
        [Route("{id}/{date}")]
        public IActionResult Filter(string id, DateTime date)
        {
            var query = from rh in db.resultHistories
                        join qh in db.QuestionHistories on rh.questionHisId equals qh.Id
                        where qh.userId == id && qh.CreatedAt.Date <= date.Date
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
        [HttpPost]
        public IActionResult AddNew([FromBody] UpdataQuestion questions)
        {
            if (questions.OccupationId is null) return NotFound("OccupationId is empty");
            if (questions.UserId is null) return NotFound("UserId is empty");
            if (questions.Point < 0) return NotFound("Point is empty");
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
                history.CreatedAt = currentDate;
                history.pointAll = questions.Point;
                db.QuestionHistories.Add(history);
                db.SaveChanges();
                return Ok("resultHistory just had written");
            }
            return Ok("resultHistory had written");
        }
        [HttpPost]
        public async Task<IActionResult> AddNewResult([FromBody] UpdataQuestion questions)
        {
            if (questions.OccupationId is null) return NotFound("OccupationId is empty");
            if (questions.UserId is null) return NotFound("UserId is empty");
            if (questions.CatePartId is null) return NotFound("CatePartId is empty");
            if (questions.Answer is null) return NotFound("Answer is empty");

            QuestionHistory userHistory = await db.QuestionHistories.FirstOrDefaultAsync(q => q.userId == questions.UserId && q.occupationId == questions.OccupationId);
            if (userHistory is not null)
            {
                ResultHistory resultHistory = await db.resultHistories.FirstOrDefaultAsync(r => r.catePartId == questions.CatePartId && r.occupaionId == questions.OccupationId);
                if (resultHistory is null)
                {
                    DateTime currentDate = DateTime.Now;
                    ResultHistory result = new ResultHistory();
                    result.Answer = questions.Answer;
                    result.questionHisId = userHistory.Id;
                    result.occupaionId = questions.OccupationId;
                    result.CreatedAt = currentDate;
                    result.catePartId = questions.CatePartId;
                    await db.resultHistories.AddAsync(result);
                    await db.SaveChangesAsync();
                    return Ok("ok");

                }
                Condidate candidate = await db.Condidates.FirstOrDefaultAsync(c => c.userId == questions.UserId);
                ResultHistory resultHistoryr = await db.resultHistories.FirstOrDefaultAsync(r => r.catePartId == questions.CatePartId && r.occupaionId == questions.OccupationId);
                if (resultHistoryr is null)
                {
                    ResultHistory result = new ResultHistory();
                    result.Answer = questions.Answer;
                    result.questionHisId = userHistory.Id;
                    result.occupaionId = questions.OccupationId;
                    result.catePartId = questions.CatePartId;
                    await db.resultHistories.AddAsync(result);
                    await db.SaveChangesAsync();
                    return Ok("ok");

                }
                if (candidate.ReTest is not null)
                {
                    DateTime currentDate = DateTime.Now;
                    resultHistoryr.Answer = questions.Answer; // second test again 
                    resultHistoryr.UpdatedAt = currentDate;
                    db.resultHistories.Update(resultHistoryr);
                    await db.SaveChangesAsync();
                    return Ok("reTest");
                }
                return NotFound(new { status = 0, message = "resultHistory had existed" });
            }
            return NotFound(new { status = 1, message = "QuestionHistory had existed" });

        }
        [HttpDelete]
        [Route("{occupationId}/{userId}/{manaId}")]
        public IActionResult Delete(string occupationId, string userId, string manaId)
        {
            if (occupationId is null) return NotFound("occupationId is empty");
            if (userId is null) return NotFound("userId is empty");
            if (manaId is null) return NotFound("manaId is empty");
            User user = db.Users.SingleOrDefault(u => u.Id == manaId);
            if (user != null)
            {
                ValidateOn validate = new ValidateOn(db);
                if (!validate.rule(manaId, "delete", "admin"))
                {
                    QuestionHistory questionHistorys = db.QuestionHistories.FirstOrDefault(q => q.userId == userId && q.occupationId == occupationId);
                    if (questionHistorys is not null)
                    {
                        List<ResultHistory> resultHistories = db.resultHistories.Where(r => r.occupaionId == questionHistorys.occupationId && r.questionHisId == questionHistorys.Id).ToList();
                        db.resultHistories.RemoveRange(resultHistories);
                        db.QuestionHistories.Remove(questionHistorys);
                        db.SaveChanges();
                        return Ok("ok");
                    }
                }
                return NotFound(new { status = 0, masseage = "Authorization" });

            }
            return NotFound("Authorization!");
        }
    }
}
