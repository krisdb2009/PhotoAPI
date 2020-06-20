using System.Web;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

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
                foreach(KeyValuePair<string, string> esc in replace)
                {
                    userName = userName.Replace(esc.Key, esc.Value);
                }
                DirectorySearcher ds = new DirectorySearcher("(&(objectClass=user)(sAMAccountName=" + userName + "))", new string[] { "thumbnailPhoto" });
                SearchResult result = ds.FindOne();
                if (result != null && result.Properties.Contains("thumbnailphoto"))
                {
                    http.Response.ContentType = "image/jpeg";
                    http.Response.BinaryWrite((byte[])result.Properties["thumbnailphoto"][0]);
                    return;
                }
            }
            http.Response.ContentType = "text/plain";
            http.Response.Write("Please specify a valid User Name, E.G: ./?username");
        }
        private Dictionary<string, string> replace = new Dictionary<string, string>() {
            { "*", @"\2a" },
            { "(", @"\28" },
            { ")", @"\29" },
            { @"\", @"\5c" },
            { "NUL", @"\00" },
            { "/", @"\2f" }
        };
    }
}