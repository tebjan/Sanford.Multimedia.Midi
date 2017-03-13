using System;
using System.Collections.Generic;
using System.Text;

namespace Sanford.Multimedia.Midi
{
    public class MetaMessageEventArgs : EventArgs
    {
        private MetaMessage message;

        public MetaMessageEventArgs(MetaMessage message)
        {
            this.message = message;
        }

        public MetaMessage Message
        {
            get
            {
                return message;
            }
        }
    }
}
