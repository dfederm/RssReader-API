namespace RssReader.Controllers0
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    public class FeedController : Controller
    {
        private ILogger logger;

        public FeedController(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("Feed");
        }

        [HttpPost]
        public IActionResult Create([FromBody] string[] feeds)
        {
            return this.CreatedAtAction(
                nameof(this.Get),
                new { feeds = feeds},
                feeds);
        }

        [HttpGet]
        public string Get([FromQuery] string[] feeds)
        {
            return "Greetings from .NET Core Web API! " + feeds.Length + ", " + string.Join(", ", feeds);
        }
    }
}
