using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAptitudeTest.Model;
using OnlineAptitudeTest.Validation;

namespace OnlineAptitudeTest.Controllers
{
    [ApiController]
    [Route("/api/[Controller]/[Action]")]
    public class CandidateController : ControllerBase
    {
        readonly private AptitudeTestDbText db;
        public CandidateController(AptitudeTestDbText db)
        {
            this.db = db;
        }
        [HttpGet]
        [Route("{userID}")]
        public IActionResult ListingRegisters(string userId)
        {
            ValidateOn validate = new ValidateOn(db);
            if (validate.rule(userId, "read"))
            {
                List<Condidate> candidates = db.Condidates.Include(o => o.occupation).Where(c => c.managerId == userId).ToList();
                return Ok(candidates);
            }
            return NotFound("Authorization");
        }
        [HttpGet]
        [Route("{userID}")]
        public IActionResult ListingFinish(string userId)
        {
            User user = db.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Roles roles = db.Roles.SingleOrDefault(r => r.Id == user.RoleId);

                if (roles != null && roles.Name == "admin" && roles.Permissions.Contains("read"))
                {
                    List<Condidate> candidates = db.Condidates.Include(o => o.occupation).Where(c => c.managerId == userId && c.Start == "end").ToList();
                    return Ok(candidates);
                }
            }

            return NotFound("Authorization");
        }
        [HttpGet]
        [Route("{name}/{cate}/{manaId}")]
        public IActionResult SearchByName(string name, string cate, string manaId)
        {

            ValidateOn validateOn = new ValidateOn(db);
            if (validateOn.rule(manaId, "read"))
            {
                if (cate == "finish")
                {
                    return Ok(db.Condidates.Include(o => o.occupation).Where(c => c.managerId == manaId && c.Name.Contains(name) && c.Start == "end").ToList());
                }
                return Ok(db.Condidates.Include(o => o.occupation).Where(c => c.managerId == manaId && c.Name.Contains(name)).ToList());

            }
            return NotFound("Authorization!");
        }
        [HttpPost]
        public IActionResult AddNew([FromBody] Condidate condidate)
        {
            if (condidate == null) return NotFound("Condidate no found");
            if (condidate.Email is null) return NotFound("Email is not found");
            if (condidate.Experience is null) return NotFound("Experience is not found");
            if (condidate.Name is null) return NotFound("Name is not found");
            if (condidate.PhoneNumber is null) return NotFound("Phone Number is not found");
            if (condidate.userId is null) return NotFound("UserId is not found");
            if (condidate.occupationId is null) return NotFound("occupationId is not found");
            if (condidate.Education is null) return NotFound("Education is not found");
            if (condidate.managerId is null) return NotFound("ManagerId is not found");
            if (condidate.BirthDay is null) return NotFound("BirthDay is not found");
            bool cdd = db.Condidates.Any(c => c.userId == condidate.userId && c.managerId == condidate.managerId && c.Name == condidate.Name);
            if (!cdd)
            {
                User isM = db.Users.SingleOrDefault(u => u.Id == condidate.managerId);
                if (isM is not null)
                {
                    bool isD = db.Roles.Any(u => u.Id == isM.RoleId && u.Name == "admin" && u.Permissions.Contains("create"));
                    if (isD)
                    {
                        Condidate cd = new Condidate();
                        cd.userId = condidate.userId;
                        cd.managerId = condidate.managerId;
                        cd.occupationId = condidate.occupationId;
                        cd.Name = condidate.Name;
                        cd.Email = condidate.Email;
                        cd.Experience = condidate.Experience;
                        cd.PhoneNumber = condidate.PhoneNumber;
                        cd.BirthDay = condidate.BirthDay;
                        cd.Education = condidate.Education;
                        cd.Start = "ready";
                        cd.ReTest = null;
                        db.Condidates.Add(cd);
                        db.SaveChanges();
                        return Ok("Add successful");
                    }
                }
                return NotFound("Manager doesn't exist!");
            }
            return NotFound("Candidate was existing or Name!");

        }
        [HttpDelete]
        [Route("{id}/{userId}")]
        public IActionResult Delete(int id, string userId)
        {
            if (id == null || userId == null) return NotFound("id or userId are not found!");
            Condidate condidate = db.Condidates.SingleOrDefault(c => c.Id == id && c.userId == userId);
            if (condidate == null) return NotFound("Candidate is not found");
            db.Condidates.Remove(condidate);
            db.SaveChanges();
            return Ok("ok");
        }
        [HttpPut]
        [Route("{manaId}/{userId}")]
        public IActionResult Finish(string manaId, string userId)
        {
            if (manaId == null || userId == null) return NotFound("manaId or userId are not found!");
            Condidate condidate = db.Condidates.FirstOrDefault(c => c.managerId == manaId && c.userId == userId);
            if (condidate == null) return NotFound("Candidate is not found");
            condidate.ReTest = null;
            condidate.Start = "end";
            db.Condidates.Update(condidate);
            db.SaveChanges();
            return Ok("ok");
        }
        [HttpPut]
        public IActionResult Generate([FromBody] Condidate condidate)
        {
            if (condidate.UserName is null) return NotFound("UserName is not found");
            if (condidate.Id < 0) return NotFound("Id is not found");
            if (condidate.Password is null) return NotFound("Password is not found");
            if (condidate.managerId is null) return NotFound("managerId is not found");
            if (condidate.occupationId is null) return NotFound("occupationId is not found");
            if (condidate.userId is null) return NotFound("userId is not found");
            if (condidate.Note is null) return NotFound("Note is not found");
            Condidate c = db.Condidates.FirstOrDefault(c => c.UserName == condidate.UserName && c.occupationId == condidate.occupationId && c.managerId == condidate.managerId);
            if (c == null)
            {
                Condidate ca = db.Condidates.FirstOrDefault(c => c.Id == condidate.Id && c.occupationId == condidate.occupationId && c.userId == condidate.userId && c.managerId == condidate.managerId);
                if (ca is not null)
                {
                    ca.UserName = condidate.UserName;
                    ca.Password = condidate.Password;
                    ca.Note = condidate.Note;
                    ca.Start = "generated";
                    db.Condidates.Update(ca);
                    db.SaveChanges();
                    return Ok("ok");
                }
                return NotFound("Candidate not found");
            }
            return NotFound("User name is existing!");
        }
        [HttpPost]
        public IActionResult Login([FromBody] Condidate condidate)
        {
            if (condidate.UserName == null) return NotFound("User name  can't be null!");
            if (condidate.Password == null) return NotFound(" Password can't be null!");
            if (condidate.occupationId == null) return NotFound("occupationId can't be null!");
            if (condidate.managerId == null) return NotFound("managerId can't be null!");
            if (condidate.userId == null) return NotFound("userId can't be null!");
            Condidate c = db.Condidates.FirstOrDefault(c => c.userId == condidate.userId && c.managerId == condidate.managerId && c.occupationId == condidate.occupationId && c.UserName == condidate.UserName && c.Password == condidate.Password);
            if (c == null) return NotFound("User name or Password was wrong!");
            DateTime currentDate = DateTime.Now;
            c.Start = "starting";
            c.UpdatedAt = currentDate;
            db.Condidates.Update(c);
            db.SaveChanges();
            return Ok(true);
        }
        [HttpPut]
        [Route("{userId}/{manaId}/{idReTest}")]
        public IActionResult UpdateRetest(string userId, string manaId, string idReTest)
        {
            if (manaId is null) return NotFound("ManagerID is empty!");
            if (userId is null) return NotFound("CandidateId is empty!");
            if (idReTest is null) return NotFound("idReTest is empty!");
            User user = db.Users.SingleOrDefault(u => u.Id == manaId);
            if (user != null)
            {
                Roles roles = db.Roles.SingleOrDefault(r => r.Id == user.RoleId);

                if (roles != null && roles.Name == "admin" && roles.Permissions.Contains("update"))
                {
                    Condidate candidate = db.Condidates.SingleOrDefault(c => c.userId == userId && c.managerId == user.Id);
                    if (candidate != null)
                    {
                        candidate.ReTest = idReTest;
                        db.Condidates.Update(candidate);
                        db.SaveChanges();
                        return Ok("ok");
                    }
                }
            }

            return NotFound("Authorization");
        }
    }
}

