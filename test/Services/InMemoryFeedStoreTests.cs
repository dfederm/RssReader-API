namespace RssReader.Tests.Services
{
    using System.Threading.Tasks;
    using RssReader.Models;
    using RssReader.Services;
    using Xunit;

    public sealed class InMemoryFeedStoreTests
    {
        public sealed class CreateAsync
        {
            [Fact]
            public async Task Success()
            {
                var feedStore = new InMemoryFeedStore();
                for (var i = 0; i < 1000; i++)
                {
                    var id = await feedStore.CreateAsync(new Feed());
                    Assert.NotNull(id);
                }
            }
        }

        public sealed class GetAsync
        {
            [Fact]
            public async Task Success()
            {
                var feedStore = new InMemoryFeedStore();
                var expectedFeed = new Feed();
                var id = await feedStore.CreateAsync(expectedFeed);
                var actualFeed = await feedStore.GetAsync(id);
                Assert.Equal(expectedFeed, actualFeed);
            }

            [Fact]
            public async Task Empty()
            {
                var feedStore = new InMemoryFeedStore();
                var feed = await feedStore.GetAsync("someId");
                Assert.Null(feed);
            }

            [Fact]
            public async Task Missing()
            {
                var feedStore = new InMemoryFeedStore();
                for (var i = 0; i < 1000; i++)
                {
                    await feedStore.CreateAsync(new Feed());
                }

                var feed = await feedStore.GetAsync("someNonexistantId");
                Assert.Null(feed);
            }
        }
    }
}
