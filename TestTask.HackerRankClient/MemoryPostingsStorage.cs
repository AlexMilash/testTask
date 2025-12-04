using Microsoft.Extensions.Caching.Memory;
using TestTask.HackerRankClient.Contracts;
using TestTask.PostingsClient.Contracts;

namespace TestTask.HackerRankClient
{
    //I'd prefer to go with redis
    // but I don't want to spend time on it
    // so lets imagine we have redis here (or 2 layers caching) and it's horizontally scalable
    public class MemoryPostingsStorage : IPostingsCache
    {
        private const string PostingsKey = "AllPostings";
        // TODO: DI?
        private IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public bool ContainsPostingId(int postingId)
        {
            return _cache.TryGetValue(postingId, out var posting);
        }

        public void StorePostings(IEnumerable<Posting> postings, DateTimeOffset expiresAt)
        {
            _cache.Set(PostingsKey, postings, expiresAt);
            foreach(var posting in postings)
            {
                _cache.Set(posting.Id, posting, expiresAt);
            }
        }

        public IEnumerable<Posting> GetPostings() => _cache.Get<IEnumerable<Posting>>(PostingsKey) ?? [];
    }
}
