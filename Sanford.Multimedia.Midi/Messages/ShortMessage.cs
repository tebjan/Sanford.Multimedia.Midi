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

namespace Sanford.Multimedia.Midi
{
	/// <summary>
	/// Represents the basic class for all MIDI short messages.
	/// </summary>
    /// <remarks>
    /// MIDI short messages represent all MIDI messages except meta messages
    /// and system exclusive messages. This includes channel messages, system
    /// realtime messages, and system common messages.
    /// </remarks>
	public abstract class ShortMessage : IMidiMessage
	{
        #region ShortMessage Members

        #region Constants

        public const int DataMaxValue= 127;

        public const int StatusMaxValue = 255;

        //
        // Bit manipulation constants.
        //

        private const int StatusMask = ~255;
        protected const int DataMask = ~StatusMask;
        private const int Data1Mask = ~65280;
        private const int Data2Mask = ~Data1Mask + DataMask;
        private const int Shift = 8;

        #endregion

        protected int msg = 0;

        #region Methods

        public byte[] GetBytes()
        {
            return BitConverter.GetBytes(msg);
        }

        internal static int PackStatus(int message, int status)
        {
            #region Require

            if(status < 0 || status > StatusMaxValue)
            {
                throw new ArgumentOutOfRangeException("status", status,
                    "Status value out of range.");
            }

            #endregion            

            return (message & StatusMask) | status;
        }

        internal static int PackData1(int message, int data1)
        {
            #region Require

            if(data1 < 0 || data1 > DataMaxValue)
            {
                throw new ArgumentOutOfRangeException("data1", data1,
                    "Data 1 value out of range.");
            }

            #endregion

            return (message & Data1Mask) | (data1 << Shift);
        }

        internal static int PackData2(int message, int data2)
        {
            #region Require

            if(data2 < 0 || data2 > DataMaxValue)
            {
                throw new ArgumentOutOfRangeException("data2", data2,
                    "Data 2 value out of range.");
            }

            #endregion

            return (message & Data2Mask) | (data2 << (Shift * 2));
        }

        internal static int UnpackStatus(int message)
        {
            return message & DataMask;
        }

        internal static int UnpackData1(int message)
        {
            return (message & ~Data1Mask) >> Shift;
        }

        internal static int UnpackData2(int message)
        {
            return (message & ~Data2Mask) >> (Shift * 2);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the short message as a packed integer.
        /// </summary>
        /// <remarks>
        /// The message is packed into an integer value with the low-order byte
        /// of the low-word representing the status value. The high-order byte
        /// of the low-word represents the first data value, and the low-order
        /// byte of the high-word represents the second data value.
        /// </remarks>
        public int Message
        {
            get
            {
                return msg;
            }
        }

        /// <summary>
        /// Gets the messages's status value.
        /// </summary>
        public int Status
        {
            get
            {
                return UnpackStatus(msg);
            }
        }

        public abstract MessageType MessageType
        {
            get;
        }
   
        #endregion

        #endregion
	}
}
