namespace RssReader.Controllers0
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class FeedController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Greetings from .NET Core Web API!";
        }
    }
}
