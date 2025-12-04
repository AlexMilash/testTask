using TestTask.PostingsClient.Contracts;

namespace TestTask.HackerRankClient.Contracts
{
    public interface IPostingsCache
    {
        bool ContainsPostingId(int postingId);
        void StorePostings(IEnumerable<Posting> postings, DateTimeOffset expiresAt);
        IEnumerable<Posting> GetPostings();
    }
}
