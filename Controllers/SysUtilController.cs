using Microsoft.AspNetCore.Mvc;

namespace OnlineAptitudeTest.Controllers
{
    public class SysUtilController : Controller
    {
        /// <summary>
        /// Convert a string from one charset to another charset
        /// </summary>
        /// <param name="strText">source string</param>
        /// <param name="strSrcEncoding">original encoding name</param>
        /// <param name="strDestEncoding">dest encoding name</param>
        /// <returns></returns>
        public static String StringEncodingConvert(String strText, String strSrcEncoding, String strDestEncoding)
        {
            System.Text.Encoding srcEnc = System.Text.Encoding.GetEncoding(strSrcEncoding);
            System.Text.Encoding destEnc = System.Text.Encoding.GetEncoding(strDestEncoding);
            byte[] bData = srcEnc.GetBytes(strText);
            byte[] bResult = System.Text.Encoding.Convert(srcEnc, destEnc, bData);
            return destEnc.GetString(bResult);
        }

    }
}
