namespace TestTask.PostingsClient.Contracts
{
    public interface IPostingsService
    {
        Task<PageResponse<Posting>> GetPostings(PageRequest pageRequest);
    }
}
