using System.Net.WebSockets;
using System.Text.Json;
using MangaMagnet.Api.Service;
using MangaMagnet.Core.Progress;
using MangaMagnet.Core.Util;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace MangaMagnet.Api.Middlewares;

/// <summary>
/// Handles WebSocket requests.
/// </summary>
public class WebSocketMiddleware(WebSocketService webSocketService, ProgressService progressService, IOptions<JsonOptions> jsonOptions) : IMiddleware
{
	/// <inheritdoc />
	public Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (context.Request.Path != "/api/ws")
		{
			return next(context);
		}

		if (context.WebSockets.IsWebSocketRequest)
		{
			return HandleWebSocketRequest(context);
		}

		context.Response.StatusCode = 400;
		return Task.CompletedTask;
	}

	private async Task SendCurrentTasksAsync(WebSocket webSocket)
	{
		var tasks = progressService.GetAllTasks();
		var buffer = new PooledArrayBufferWriter<byte>();
		var writer = new Utf8JsonWriter(buffer);
		JsonSerializer.Serialize(writer, tasks, jsonOptions.Value.SerializerOptions);
		await writer.FlushAsync();
		await webSocket.SendAsync(buffer.WrittenMemory, WebSocketMessageType.Text, true, CancellationToken.None);
	}

	private async Task HandleWebSocketRequest(HttpContext context)
	{
		var id = Guid.NewGuid();
		using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

		try
		{
			var cancellationToken = context.RequestAborted;

			await SendCurrentTasksAsync(webSocket);
			webSocketService.AddSocket(id, webSocket);

			var buffer = new byte[1024];
			var result = await webSocket.ReceiveAsync(buffer, default);

			while (!result.CloseStatus.HasValue)
			{
				result = await webSocket.ReceiveAsync(buffer, cancellationToken);

				// This websocket is only for sending progress updates, so we don't need to handle any incoming messages.
			}

			await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, cancellationToken);
		}
		catch (WebSocketException e)
		{
			await TryCloseAsync(webSocket, WebSocketCloseStatus.ProtocolError);
		}
		catch (Exception)
		{
			await TryCloseAsync(webSocket, WebSocketCloseStatus.InternalServerError);
		}
		finally
		{
			webSocketService.RemoveSocket(id);
		}
	}

	private async Task TryCloseAsync(WebSocket webSocket, WebSocketCloseStatus closeStatus, string closeStatusDescription = "")
	{
		if (webSocket.State is not (WebSocketState.Open or WebSocketState.CloseReceived or WebSocketState.CloseSent))
		{
			return;
		}

		try
		{
			await webSocket.CloseAsync(closeStatus, closeStatusDescription, CancellationToken.None);
		}
		catch
		{
			// Ignore
		}
	}
}
