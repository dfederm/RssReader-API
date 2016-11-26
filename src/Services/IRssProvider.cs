namespace RssReader.Services
{
    using System;
    using System.Threading.Tasks;
    using RssReader.Models.Rss;

    public interface IRssProvider
    {
        Task<Rss> FetchAsync(Uri uri);
    }
}
