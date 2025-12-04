using Microsoft.Extensions.Configuration;
using TestTask.HackerRankClient.Contracts;
using TestTask.PostingsClient.Contracts;

namespace TestTask.HackerRankClient
{
    internal class HackerRankPostingsService : IHackerRankPostingsService, IPostingsService
    {
        private IPostingsCache _postingsCache;
        private IHackerRankPostingsClient _client;
        private int _cacheTtlMilliseconds = 1 * 60 * 1000; // the default lifetime of 1 minute
        private int _parallelRequetBatchSize = 10;
        public HackerRankPostingsService(IPostingsCache postingsCache, IHackerRankPostingsClient client, IConfiguration configuration)
        {
            _postingsCache = postingsCache;
            _client = client;
            _cacheTtlMilliseconds = int.TryParse(configuration["cacheLifetime"], out var lifetime) ? lifetime : _cacheTtlMilliseconds;
            _parallelRequetBatchSize = int.TryParse(configuration["paralelRequestBatchSize"], out var parallelRequetBatchSize) ? parallelRequetBatchSize : _parallelRequetBatchSize;
        }

        public async Task RefreshPostings()
        {
            var postingIds = await _client.GetPostings(new PageRequest { PageNumber = 1, PageSize = 100 });
            var postings = new List<Posting>();

            // I'm assuming those postings are not getting changed frequently, and we can cache them once

            // we could keep already cached postings for a longer time
            // this would help us reduce the number of requests to the hackerrank service
            // but that would require a proper strategy
            var currBatchSize = Math.Min(_parallelRequetBatchSize, postingIds.Count());
            var currBatchNumber = 0;

            var requestBatches = postingIds.Chunk(currBatchSize).ToList();
            var ttl = DateTimeOffset.Now.AddMilliseconds(_cacheTtlMilliseconds);
            for (var i = 0; i < requestBatches.Count; i++)
            {
                var postingRetrievalTasks = requestBatches[i].Select(_client.GetPosting);
                await Task.WhenAll(postingRetrievalTasks);
                var newPostings = postingRetrievalTasks.Select(t => t.Result).ToList();
                postings.AddRange(newPostings);
                List<Posting> currList = [.. _postingsCache.GetPostings(), .. newPostings];
                _postingsCache.StorePostings(currList, ttl);
            }
        }

        public async Task<PageResponse<Posting>> GetPostings(PageRequest pageRequest)
        {
            // it's pretty stupid to get all items and add postings only on a back end side
            // instead we should be getting PageSize items number from storage (cache) in this case
            // I'm aware it's suboptimal but leaving it as is for the sake of simplicity
            if (pageRequest == null)
                throw new ArgumentNullException(nameof(pageRequest));

            if (pageRequest.PageNumber < 0)
                throw new ArgumentException("Page number must be non-negative", nameof(pageRequest));

            var allPostings = _postingsCache.GetPostings();
            var skipNum = Math.Max((pageRequest.PageNumber - 1) * pageRequest.PageSize, 0);
            var currPageItems = allPostings.Skip(skipNum).Take(pageRequest.PageSize).ToList();

            return new PageResponse<Posting>
            {
                Postings = currPageItems,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
                HasMore = allPostings.Count() > skipNum + pageRequest.PageSize
            };
        }
    }
}
