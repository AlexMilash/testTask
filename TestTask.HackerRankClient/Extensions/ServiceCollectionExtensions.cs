using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            });

            collection.AddTransient<IHackerRankPostingsService, HackerRankPostingsService>();
            collection.AddTransient<IHackerRankPostingsClient, HackerRankPostingsClient>();
            collection.AddTransient<IPostingsService, HackerRankPostingsService>();
            collection.AddSingleton<IPostingsCache, MemoryPostingsStorage>();
            collection.AddHostedService<BackgroundCacheRefresher>();

            return collection;
        }
    }
}
