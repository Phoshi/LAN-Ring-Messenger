using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    /// <summary>
    /// A low-level message. Basically just the packet.
    /// </summary>
    public class LowLevelMessage {

        /// <summary>
        /// Returns the raw byte array for the packet in the format:
        /// { (Byte)
        ///     Recipient (Byte)
        ///     Sender (Byte)
        ///     Message (26 bytes)
        ///     Message Type (Byte)
        ///     Checksum (Byte)
        /// } (Byte)
        /// </summary>
        /// <returns>A byte array representing an ordered packet</returns>
        public byte[] ToByteArray() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a Message object representing the Low Level Message's data
        /// </summary>
        /// <returns>A message object</returns>
        public Message ToMessage() {
            throw new NotImplementedException();
        }
    }
}
