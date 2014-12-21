using System;
using System.Collections;
using System.Text;

namespace Sanford.Multimedia.Midi
{
    public class InvalidSysExMessageEventArgs : EventArgs
    {
        private byte[] messageData;

        public InvalidSysExMessageEventArgs(byte[] messageData)
        {
            this.messageData = messageData;
        }

        public ICollection MessageData
        {
            get
            {
                return messageData;
            }
        }
    }
}
