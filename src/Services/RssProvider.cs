namespace RssReader.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using RssReader.Models.Rss;

    public sealed class RssProvider : IRssProvider
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Rss));

        private readonly HttpClient httpClient;

        public RssProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Rss> FetchAsync(Uri uri)
        {
            var response = await this.httpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync();
            var rss = (Rss)serializer.Deserialize(stream);

            // TODO: Convert to more easy to use data structure
            return rss;
        }
    }
}
