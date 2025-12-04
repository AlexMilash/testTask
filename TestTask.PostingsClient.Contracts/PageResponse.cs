namespace TestTask.PostingsClient.Contracts
{
    public class PageResponse<T>
    {
        public IEnumerable<T> Postings { get; set; } = Array.Empty<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasMore { get; set; }
    }
}
