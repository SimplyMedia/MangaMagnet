using System.Threading.RateLimiting;

namespace MangaMagnet.Core.Metadata.Providers;

public abstract class SimpleRatelimitedProvider
{
    protected readonly RateLimiter RateLimiter = new TokenBucketRateLimiter(
        new TokenBucketRateLimiterOptions
        {
            ReplenishmentPeriod = TimeSpan.FromSeconds(5),
            TokensPerPeriod = 10,
            TokenLimit = 10,
            AutoReplenishment = true,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
}
