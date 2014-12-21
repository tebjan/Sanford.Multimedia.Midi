#region License

/* Copyright (c) 2005 Leslie Sanford
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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Sanford.Multimedia;

namespace Sanford.Multimedia.Midi
{
    public partial class InputDevice : MidiDevice
    {
        private void HandleMessage(int handle, int msg, int instance, int param1, int param2)
        {
            if(msg == MIM_OPEN)
            {
            }
            else if(msg == MIM_CLOSE)
            {
            }
            else if(msg == MIM_DATA)
            {
                delegateQueue.Post(HandleShortMessage, param1);
            }
            else if(msg == MIM_MOREDATA)
            {
                delegateQueue.Post(HandleShortMessage, param1);
            }
            else if(msg == MIM_LONGDATA)
            {
                delegateQueue.Post(HandleSysExMessage, new IntPtr(param1));
            }
            else if(msg == MIM_ERROR)
            {
                delegateQueue.Post(HandleInvalidShortMessage, param1);
            }
            else if(msg == MIM_LONGERROR)
            {
                delegateQueue.Post(HandleInvalidSysExMessage, new IntPtr(param1));
            }
        }

        private void HandleShortMessage(object state)
        {
            int message = (int)state;
            int status = ShortMessage.UnpackStatus(message);

            if(status >= (int)ChannelCommand.NoteOff &&
                status <= (int)ChannelCommand.PitchWheel +
                ChannelMessage.MidiChannelMaxValue)
            {
                cmBuilder.Message = message;
                cmBuilder.Build();

                OnChannelMessageReceived(new ChannelMessageEventArgs(cmBuilder.Result));
            }
            else if(status == (int)SysCommonType.MidiTimeCode ||
                status == (int)SysCommonType.SongPositionPointer ||
                status == (int)SysCommonType.SongSelect ||
                status == (int)SysCommonType.TuneRequest)
            {
                scBuilder.Message = message;
                scBuilder.Build();

                OnSysCommonMessageReceived(new SysCommonMessageEventArgs(scBuilder.Result));
            }
            else
            {
                SysRealtimeMessageEventArgs e = null;

                switch((SysRealtimeType)status)
                {
                    case SysRealtimeType.ActiveSense:
                        e = SysRealtimeMessageEventArgs.ActiveSense;
                        break;

                    case SysRealtimeType.Clock:
                        e = SysRealtimeMessageEventArgs.Clock;
                        break;

                    case SysRealtimeType.Continue:
                        e = SysRealtimeMessageEventArgs.Continue;
                        break;

                    case SysRealtimeType.Reset:
                        e = SysRealtimeMessageEventArgs.Reset;
                        break;

                    case SysRealtimeType.Start:
                        e = SysRealtimeMessageEventArgs.Start;
                        break;

                    case SysRealtimeType.Stop:
                        e = SysRealtimeMessageEventArgs.Stop;
                        break;

                    case SysRealtimeType.Tick:
                        e = SysRealtimeMessageEventArgs.Tick;
                        break;
                }

                OnSysRealtimeMessageReceived(e);
            }
        }

        private void HandleSysExMessage(object state)
        {
            lock(lockObject)
            {
                IntPtr headerPtr = (IntPtr)state;

                MidiHeader header = (MidiHeader)Marshal.PtrToStructure(headerPtr, typeof(MidiHeader));

                if(!resetting)
                {
                    for(int i = 0; i < header.bytesRecorded; i++)
                    {
                        sysExData.Add(Marshal.ReadByte(header.data, i));
                    }

                    if(sysExData[0] == 0xF0 && sysExData[sysExData.Count - 1] == 0xF7)
                    {
                        SysExMessage message = new SysExMessage(sysExData.ToArray());

                        sysExData.Clear();

                        OnSysExMessageReceived(new SysExMessageEventArgs(message));
                    }

                    int result = AddSysExBuffer();

                    if(result != DeviceException.MMSYSERR_NOERROR)
                    {
                        Exception ex = new InputDeviceException(result);

                        OnError(new ErrorEventArgs(ex));
                    }
                }

                ReleaseBuffer(headerPtr);
            }
        }

        private void HandleInvalidShortMessage(object state)
        {
            OnInvalidShortMessageReceived(new InvalidShortMessageEventArgs((int)state));
        }

        private void HandleInvalidSysExMessage(object state)
        {
            lock(lockObject)
            {
                IntPtr headerPtr = (IntPtr)state;

                MidiHeader header = (MidiHeader)Marshal.PtrToStructure(headerPtr, typeof(MidiHeader));

                if(!resetting)
                {
                    byte[] data = new byte[header.bytesRecorded];

                    Marshal.Copy(header.data, data, 0, data.Length);

                    OnInvalidSysExMessageReceived(new InvalidSysExMessageEventArgs(data));

                    int result = AddSysExBuffer();

                    if(result != DeviceException.MMSYSERR_NOERROR)
                    {
                        Exception ex = new InputDeviceException(result);

                        OnError(new ErrorEventArgs(ex));
                    }
                }

                ReleaseBuffer(headerPtr);
            }
        }

        private void ReleaseBuffer(IntPtr headerPtr)
        {
            int result = midiInUnprepareHeader(Handle, headerPtr, SizeOfMidiHeader);

            if(result != DeviceException.MMSYSERR_NOERROR)
            {
                Exception ex = new InputDeviceException(result);

                OnError(new ErrorEventArgs(ex));
            }

            headerBuilder.Destroy(headerPtr);

            bufferCount--;

            Debug.Assert(bufferCount >= 0);

            Monitor.Pulse(lockObject);
        }

        public int AddSysExBuffer()
        {
            int result;

            // Initialize the MidiHeader builder.
            headerBuilder.BufferLength = sysExBufferSize;
            headerBuilder.Build();

            // Get the pointer to the built MidiHeader.
            IntPtr headerPtr = headerBuilder.Result;

            // Prepare the header to be used.
            result = midiInPrepareHeader(Handle, headerPtr, SizeOfMidiHeader);

            // If the header was perpared successfully.
            if(result == DeviceException.MMSYSERR_NOERROR)
            {
                bufferCount++;

                // Add the buffer to the InputDevice.
                result = midiInAddBuffer(Handle, headerPtr, SizeOfMidiHeader);

                // If the buffer could not be added.
                if(result != MidiDeviceException.MMSYSERR_NOERROR)
                {
                    // Unprepare header - there's a chance that this will fail 
                    // for whatever reason, but there's not a lot that can be
                    // done at this point.
                    midiInUnprepareHeader(Handle, headerPtr, SizeOfMidiHeader);

                    bufferCount--;

                    // Destroy header.
                    headerBuilder.Destroy();
                }
            }
            // Else the header could not be prepared.
            else
            {
                // Destroy header.
                headerBuilder.Destroy();
            }

            return result;
        }
    }
}