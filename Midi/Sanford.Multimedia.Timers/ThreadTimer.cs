using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Sanford.Multimedia.Timers
{
    class TimerQueue
    {
        Stopwatch watch = Stopwatch.StartNew();
        Thread loop;

        public static TimerQueue Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimerQueue();
                }
                return instance;

            }
        }
        static TimerQueue instance;

        private TimerQueue()
        {
        }

        public void Add(ThreadTimer timer)
        {
            lock(this)
            {
                var tick = new Tick
                {
                    Timer = timer,
                    Time = watch.Elapsed
                };
                ticks.Add(tick);
                ticks.Sort();

                if (loop == null)
                {
                    loop = new Thread(TimerLoop);
                    loop.Start();
                }
                Monitor.PulseAll(this);
            }
        }

        public void Remove(ThreadTimer timer)
        {
            lock(this)
            {
                int i = 0;
                for (; i < ticks.Count; ++i)
                {
                    if (ticks[i].Timer == timer)
                    {
                        break;
                    }
                }
                if (i < ticks.Count)
                {
                    ticks.RemoveAt(i);
                }
                Monitor.PulseAll(this);
            }
        }

        class Tick : IComparable
        {
            public ThreadTimer Timer;
            public TimeSpan Time;

            public int CompareTo(object obj)
            {
                var r = obj as Tick;
                if (r == null)
                {
                    return -1;
                }
                return Time.CompareTo(r.Time);
            }
        }

        List<Tick> ticks = new List<Tick>();

        static TimeSpan Min(TimeSpan x0, TimeSpan x1)
        {
            if (x0 > x1)
            {
                return x1;
            }
            else
            {
                return x0;
            }
        }

        private void TimerLoop()
        {
            lock (this)
            {
                TimeSpan maxTimeout = TimeSpan.FromMilliseconds(500);

                for (int empty=0;empty<3;++empty)
                {
                    var waitTime = maxTimeout;
                    if (ticks.Count > 0)
                    {
                        waitTime = Min(ticks[0].Time - watch.Elapsed, waitTime);
                        empty = 0;
                    }

                    if (waitTime > TimeSpan.Zero)
                    {
                        Monitor.Wait(this, waitTime);
                    }

                    if (ticks.Count > 0)
                    {
                        var tick = ticks[0];
                        Monitor.Exit(this);
                        tick.Timer.DoTick();
                        Monitor.Enter(this);
                        tick.Time += tick.Timer.period;
                        ticks.Sort();
                    }
                }
                loop = null;
            }
        }
    }

    /// <summary>
    /// Replacement for the Windows multimedia timer that also runs on Mono
    /// </summary>
    public sealed class ThreadTimer : ITimer
    {
        public ThreadTimer()
            : this(TimerQueue.Instance)
        {
            if (!Stopwatch.IsHighResolution)
            {
                throw new NotImplementedException("Stopwatch is not IsHighResolution");
            }

            mode = TimerMode.Periodic;
            resolution = 1;
            period = TimeSpan.FromMilliseconds(resolution);

            tickRaiser = new EventRaiser(OnTick);
        }

        TimerQueue queue;
        bool isRunning = false;
        internal TimeSpan period;

        ThreadTimer(TimerQueue queue)
        {
            this.queue = queue;
        }

        static object[] emptyArgs = new object[] { EventArgs.Empty };

        internal void DoTick()
        {
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
            {
                SynchronizingObject.BeginInvoke(tickRaiser, emptyArgs);
            }
            else
            {
                OnTick(EventArgs.Empty);
            }
        }

        // Represents methods that raise events.
        private delegate void EventRaiser(EventArgs e);

        // Represents the method that raises the Tick event.
        private EventRaiser tickRaiser;

        // The ISynchronizeInvoke object to use for marshaling events.
        private ISynchronizeInvoke synchronizingObject = null;

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public TimerMode Mode
        {
            get
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                return mode;
            }

            set
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                mode = value;

                if (IsRunning)
                {
                    Stop();
                    Start();
                }
            }
        }

        TimerMode mode;

        public int Period
        {
            get
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                return (int) period.TotalMilliseconds;
            }
            set
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                var wasRunning = IsRunning;
                
                if (wasRunning)
                {
                    Stop();
                }

                period = TimeSpan.FromMilliseconds(value);

                if (wasRunning)
                {
                    Start();
                }
            }
        }

        public int Resolution
        {
            get
            {
                return resolution;
            }

            set
            {
                resolution = value;
            }
        }

        int resolution;

        // For implementing IComponent.
        private ISite site = null;

        public ISite Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
            }
        }

        /// <summary>
        /// Gets or sets the object used to marshal event-handler calls.
        /// </summary>
        public ISynchronizeInvoke SynchronizingObject
        {
            get
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                return synchronizingObject;
            }
            set
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                synchronizingObject = value;
            }
        }

        public event EventHandler Disposed;
        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler Tick;

        public void Dispose()
        {
            Stop();
            disposed = true;
            OnDisposed(EventArgs.Empty);
        }

        #region Event Raiser Methods

        // Raises the Disposed event.
        private void OnDisposed(EventArgs e)
        {
            EventHandler handler = Disposed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Started event.
        private void OnStarted(EventArgs e)
        {
            EventHandler handler = Started;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Stopped event.
        private void OnStopped(EventArgs e)
        {
            EventHandler handler = Stopped;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Tick event.
        private void OnTick(EventArgs e)
        {
            EventHandler handler = Tick;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion        

        bool disposed = false;

        public void Start()
        {
            #region Require

            if (disposed)
            {
                throw new ObjectDisposedException("Timer");
            }

            #endregion

            #region Guard

            if (IsRunning)
            {
                return;
            }

            #endregion

            // If the periodic event callback should be used.
            if (Mode == TimerMode.Periodic)
            {
                queue.Add(this);
                isRunning = true;
            }
            // Else the one shot event callback should be used.
            else
            {
                throw new NotImplementedException();
            }

            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
            {
                SynchronizingObject.BeginInvoke(
                    new EventRaiser(OnStarted),
                    new object[] { EventArgs.Empty });
            }
            else
            {
                OnStarted(EventArgs.Empty);
            }
        }

        public void Stop()
        {
            #region Require

            if (disposed)
            {
                throw new ObjectDisposedException("Timer");
            }

            #endregion

            #region Guard

            if (!IsRunning)
            {
                return;
            }

            #endregion

            queue.Remove(this);
            isRunning = false;

            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
            {
                SynchronizingObject.BeginInvoke(
                    new EventRaiser(OnStopped),
                    new object[] { EventArgs.Empty });
            }
            else
            {
                OnStopped(EventArgs.Empty);
            }
        }

    }
}
