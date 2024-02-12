using System.Net;
using MangaMagnet.Core.Metadata;
using MangaMagnet.Core.Metadata.MangaDex;
using MangaMagnet.Core.Providers.MangaDex;
using Microsoft.Extensions.DependencyInjection;

namespace MangaMagnet.Core.Services;

public static class IServiceCollectionExtension
{
	public static void AddMangaDexServices(this IServiceCollection services)
	{
		services.AddHttpClient("MangaDex")
			.ConfigurePrimaryHttpMessageHandler(() =>
			{
				var handler = new HttpClientHandler();

				if (handler.SupportsAutomaticDecompression)
				{
					handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli;
				}

				return handler;
			});

		services.AddSingleton<MangaDexApiService>();
		services.AddSingleton<MangaDexDownloadService>();
		services.AddSingleton<MangaDexConverterService>();
		services.AddSingleton<IMetadataFetcher, MangaDexMetadataFetcher>();
	}
}
