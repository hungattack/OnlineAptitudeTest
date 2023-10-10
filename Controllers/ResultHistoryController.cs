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
    }
}
