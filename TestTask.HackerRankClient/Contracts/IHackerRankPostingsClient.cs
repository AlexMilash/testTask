using System.Threading.Tasks;
using TestTask.PostingsClient.Contracts;

namespace TestTask.HackerRankClient.Contracts
{
    internal interface IHackerRankPostingsClient
    {
        Task<int[]> GetPostings(PageRequest request);

        Task<Posting> GetPosting(int id);

    }
}
