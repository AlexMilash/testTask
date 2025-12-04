using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestTask.HackerRankClient.Contracts;

public class BackgroundCacheRefresher : BackgroundService
{
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    private readonly ILogger<BackgroundCacheRefresher> _logger;
    private readonly IHackerRankPostingsService _postingsService;
    // lets say we're checking postings on a hackerrank every minute and refreshing our cache
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public BackgroundCacheRefresher(ILogger<BackgroundCacheRefresher> logger, IHackerRankPostingsService postingsService, IConfiguration configuration)
    {
        _postingsService = postingsService;
        _logger = logger;
        _interval = int.TryParse(configuration["cacheUpdateFrequencyMinutes"], out var intervalMinutes) ? TimeSpan.FromMinutes(intervalMinutes) : _interval;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background job started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _lock.WaitAsync();
                // we want to make sure only one instance of the job is running at a time
                // there might be better ways (e.g. tracking the progress instead of lock)
                // for the sake of simplicity let's go with lock
                await DoWorkAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the background job.");
            }
            finally
            {
                _lock.Release();
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Background job stopping at: {time}", DateTimeOffset.Now);
    }

    private async Task DoWorkAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background job running at: {time}", DateTimeOffset.Now);
        await _postingsService.RefreshPostings();
    }
}
