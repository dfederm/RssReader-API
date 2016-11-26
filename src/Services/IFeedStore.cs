namespace RssReader.Services
{
    using System.Threading.Tasks;
    using RssReader.Models;

    public interface IFeedStore
    {
        Task<string> CreateAsync(Feed feed);

        Task<Feed> GetAsync(string id);
    }
}
