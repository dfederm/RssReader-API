namespace RssReader.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using RssReader.Models;
    using RssReader.Extensions;

    public sealed class InMemoryFeedStore : IFeedStore
    {
        private readonly ConcurrentDictionary<string, Feed> feeds = new ConcurrentDictionary<string, Feed>(StringComparer.OrdinalIgnoreCase);

        public Task<string> CreateAsync(Feed feed)
        {
            var id = Guid.NewGuid().ToShortString();
            if (this.feeds.TryAdd(id, feed))
            {
                return Task.FromResult(id);
            }
            else
            {
                return Task.FromResult<string>(null);
            }
        }

        public Task<Feed> GetAsync(string id)
        {
            Feed feed;
            if (this.feeds.TryGetValue(id, out feed))
            {
                return Task.FromResult(feed);
            }
            else
            {
                return Task.FromResult<Feed>(null);
            }
        }
    }
}
