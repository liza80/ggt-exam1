using System.Collections.Generic;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

public class GitHubController : ApiController
{
    [HttpPost]
    [Route("api/github/bookmark")]
    public IHttpActionResult BookmarkRepository([FromBody] Repository repository)
    {
        var sessionBookmarks = HttpContext.Current.Session["Bookmarks"] as List<Repository>;
        if (sessionBookmarks == null)
        {
            sessionBookmarks = new List<Repository>();
        }
        sessionBookmarks.Add(repository);
        HttpContext.Current.Session["Bookmarks"] = sessionBookmarks;
        return Ok();
    }

    [HttpGet]
    [Route("api/github/bookmarks")]
    public JsonResult<List<Repository>> GetBookmarks()
    {
        var sessionBookmarks = HttpContext.Current.Session["Bookmarks"] as List<Repository>;
        if (sessionBookmarks == null)
        {
            sessionBookmarks = new List<Repository>();
        }
        return Json(sessionBookmarks);
    }

    [HttpPost]
    [Route("api/github/sendemail")]
    public IHttpActionResult SendEmail([FromBody] EmailRequest request)
    {
        try
        {
            var message = new MailMessage();
            message.To.Add(request.EmailAddress);
            message.Subject = "GitHub Repository Details";
            message.Body = $"Repository Name: {request.Repository.name}\nURL: {request.Repository.html_url}";
            var smtpClient = new SmtpClient();
            smtpClient.Send(message);
            return Ok();
        }
        catch (SmtpException ex)
        {
            return InternalServerError(ex);
        }
    }
}

public class Repository
{
    public string name { get; set; }
    public string html_url { get; set; }
    public Owner owner { get; set; }
}

public class Owner
{
    public string avatar_url { get; set; }
}

public class EmailRequest
{
    public string EmailAddress { get; set; }
    public Repository Repository { get; set; }
}
