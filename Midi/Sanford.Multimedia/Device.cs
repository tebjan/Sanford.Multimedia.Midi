#region License

/* Copyright (c) 2006 Leslie Sanford
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
using System.Collections.Generic;
using System.Threading;

namespace Sanford.Multimedia
{
    public abstract class Device : IDisposable
    {
        protected const int CALLBACK_FUNCTION = 0x30000;

        protected const int CALLBACK_EVENT = 0x50000;

        private int deviceID;

        protected SynchronizationContext context;

        // Indicates whether the device has been disposed.
        private bool disposed = false;

        public event EventHandler<ErrorEventArgs> Error;

        public Device(int deviceID)
        {
            this.deviceID = deviceID;

            if(SynchronizationContext.Current == null)
            {
                context = new SynchronizationContext();
            }
            else
            {
                context = SynchronizationContext.Current;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                disposed = true;

                GC.SuppressFinalize(this);
            }
        }

        protected virtual void OnError(ErrorEventArgs e)
        {
            EventHandler<ErrorEventArgs> handler = Error;

            if(handler != null)
            {
                context.Post(delegate(object dummy)
                {
                    handler(this, e);
                }, null);
            }
        }

        /// <summary>
        /// Closes the MIDI device.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Resets the device.
        /// </summary>
        public abstract void Reset();        

        /// <summary>
        /// Gets the device handle.
        /// </summary>
        public abstract int Handle
        {
            get;
        }

        public int DeviceID
        {
            get
            {
                return deviceID;
            }
        }

        public bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }

        #region IDisposable

        /// <summary>
        /// Disposes of the device.
        /// </summary>
        public abstract void Dispose();

        #endregion
    }
}
