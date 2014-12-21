using System;

namespace Sanford.Multimedia.Midi
{
    public partial class InputDevice
    {
        public event EventHandler<ChannelMessageEventArgs> ChannelMessageReceived;

        public event EventHandler<SysExMessageEventArgs> SysExMessageReceived;

        public event EventHandler<SysCommonMessageEventArgs> SysCommonMessageReceived;

        public event EventHandler<SysRealtimeMessageEventArgs> SysRealtimeMessageReceived;

        public event EventHandler<InvalidShortMessageEventArgs> InvalidShortMessageReceived;

        public event EventHandler<InvalidSysExMessageEventArgs> InvalidSysExMessageReceived;

        protected virtual void OnChannelMessageReceived(ChannelMessageEventArgs e)
        {
            EventHandler<ChannelMessageEventArgs> handler = ChannelMessageReceived;

            if(handler != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnSysExMessageReceived(SysExMessageEventArgs e)
        {
            EventHandler<SysExMessageEventArgs> handler = SysExMessageReceived;

            if(handler != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnSysCommonMessageReceived(SysCommonMessageEventArgs e)
        {
            EventHandler<SysCommonMessageEventArgs> handler = SysCommonMessageReceived;

            if(handler != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnSysRealtimeMessageReceived(SysRealtimeMessageEventArgs e)
        {
            EventHandler<SysRealtimeMessageEventArgs> handler = SysRealtimeMessageReceived;

            if(handler != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnInvalidShortMessageReceived(InvalidShortMessageEventArgs e)
        {
            EventHandler<InvalidShortMessageEventArgs> handler = InvalidShortMessageReceived;

            if(handler != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        protected virtual void OnInvalidSysExMessageReceived(InvalidSysExMessageEventArgs e)
        {
            EventHandler<InvalidSysExMessageEventArgs> handler = InvalidSysExMessageReceived;

            if(handler != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }
    }
}
