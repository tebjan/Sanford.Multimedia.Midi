using System;
using System.Collections.Generic;
using System.Text;

namespace Sanford.Multimedia.Midi
{
    public class SysCommonMessageEventArgs : EventArgs
    {
        private SysCommonMessage message;

        public SysCommonMessageEventArgs(SysCommonMessage message)
        {
            this.message = message;
        }

        public SysCommonMessage Message
        {
            get
            {
                return message;
            }
        }
    }
}
