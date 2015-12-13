using System;
using System.ComponentModel;

namespace Sanford.Multimedia.Timers
{
    public interface ITimer : IComponent
    {
        bool IsRunning { get; }
        TimerMode Mode { get; set; }

        /// <summary>
        /// Period between timer events in milliseconds.
        /// </summary>
        int Period { get; set; }
        int Resolution { get; set; }
        ISynchronizeInvoke SynchronizingObject { get; set; }

        event EventHandler Started;
        event EventHandler Stopped;
        event EventHandler Tick;

        void Start();
        void Stop();
    }
}