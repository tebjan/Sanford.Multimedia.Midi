using Sanford.Multimedia.Timers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sanford.Multimedia.Timers
{
    public static class TimerFactory
    {
        public static ITimer Create()
        {
            try
            {
                return new Timer();
            }
            catch
            {
                return new ThreadTimer();
            }
        }
    }
}
