using System.Buffers;

namespace MangaMagnet.Core.Util;

/// <summary>
/// Pooled version of <see cref="ArrayBufferWriter{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the elements in the array.</typeparam>
public sealed class PooledArrayBufferWriter<T> : IBufferWriter<T>, IDisposable
{
	private const int InitialBufferSize = 1024;

	private readonly ArrayPool<T> _pool;
	private T[] _buffer;
	private int _index;

	/// <summary>
	/// Initializes a new instance of the <see cref="PooledArrayBufferWriter{T}"/> class.
	/// </summary>
	/// <param name="pool">The pool to use. Defaults to <see cref="ArrayPool{T}.Shared"/>.</param>
	public PooledArrayBufferWriter(ArrayPool<T>? pool = null)
	{
		_pool = pool ?? ArrayPool<T>.Shared;
		_buffer = _pool.Rent(InitialBufferSize);
	}

	/// <summary>
	/// Gets the written memory.
	/// </summary>
	public ReadOnlyMemory<T> WrittenMemory => _buffer.AsMemory(0, _index);

	/// <summary>
	/// Resets the writer to the initial state.
	/// </summary>
	public void Reset()
	{
		_index = 0;

		if (_buffer.Length > InitialBufferSize)
		{
			_pool.Return(_buffer);
			_buffer = _pool.Rent(InitialBufferSize);
		}
	}

	/// <inheritdoc />
	public void Advance(int count)
	{
		_index += count;
	}

	/// <inheritdoc />
	public Memory<T> GetMemory(int sizeHint = 0)
	{
		if (_buffer.Length - _index < sizeHint)
		{
			Grow(sizeHint);
		}

		return _buffer.AsMemory(_index);
	}

	/// <inheritdoc />
	public Span<T> GetSpan(int sizeHint = 0)
	{
		return GetMemory(sizeHint).Span;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		_pool.Return(_buffer);
	}

	private void Grow(int sizeHint)
	{
		var newSize = Math.Max(_buffer.Length * 2, _buffer.Length + sizeHint);
		var newBuffer = _pool.Rent(newSize);

		_buffer.AsSpan(0, _index).CopyTo(newBuffer);

		_pool.Return(_buffer);
		_buffer = newBuffer;
	}
}

