using System;
using System.Collections.Generic;
using System.Text;

namespace Sanford.Multimedia.Midi
{
    public class SysExMessageEventArgs : EventArgs
    {
        private SysExMessage message;

        public SysExMessageEventArgs(SysExMessage message)
        {
            this.message = message;
        }

        public SysExMessage Message
        {
            get
            {
                return message;
            }
        }
    }
}
