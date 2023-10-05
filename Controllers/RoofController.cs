using Microsoft.AspNetCore.Mvc;
using OnlineAptitudeTest.Model;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class RoofController : ControllerBase
    {
        readonly private AptitudeTestDbText db;
        public RoofController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]

        public IActionResult Index()
        {
            return null;
        }
    }
}
