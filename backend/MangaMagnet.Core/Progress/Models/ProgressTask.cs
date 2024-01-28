using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MangaMagnet.Core.Progress.Models;

public sealed class ProgressTask : INotifyPropertyChanged, IDisposable, IProgress<int>
{
	private string? _name = "New task";
	private int _progress;
	private bool _isCompleted;

	/// <inheritdoc />
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// Gets or sets the service that this task belongs to.
	/// </summary>
	internal ProgressService? Service { get; set; }

	/// <summary>
	/// Gets the unique identifier of the task.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the task.
	/// </summary>
	public string Name
	{
		get => _name;
		set => SetField(ref _name, value);
	}

	/// <summary>
	/// Gets or sets the progress of the task.
	/// </summary>
	public int Progress
	{
		get => _progress;
		set => SetField(ref _progress, value);
	}

	/// <summary>
	/// <c>true</c> if the task is completed, <c>false</c> otherwise.
	/// </summary>
	public bool IsCompleted
	{
		get => _isCompleted;
		set => SetField(ref _isCompleted, value);
	}

	/// <summary>
	/// Sets the value of a field and raises the <see cref="PropertyChanged"/> event if the value changed.
	/// </summary>
	/// <param name="field">Field to set</param>
	/// <param name="value">Value to set the field to</param>
	/// <param name="propertyName">Name of the property that was changed</param>
	/// <typeparam name="T">Type of the field</typeparam>
	/// <returns>True if the value changed, false otherwise</returns>
	private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;

		if (PropertyChanged is not null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		return true;
	}

	/// <inheritdoc />
	public void Dispose()
	{
		Service?.RemoveTask(this);
	}

	void IProgress<int>.Report(int value) => Progress = value;
}
