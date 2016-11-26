namespace RssReader.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using RssReader.Controllers;
    using RssReader.Models;
    using RssReader.Models.Rss;
    using RssReader.Services;
    using Xunit;

    public sealed class FeedControllerTests
    {
        public sealed class Create
        {
            [Fact]
            public async Task Success()
            {
                var feed = new Feed { Uris = new Uri[] { new Uri("http://someUrl") } };
                var feedStore = new Mock<IFeedStore>(MockBehavior.Strict);
                feedStore.Setup(_ => _.CreateAsync(feed)).ReturnsAsync("someId");
                var rssProvider = new Mock<IRssProvider>(MockBehavior.Strict);
                var controller = new FeedController(feedStore.Object, rssProvider.Object);

                var result = await controller.Create(feed);

                Assert.NotNull(result);
                Assert.IsType(typeof(CreatedAtActionResult), result);

                var createdAtActionResult = (CreatedAtActionResult)result;
                Assert.Equal((int)HttpStatusCode.Created, createdAtActionResult.StatusCode);
                Assert.Equal(nameof(FeedController.Get), createdAtActionResult.ActionName);
                Assert.Equal("someId", createdAtActionResult.RouteValues["id"]);
                Assert.Equal("someId", createdAtActionResult.Value);

                feedStore.VerifyAll();
                rssProvider.VerifyAll();
            }

            [Fact]
            public async Task NoUrisInFeed()
            {
                var feed = new Feed();
                var feedStore = new Mock<IFeedStore>(MockBehavior.Strict);
                var rssProvider = new Mock<IRssProvider>(MockBehavior.Strict);
                var controller = new FeedController(feedStore.Object, rssProvider.Object);

                var result = await controller.Create(feed);

                Assert.NotNull(result);
                Assert.IsType(typeof(BadRequestResult), result);

                feedStore.VerifyAll();
                rssProvider.VerifyAll();
            }

            [Fact]
            public async Task FeedStoreCreationFailure()
            {
                var feed = new Feed { Uris = new Uri[] { new Uri("http://someUrl") } };
                var feedStore = new Mock<IFeedStore>(MockBehavior.Strict);
                feedStore.Setup(_ => _.CreateAsync(feed)).ReturnsAsync(null);
                var rssProvider = new Mock<IRssProvider>(MockBehavior.Strict);
                var controller = new FeedController(feedStore.Object, rssProvider.Object);

                var result = await controller.Create(feed);

                Assert.NotNull(result);
                Assert.IsType(typeof(BadRequestResult), result);

                feedStore.VerifyAll();
                rssProvider.VerifyAll();
            }
        }

        public sealed class Get
        {
            [Theory]
            [InlineData("http://someUri1")]
            [InlineData("http://someUri1", "http://someUri2")]
            [InlineData("http://someUri1", "http://someUri2", "http://someUri3")]
            public async Task Success(params string[] urls)
            {
                var feed = new Feed { Uris = urls.Select(_ => new Uri(_)).ToArray() };
                var feedStore = new Mock<IFeedStore>(MockBehavior.Strict);
                feedStore.Setup(_ => _.GetAsync("someId")).ReturnsAsync(feed);

                var rssProvider = new Mock<IRssProvider>(MockBehavior.Strict);
                var expectedRss = new List<Rss>(urls.Length);
                foreach (var url in urls)
                {
                    var rss = new Rss();
                    expectedRss.Add(rss);
                    rssProvider.Setup(_ => _.FetchAsync(new Uri(url))).ReturnsAsync(rss);
                }

                var controller = new FeedController(feedStore.Object, rssProvider.Object);

                var result = await controller.Get("someId");
                
                Assert.NotNull(result);
                Assert.IsType(typeof(OkObjectResult), result);
                var okObjectResult = (OkObjectResult)result;
                Assert.IsAssignableFrom(typeof(IEnumerable<Rss>), okObjectResult.Value);
                var actualRss = ((IEnumerable<Rss>)okObjectResult.Value).ToArray();
                Assert.Equal(expectedRss, actualRss);

                feedStore.VerifyAll();
                rssProvider.VerifyAll();
            }

            [Fact]
            public async Task FeedNotFound()
            {
                var feedStore = new Mock<IFeedStore>(MockBehavior.Strict);
                feedStore.Setup(_ => _.GetAsync("someId")).ReturnsAsync(null);

                var rssProvider = new Mock<IRssProvider>(MockBehavior.Strict);

                var controller = new FeedController(feedStore.Object, rssProvider.Object);

                var result = await controller.Get("someId");
                
                Assert.NotNull(result);
                Assert.IsType(typeof(NotFoundResult), result);

                feedStore.VerifyAll();
                rssProvider.VerifyAll();
            }
        }
    }
}
