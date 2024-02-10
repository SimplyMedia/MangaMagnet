using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace MangaMagnet.Api.Service;

public class WebSocketService
{
	private readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new();

	/// <summary>
	/// Add a new socket to the list.
	/// </summary>
	/// <param name="id">Unique identifier of the socket.</param>
	/// <param name="socket">Socket to add.</param>
	public void AddSocket(Guid id, WebSocket socket)
	{
		_sockets.TryAdd(id, socket);
	}

	/// <summary>
	/// Remove a socket from the list.
	/// </summary>
	/// <param name="id">Unique identifier of the socket.</param>
	public void RemoveSocket(Guid id)
	{
		_sockets.TryRemove(id, out _);
	}

	/// <summary>
	/// Send message to all connected clients.
	/// </summary>
	/// <param name="memory">Message to send.</param>
	/// <param name="stoppingToken">Cancellation token.</param>
	public Task SendToAllAsync(ReadOnlyMemory<byte> memory, CancellationToken stoppingToken)
	{
		return Task.WhenAll(_sockets.Values.Select(socket => SendToSocketAsync(socket, memory, stoppingToken)));
	}

	private static async Task SendToSocketAsync(WebSocket socket, ReadOnlyMemory<byte> memory, CancellationToken stoppingToken)
	{
		try
		{
			await socket.SendAsync(memory, WebSocketMessageType.Text, true, stoppingToken);
		}
		catch
		{
			// ignore
		}
	}
}
