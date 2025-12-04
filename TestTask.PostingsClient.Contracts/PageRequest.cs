namespace TestTask.PostingsClient.Contracts
{
    // in some cases it makes sense to track an exact item we want our next page to start from
    // I wouldn't overcomplicate it for now though
    public class PageRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
