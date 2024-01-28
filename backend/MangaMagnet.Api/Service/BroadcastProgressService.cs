using System.Buffers;
using System.Text.Json;
using MangaMagnet.Core.Progress;
using MangaMagnet.Core.Util;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace MangaMagnet.Api.Service;

public class BroadcastProgressService(ProgressService progressService, WebSocketService webSocketService, IOptions<JsonOptions> jsonOptions) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
			var buffer = new PooledArrayBufferWriter<byte>();

			while (await timer.WaitForNextTickAsync(stoppingToken))
			{
				var changedTasks = progressService.GetUpdatedTasksAndReset();

				if (changedTasks.Count == 0)
				{
					continue;
				}

				var writer = new Utf8JsonWriter(buffer);
				JsonSerializer.Serialize(writer, changedTasks, jsonOptions.Value.SerializerOptions);
				await writer.FlushAsync(stoppingToken);

				await webSocketService.SendToAllAsync(buffer.WrittenMemory, stoppingToken);

				buffer.Reset();
			}
		}
		catch (TaskCanceledException)
		{
			// Ignore
		}
	}
}
