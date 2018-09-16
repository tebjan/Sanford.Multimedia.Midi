#region License

/* Copyright (c) 2006 Leslie Sanford
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

#endregion

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace VL.Reactive.Timer
{
    /// <summary>
    /// Represents a timer with very high accuracy.
    /// </summary>
    public sealed class BusyWaitTimer : IComponent
    {
        #region Timer Members

        #region Delegates

        // Represents the method that is called by Windows when a timer event occurs.
        private delegate void TimeProc(int id, int msg, int user, int param1, int param2);

        // Represents methods that raise events.
        private delegate void EventRaiser(EventArgs e);

        #endregion

        #region Win32 Multimedia Timer Functions

        // Gets timer capabilities.
        [DllImport("winmm.dll")]
        private static extern int timeGetDevCaps(ref TimerCaps caps, int sizeOfTimerCaps);

        // Creates and starts the timer.
        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimeProc proc, IntPtr user, int mode);

        // Stops and destroys the timer.
        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);

        // Indicates that the operation was successful.
        private const int TIMERR_NOERROR = 0;

        #endregion

        #region Fields

        // Timer identifier.
        private int timerID;

        // Timer identifier.
        private int timerIDTest;

        // Timer mode.
        private volatile TimerMode mode;

        // Period between timer events in milliseconds.
        private volatile int period;

        // Timer resolution in milliseconds.
        private volatile int resolution;        

        // Called by Windows when a timer periodic event occurs.
        //private TimeProc timeProcPeriodic;

        // Called by Windows when a timer one shot event occurs.
        private TimeProc timeProcOneShot;

        // Called by Windows when a timer one shot event occurs.
        private TimeProc timeProcOneShotTest;

        // Represents the method that raises the Tick event.
        private EventRaiser tickRaiser;

        // The ISynchronizeInvoke object to use for marshaling events.
        private ISynchronizeInvoke synchronizingObject = null;

        // Indicates whether or not the timer is running.
        private volatile bool running = false;

        // Indicates whether or not the timer has been disposed.
        private volatile bool disposed = false;

        // For implementing IComponent.
        private ISite site = null;

        // Multimedia timer capabilities.
        private static TimerCaps caps;

        // time measurement
        private Stopwatch stopwatch;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Timer has started;
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the Timer has stopped;
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when the time period has elapsed.
        /// </summary>
        public event EventHandler Tick;

        #endregion

        #region Construction

        /// <summary>
        /// Initialize class.
        /// </summary>
        static BusyWaitTimer()
        {
            // Get multimedia timer capabilities.
            timeGetDevCaps(ref caps, Marshal.SizeOf(caps));
        }

        /// <summary>
        /// Initializes a new instance of the Timer class with the specified IContainer.
        /// </summary>
        /// <param name="container">
        /// The IContainer to which the Timer will add itself.
        /// </param>
        public BusyWaitTimer(IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the Timer class.
        /// </summary>
        public BusyWaitTimer()
        {
            Initialize();
        }

        ~BusyWaitTimer()
        {
            if(IsRunning)
            {
                // Stop and destroy timer.s
                Stop();
            }
        }

        // Initialize timer with default values.
        private void Initialize()
        {
            this.mode = TimerMode.Periodic;
            this.period = Capabilities.periodMin;
            this.resolution = 1;

            running = false;

            timeProcOneShot = new TimeProc(OneShotEventCallback);
            timeProcOneShotTest = new TimeProc(OneShotEventCallbackTest);
            tickRaiser = new EventRaiser(OnTick);

            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        #endregion

        #region Methods

        TimeSpan FOldTime;
        TimeSpan FAfterFrameTime;
        TimeSpan FInterval;
        TimeSpan FCallbackAccuracy = TimeSpan.FromTicks((long)Math.Ceiling(1.5 * TimeSpan.TicksPerMillisecond));

        public TimeSpan LastInterval
        {
            get;
            private set;
        }

        public TimeSpan LastIntervalDiff
        {
            get;
            private set;
        }

        public TimeSpan LastTestDiff
        {
            get;
            private set;
        }

        public long LastTestDiffMax
        {
            get;
            private set;
        }

        TimeSpan LastTestTime;
        bool firstTest = true;
        void TimerFunctionTest()
        {
            while (running)
            {
                var t = stopwatch.Elapsed;
                LastTestDiff = t - LastTestTime;
                LastTestTime = t;

                if (!firstTest)
                    LastTestDiffMax = Math.Max(LastTestDiffMax, LastTestDiff.Ticks);

                firstTest = false;

                //Thread.Sleep(1);

                ////advance time
                timerIDTest = timeSetEvent(4, 1, timeProcOneShotTest, IntPtr.Zero, 0);
                return; //we are finished, the timeProcOneShotTest will call this method back again
            }
        }


        void TimerFunction()
        {          
            while (running)
            {
                var t = stopwatch.Elapsed;
                var remainingTime = FInterval - (t - FOldTime);

                if (remainingTime.Ticks > 0)
                {
                    //if there is more time left than the multimedia timer resolution do callback
                    if (remainingTime.Ticks > FCallbackAccuracy.Ticks)
                    {
                        //advance time
                        timerID = timeSetEvent((int)(remainingTime.Ticks / TimeSpan.TicksPerMillisecond), 1, timeProcOneShot, IntPtr.Zero, 0);
                        return; //we are finished, the timeProcOneShot will call this method back again

                        //Thread.Sleep(1);
                        //continue;
                    }
                    else //busy wait last time < 1 millisecond, should be an average of 0.5 ms
                    {
                        while (remainingTime.Ticks > 2)
                        {
                            //Thread.SpinWait(1)?
                            t = stopwatch.Elapsed;
                            remainingTime = FInterval - (t - FOldTime);
                        }
                    }
                }

                //if wait timer done, we arrive here
                var interval = t - FOldTime;
                LastIntervalDiff = interval - LastInterval;
                LastInterval = interval;
                FOldTime = t;
                //do the actual work
                DoTimer();
                FAfterFrameTime = stopwatch.Elapsed;
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The timer has already been disposed.
        /// </exception>
        /// <exception cref="TimerStartException">
        /// The timer failed to start.
        /// </exception>
        public void Start()
        {
            #region Require

            if(disposed)
            {
                throw new ObjectDisposedException("Timer");
            }

            #endregion

            #region Guard

            if(IsRunning)
            {
                return;
            }

            #endregion

            // start thread which does the timer work
            running = true;
            var thread = new Thread(TimerFunctionStart);
            thread.Priority = ThreadPriority.Highest;
            thread.Start();

            var threadTest = new Thread(TimerFunctionTest);
            threadTest.Priority = ThreadPriority.Lowest;
            threadTest.Start();

            // If the timer was created successfully.
            if (thread.IsAlive)
            {
                if(SynchronizingObject != null && SynchronizingObject.InvokeRequired)
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
            else
            {
                running = false;
                throw new TimerStartException("Unable start Timer.");
            }
        }

        private void TimerFunctionStart()
        {
            StartedAt = stopwatch.Elapsed;
            TimerFunction();
        }

        /// <summary>
        /// Stops timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        public void Stop()
        {
            #region Require

            if(disposed)
            {
                throw new ObjectDisposedException("Timer");
            }

            #endregion

            #region Guard

            if(!running)
            {
                return;
            }

            #endregion

            running = false;
            firstTest = true;
            LastTestDiffMax = 0;

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

        #region Callbacks

        void DoTimer()
        {
            switch (mode)
            {
                case TimerMode.OneShot:
                    TimerOneShotEventCallback();
                    break;
                case TimerMode.Periodic:
                    TimerPeriodicEventCallback();
                    break;
            }
        }

        // Callback method called by the Win32 multimedia timer when a timer
        // periodic event occurs.
        private void TimerPeriodicEventCallback()
        {
            #region Guard

            if(disposed)
            {
                return;
            }

            #endregion

            if(synchronizingObject != null)
            {
                synchronizingObject.BeginInvoke(tickRaiser, new object[] { EventArgs.Empty });
            }
            else
            {
                OnTick(EventArgs.Empty);
            }
        }

        // Callback method called by the Win32 multimedia timer when a timer
        // one shot event occurs.
        private void TimerOneShotEventCallback()
        {
            #region Guard

            if(disposed)
            {
                return;
            }

            #endregion

            if(synchronizingObject != null)
            {
                synchronizingObject.BeginInvoke(tickRaiser, new object[] { EventArgs.Empty });
                Stop();
            }
            else
            {
                OnTick(EventArgs.Empty);
                Stop();
            }
        }

        // Callback method called by the Win32 multimedia timer when a timer
        // one shot event occurs.
        private void OneShotEventCallback(int id, int msg, int user, int param1, int param2)
        {
            #region Guard

            if (disposed)
            {
                return;
            }

            #endregion

            //call timer function again
            TimerFunction();
        }

        // Callback method called by the Win32 multimedia timer when a timer
        // one shot event occurs.
        private void OneShotEventCallbackTest(int id, int msg, int user, int param1, int param2)
        {
            #region Guard

            if (disposed)
            {
                return;
            }

            #endregion

            //call timer function again
            TimerFunctionTest();
        }

        #endregion

        #region Event Raiser Methods

        // Raises the Disposed event.
        private void OnDisposed(EventArgs e)
        {
            EventHandler handler = Disposed;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Started event.
        private void OnStarted(EventArgs e)
        {
            EventHandler handler = Started;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Stopped event.
        private void OnStopped(EventArgs e)
        {
            EventHandler handler = Stopped;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Tick event.
        private void OnTick(EventArgs e)
        {
            EventHandler handler = Tick;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        #endregion        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the object used to marshal event-handler calls.
        /// </summary>
        public ISynchronizeInvoke SynchronizingObject
        {
            get
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                return synchronizingObject;
            }
            set
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                synchronizingObject = value;
            }
        }

        /// <summary>
        /// Gets or sets the time between Tick events.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>   
        public int Period
        {
            get
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                return period;
            }
            set
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }
                else if(value < Capabilities.periodMin || value > Capabilities.periodMax)
                {
                    throw new ArgumentOutOfRangeException("Period", value,
                        "Multimedia Timer period out of range.");
                }

                #endregion

                period = value;

                if(IsRunning)
                {
                    Stop();
                    Start();
                }
            }
        }

        /// <summary>
        /// Gets or sets the timer resolution.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>        
        /// <remarks>
        /// The resolution is in milliseconds. The resolution increases 
        /// with smaller values; a resolution of 0 indicates periodic events 
        /// should occur with the greatest possible accuracy. To reduce system 
        /// overhead, however, you should use the maximum value appropriate 
        /// for your application.
        /// </remarks>
        public int Resolution
        {
            get
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                return resolution;
            }
            set
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }
                else if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Resolution", value,
                        "Multimedia timer resolution out of range.");
                }

                #endregion

                resolution = value;

                if(IsRunning)
                {
                    Stop();
                    Start();
                }
            }
        }



        /// <summary>
        /// Gets the timer mode.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        public TimerMode Mode
        {
            get
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                return mode;
            }
            set
            {
                #region Require

                if(disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                #endregion

                if (mode != value)
                {
                    mode = value;

                    if (IsRunning)
                    {
                        Stop();
                        Start();
                    } 
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Timer is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return running;
            }
        }

        /// <summary>
        /// Returns the time since the timer was created.
        /// Internally uses the .NET Stopwatch.
        /// </summary>
        public TimeSpan Now
        {
            get
            {
                return stopwatch.Elapsed;
            }
        }

        /// <summary>
        /// Gets the timer capabilities.
        /// </summary>
        public static TimerCaps Capabilities
        {
            get
            {
                return caps;
            }
        }

        #endregion

        #endregion

        #region IComponent Members

        public event System.EventHandler Disposed;

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


        
        public double Frequency
        {
            get
            {
                return (double)TimeSpan.TicksPerSecond / FInterval.Ticks;
            }

            set
            {
                FInterval = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / value));
            }
        }

        public TimeSpan StartedAt { get; private set; }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Frees timer resources.
        /// </summary>
        public void Dispose()
        {
            #region Guard

            if(disposed)
            {
                return;
            }

            #endregion               

            disposed = true;

            if(running)
            {
                // Stop and destroy timer.
                Stop();
            }                      

            OnDisposed(EventArgs.Empty);
        }

        #endregion       
    }
}
