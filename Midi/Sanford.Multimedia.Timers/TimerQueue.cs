using System;
using System.Collections.Generic;
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
            lock (this)
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
            lock (this)
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

                for (int empty = 0; empty < 3; ++empty)
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
                        if (tick.Timer.Mode == TimerMode.Periodic)
                        {
                            tick.Time += tick.Timer.period;
                            ticks.Sort();
                        }
                        else
                        {
                            ticks.RemoveAt(0);
                        }
                    }
                }
                loop = null;
            }
        }
    }
}