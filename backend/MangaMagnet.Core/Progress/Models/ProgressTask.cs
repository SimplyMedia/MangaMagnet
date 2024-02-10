using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MangaMagnet.Core.Progress.Models;

public sealed class ProgressTask : INotifyPropertyChanged, IDisposable, IProgress<int>
{
    private string _name = "New task";
    private int _progress;
    private bool _isCompleted;
    private int? _total;
    private int _current;
    private bool _indeterminate;

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
    /// <c>true</c> if the task is indeterminate, <c>false</c> otherwise.
    /// </summary>
    public bool Indeterminate
    {
        get => _indeterminate;
        set => SetField(ref _indeterminate, value);
    }

    public int? Total
    {
        get => _total;
        set
        {
            if (SetField(ref _total, value))
            {
                UpdateProgress();
            }
        }
    }

    public int Current
    {
        get => _current;
        set
        {
            if (SetField(ref _current, value))
            {
                UpdateProgress();
            }
        }
    }

    /// <summary>
    /// Sets the progress of the task.
    /// </summary>
    /// <param name="current">Current progress</param>
    /// <param name="total">Total progress</param>
    public void SetProgress(int current, int total)
    {
        Current = current;
        Total = total;
        UpdateProgress();
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        return true;
    }

    /// <summary>
    /// Thread-safe increment of the current progress.
    /// </summary>
    public void Increment()
    {
        Interlocked.Increment(ref _current);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Current)));
        UpdateProgress();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Service?.RemoveTask(this);
    }

    /// <summary>
    /// Updates the progress of the task based on the current and total progress.
    /// </summary>
    private void UpdateProgress()
    {
        if (_total.HasValue)
        {
            Progress = (int) Math.Floor((double) _current / _total.Value * 100);
        }
    }

    void IProgress<int>.Report(int value) => Progress = value;
}
