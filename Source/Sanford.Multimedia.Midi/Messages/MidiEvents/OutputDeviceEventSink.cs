using System;

namespace Sanford.Multimedia.Midi
{
    /// <summary>
    /// Event sink that sends midi messages to an output device
    /// </summary>
    public class OutputDeviceEventSink : IDisposable
    {
        readonly OutputDevice FOutDevice;
        readonly MidiEvents FEventSource;

        public int DeviceID
        {
            get
            {
                if (FOutDevice != null)
                {
                    return FOutDevice.DeviceID;
                }
                else
                {
                    return -1;
                }
            }
        }

        public OutputDeviceEventSink(OutputDevice outDevice, MidiEvents eventSource)
        {
            FOutDevice = outDevice;
            FEventSource = eventSource;

            RegisterEvents();

        }

        private void RegisterEvents()
        {
            FEventSource.RawMessageReceived += EventSource_RawMessageReceived;
            FEventSource.ChannelMessageReceived += EventSource_ChannelMessageReceived;
            FEventSource.SysCommonMessageReceived += EventSource_SysCommonMessageReceived;
            FEventSource.SysExMessageReceived += EventSource_SysExMessageReceived;
            FEventSource.SysRealtimeMessageReceived += EventSource_SysRealtimeMessageReceived;
        }

        private void UnRegisterEvents()
        {
            FEventSource.RawMessageReceived -= EventSource_RawMessageReceived;
            FEventSource.ChannelMessageReceived -= EventSource_ChannelMessageReceived;
            FEventSource.SysCommonMessageReceived -= EventSource_SysCommonMessageReceived;
            FEventSource.SysExMessageReceived -= EventSource_SysExMessageReceived;
            FEventSource.SysRealtimeMessageReceived -= EventSource_SysRealtimeMessageReceived;
        }


        private void EventSource_SysRealtimeMessageReceived(object sender, SysRealtimeMessageEventArgs e)
        {
            FOutDevice.Send(e.Message);
        }

        private void EventSource_SysExMessageReceived(object sender, SysExMessageEventArgs e)
        {
            FOutDevice.Send(e.Message);
        }

        private void EventSource_SysCommonMessageReceived(object sender, SysCommonMessageEventArgs e)
        {
            FOutDevice.Send(e.Message);
        }

        private void EventSource_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            FOutDevice.Send(e.Message);
        }

        private void EventSource_RawMessageReceived(object sender, RawMessageEventArgs e)
        {
            FOutDevice.SendRaw(e.IntMessage);
        }

        /// <summary>
        /// Disposes the underying output device and removes the events from the source
        /// </summary>
        public void Dispose()
        {
            UnRegisterEvents();
            FOutDevice.Dispose();
        }

        public static OutputDeviceEventSink FromDeviceID(int deviceID, MidiEvents eventSource)
        {
            var deviceCount = OutputDevice.DeviceCount;
            if (deviceCount > 0)
            {
                deviceID %= deviceCount;
                return new OutputDeviceEventSink(new OutputDevice(deviceID), eventSource);
            }
            return null;
        }
    }
}
