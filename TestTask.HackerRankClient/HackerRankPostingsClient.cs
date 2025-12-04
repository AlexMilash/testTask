using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;
using TestTask.HackerRankClient.Contracts;
using TestTask.PostingsClient.Contracts;
using TestTask.PostingsClient.Contracts.Exceptions;

namespace TestTask.HackerRankClient
{
    public class HackerRankPostingsClient : IHackerRankPostingsClient
    {
        private string _storiesUrl;
        private string _storyDetailsUrl;
        private HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public HackerRankPostingsClient(HttpClient httpClient, IConfiguration configuration)
        {

            var baseUrl = configuration["hackerRankConfig:base"] ?? throw new ArgumentException("no hackerRankConfig/base configuration available");
            httpClient.BaseAddress = new Uri(baseUrl);

            _httpClient = httpClient;
            _storiesUrl = configuration["hackerRankConfig:stories"] ?? throw new ArgumentException("no hackerRankConfig/stories configuration available");
            _storyDetailsUrl = configuration["hackerRankConfig:storyDetails"] ?? throw new ArgumentException("no hackerRankConfig/storyDetails configuration available");
        }

        public async Task<int[]> GetPostings(PageRequest request) => await GetHttpContentAsync<int[]>(_storiesUrl);

        public async Task<Posting> GetPosting(int id)
        {
            var storyDetailsUrl = string.Format(_storyDetailsUrl, id);
            return await GetHttpContentAsync<Posting>(storyDetailsUrl);
        }

        private async Task<T> GetHttpContentAsync<T>(string relativeUrl)
        {
            var storiesResponse = await _httpClient.GetAsync(relativeUrl);
            var storiesContent = await storiesResponse.Content.ReadAsStringAsync();

            if (!storiesResponse.IsSuccessStatusCode)
            {
                throw new FailedToRetrieveFromExternalSystemException(storiesContent, (int)HttpStatusCode.BadRequest);
            }

            var storyIds = JsonSerializer.Deserialize<T>(storiesContent, _options);
            return storyIds ?? throw new UnexpectedExternalDataFormatException((int)HttpStatusCode.BadRequest);
        }
    }
}
