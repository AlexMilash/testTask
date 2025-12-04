using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using TestTask.HackerRankClient.Contracts;
using TestTask.PostingsClient.Contracts;

namespace TestTask.HackerRankClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHackerRankServices(this IServiceCollection collection, IConfiguration configuration)
        {
            var baseUrl = configuration["hackerRankConfig:base"] ?? throw new ArgumentException("no hackerRankConfig/base configuration available");
            collection.AddHttpClient<HackerRankPostingsService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

            collection.AddTransient<IHackerRankPostingsService, HackerRankPostingsService>();
            collection.AddTransient<IHackerRankPostingsClient, HackerRankPostingsClient>();
            collection.AddTransient<IPostingsService, HackerRankPostingsService>();
            collection.AddSingleton<IPostingsCache, MemoryPostingsStorage>();
            collection.AddHostedService<BackgroundCacheRefresher>();

            return collection;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() // 5xx, 408, network errors
                .OrResult(msg => (int)msg.StatusCode == 429)  // Retry on TooManyRequests
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );
        }

    }
}
