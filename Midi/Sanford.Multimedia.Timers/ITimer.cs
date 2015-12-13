using System;
using System.ComponentModel;

namespace Sanford.Multimedia.Timers
{
    public interface ITimer : IComponent
    {
        /// <summary>
        /// Gets a value indicating whether the Timer is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets the timer mode.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        TimerMode Mode { get; set; }

        /// <summary>
        /// Period between timer events in milliseconds.
        /// </summary>
        int Period { get; set; }

        /// <summary>
        /// Resolution of the timer in milliseconds.
        /// </summary>
        int Resolution { get; set; }

        /// <summary>
        /// Gets or sets the object used to marshal event-handler calls.
        /// </summary>
        ISynchronizeInvoke SynchronizingObject { get; set; }

        /// <summary>
        /// Occurs when the Timer has started;
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Occurs when the Timer has stopped;
        /// </summary>
        event EventHandler Stopped;

        /// <summary>
        /// Occurs when the time period has elapsed.
        /// </summary>
        event EventHandler Tick;

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The timer has already been disposed.
        /// </exception>
        /// <exception cref="TimerStartException">
        /// The timer failed to start.
        /// </exception>
        void Start();

        /// <summary>
        /// Stops timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        void Stop();
    }
}