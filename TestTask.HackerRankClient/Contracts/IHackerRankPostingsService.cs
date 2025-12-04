using TestTask.PostingsClient.Contracts;

namespace TestTask.HackerRankClient.Contracts
{
    public interface IHackerRankPostingsService
    {
        Task RefreshPostings();
    }
}
