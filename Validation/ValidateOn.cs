using System.Text.RegularExpressions;

namespace OnlineAptitudeTest.Validation
{
    public class ValidateOn
    {
        public ValidateOn() { }
        public static bool  ForStringLength(string val,int start,int end)
        {
            if(val.Length > start  && val.Length < end)
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
    }
}
