namespace RssReader.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using RssReader.Services;
    using Xunit;

    public sealed class RssProviderTests
    {
        public sealed class FetchAsync
        {
            [Theory]
            [InlineData("MmoChampion.2015.11.25.xml")]
            public async Task Success(string file)
            {
                var uri = new Uri("http://someUrl");
                var handler = new MockHttpMessageHandler();
                handler.AddResponse(uri, file);
                var httpClient = new HttpClient(handler);

                var rssProvider = new RssProvider(httpClient);
                var rss = await rssProvider.FetchAsync(uri);

                Assert.NotNull(rss);
            }

            [Fact]
            public async Task NotFound()
            {
                var uri = new Uri("http://someUrl");
                var handler = new MockHttpMessageHandler();
                var httpClient = new HttpClient(handler);

                var rssProvider = new RssProvider(httpClient);
                var rss = await rssProvider.FetchAsync(uri);

                Assert.Null(rss);
            }
        }

        private sealed class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly Dictionary<Uri, string> responses = new Dictionary<Uri, string>();

            public void AddResponse(Uri uri, string file)
            {
                this.responses.Add(uri, file);
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage();

                string file;
                if (this.responses.TryGetValue(request.RequestUri, out file))
                {
                    var stream = new FileStream(TestData.Get(file), FileMode.Open, FileAccess.Read);
                    response.Content = new StreamContent(stream);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }

                return Task.FromResult(response);
            }
        }
    }
}
