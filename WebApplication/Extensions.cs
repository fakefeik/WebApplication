using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebApplication.Models;

namespace WebApplication
{
    public static class Extensions
    {
        public static bool CheckCaptcha(this Controller c)
        {
            var response = c.Request["g-recaptcha-response"];
            const string secret = "6LfW4hETAAAAAJuj9x_N6C_gGWwbk3cvpmaeBTzC";
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                        secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //when response is false check for the error message
            if (!captchaResponse.Success)
            {
                //if (captchaResponse.ErrorCodes.Count <= 0)
                //    return RedirectToAction("Thread", new { threadId = threadId });

                //var error = captchaResponse.ErrorCodes[0].ToLower();
                return false;
            }
            return true;
        }

        public static string ToHtmlString(this string s)
        {
            return MvcHtmlString.Create(s).ToString();
        }
    }
}