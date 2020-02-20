using System.Web;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace PhotoAPI
{
    public class _default : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext http)
        {
            if (http.Request.QueryString.Count == 1)
            {
                string userName = http.Request.QueryString[0];
                if (userName != null && Regex.IsMatch(userName, @"[\w\d]*[^/\\[\]:;|=,+*?<>]*"))
                {
                    DirectorySearcher ds = new DirectorySearcher("(&(objectClass=user)(sAMAccountName=" + userName + "))", new string[] { "thumbnailPhoto" });
                    SearchResult result = ds.FindOne();
                    if (result != null)
                    {
                        http.Response.ContentType = "image/jpeg";
                        http.Response.BinaryWrite((byte[])result.Properties["thumbnailPhoto"][0]);
                        return;
                    }
                }
            }
            http.Response.ContentType = "text/plain";
            http.Response.Write("Please specify a valid User Name, E.G: ./?username");
        }
    }
}