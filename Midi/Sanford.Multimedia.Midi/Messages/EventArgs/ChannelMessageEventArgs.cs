using System;
using System.Collections.Generic;
using System.Text;

namespace Sanford.Multimedia.Midi
{
    public class ChannelMessageEventArgs : MidiEventArgsBase
    {
        private ChannelMessage message;

        public ChannelMessageEventArgs(ChannelMessage message)
        {
            this.message = message;
        }

        public ChannelMessage Message
        {
            get
            {
                return message;
            }
        }
    }
}
