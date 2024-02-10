using System.Collections.Concurrent;
using System.ComponentModel;
using MangaMagnet.Core.Progress.Models;

namespace MangaMagnet.Core.Progress;

public class ProgressService
{
	private int _idCounter;
	private readonly ConcurrentDictionary<int, ProgressTask> _tasks = new();
	private readonly HashSet<ProgressTask> _updatedTasks = new();

	/// <summary>
	/// Creates a new task and adds it to the service.
	/// </summary>
	/// <param name="name">The name of the task</param>
	/// <returns>The created task</returns>
	public ProgressTask CreateTask(string name)
	{
		var task = new ProgressTask
		{
			Name = name,
		};
		AddTask(task);
		return task;
	}

	/// <summary>
	/// Adds a task to the service.
	/// </summary>
	/// <param name="task">The task to add</param>
	public void AddTask(ProgressTask task)
	{
		task.Id = Interlocked.Increment(ref _idCounter);
		task.Service = this;
		task.PropertyChanged += TaskOnPropertyChanged;
		_tasks.TryAdd(task.Id, task);
	}

	/// <summary>
	/// Removes a task from the service.
	/// </summary>
	/// <param name="task">The task to remove</param>
	public void RemoveTask(ProgressTask task)
	{
		if (!_tasks.TryRemove(task.Id, out _))
		{
			return;
		}

		task.PropertyChanged -= TaskOnPropertyChanged;
		task.IsCompleted = true;

		lock (_updatedTasks)
		{
			_updatedTasks.Add(task);
		}
	}

	/// <summary>
	/// Gets all the updated tasks and clears the list.
	/// </summary>
	/// <returns>A list of updated tasks</returns>
	public IReadOnlyList<ProgressTask> GetUpdatedTasksAndReset()
	{
		// ReSharper disable once InconsistentlySynchronizedField
		// This is fine because we're only reading the field and both are thread safe (integer).
		if (_updatedTasks.Count == 0)
		{
			return Array.Empty<ProgressTask>();
		}

		lock (_updatedTasks)
		{
			var tasks = _updatedTasks.ToList();
			_updatedTasks.Clear();
			return tasks;
		}
	}

	/// <summary>
	/// Gets all the tasks.
	/// </summary>
	/// <returns>A list of all the tasks</returns>
	public IReadOnlyList<ProgressTask> GetAllTasks()
	{
		if (_tasks.IsEmpty)
		{
			return Array.Empty<ProgressTask>();
		}

		// ConcurrentDictionary.Values implemented IReadOnlyCollection<T
		return (IReadOnlyList<ProgressTask>)_tasks.Values;
	}

	private void TaskOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (sender is not ProgressTask task) return;

		lock (_updatedTasks)
		{
			_updatedTasks.Add(task);
		}
	}
}
