using System.Text.Json.Serialization;

namespace TestTask.PostingsClient.Contracts
{
    public class Posting
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Uri { get; set; } = string.Empty;
        [JsonPropertyName("by")]
        public string PostedBy{ get; set; } = string.Empty;
        [JsonConverter(typeof(UnixTimeSecondsDateTimeConverter))]
        public DateTime Time { get; set; }
        [JsonPropertyName("score")]
        public int CommentCount { get; set; }
    }
}
