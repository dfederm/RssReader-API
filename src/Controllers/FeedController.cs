namespace RssReader.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using RssReader.Models;
    using RssReader.Services;

    [Route("api/[controller]")]
    public class FeedController : Controller
    {
        private readonly IFeedStore feedStore;

        private readonly IRssProvider rssProvider;

        public FeedController(
            IFeedStore feedStore,
            IRssProvider rssProvider)
        {
            this.feedStore = feedStore;
            this.rssProvider = rssProvider;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] Feed feed)
        {
            if (feed == null || feed.Uris == null || feed.Uris.Length == 0)
            {
                return this.BadRequest();
            }

            var id = await this.feedStore.CreateAsync(feed);
            if (id == null)
            {
                return this.BadRequest();
            }

            return this.CreatedAtAction(
                nameof(this.Get),
                new { id = id},
                id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var feed = await this.feedStore.GetAsync(id);
            if (feed == null)
            {
                return this.NotFound();
            }

            var tasks = feed.Uris.Select(uri => this.rssProvider.FetchAsync(uri));
            var rss = (await Task.WhenAll(tasks)).Where(result => result != null);

            return this.Ok(rss);
        }
    }
}
