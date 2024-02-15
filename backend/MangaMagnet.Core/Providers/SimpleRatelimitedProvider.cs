using System.Threading.RateLimiting;

namespace MangaMagnet.Core.Providers;

public abstract class SimpleRatelimitedProvider
{
    protected readonly RateLimiter RateLimiter = new FixedWindowRateLimiter(
        new FixedWindowRateLimiterOptions
        {
	        Window = TimeSpan.FromSeconds(2),
	        AutoReplenishment = true,
	        PermitLimit = 5,
	        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
	        QueueLimit = int.MaxValue
        });
}
