using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Conway_Nativity_Directory
{
    public class IssueReport
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }

        public void Post()
        {
            JsonIssueReport jsonIssueReport = JsonIssueReport.Create(this);

            HttpWebRequest req = HttpWebRequest.CreateHttp("https://www.cnd-app.conwaynativities.com/issues/create");
            req.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(15).TotalMilliseconds);
            req.Headers.Add("appid", "cnd");
            req.Method = "POST";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(JsonSerializer.Serialize(jsonIssueReport));
            req.ContentType = "application/json; charset=utf-8";
            req.ContentLength = bytes.Length;
            Stream newStream = req.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();
        }
    }

    public class JsonIssueReport
    {
        public string title { get; set; }
        public string description { get; set; }
        public string exceptionType { get; set; }
        public string exceptionMessage { get; set; }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        public static JsonIssueReport Create(IssueReport issueReport)
        {
            return new JsonIssueReport()
            {
                title = issueReport.Title,
                description = issueReport.Description,
                exceptionType = issueReport.ExceptionType,
                exceptionMessage = issueReport.ExceptionMessage,
            };
        }
    }
}
