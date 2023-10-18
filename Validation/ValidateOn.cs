using OnlineAptitudeTest.Model;
using System.Text.RegularExpressions;
namespace OnlineAptitudeTest.Validation
{
    public class ValidateOn
    {
        readonly private AptitudeTestDbText db;
        public ValidateOn(AptitudeTestDbText db)
        {
            this.db = db;
        }
        public ValidateOn() { }
        public static bool ForStringLength(string val, int start, int end)
        {
            if (val.Length > start && val.Length < end)
            {
                return true;
            }
            return false;
        }
        public static bool ForEmail(string email)
        {
            // Define a regular expression pattern for a basic email validation
            string pattern = @"^[A-Za-z0-9+_.-]+@(.+)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
        public bool rule(string manaId, string rule)
        {
            if (manaId is null) return false;
            User user = db.Users.SingleOrDefault(u => u.Id == manaId);
            if (user != null)
            {
                Roles roles = db.Roles.SingleOrDefault(r => r.Id == user.RoleId);
                if (roles != null && roles.Name == "admin" && roles.Permissions.Contains(rule))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
