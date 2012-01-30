using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    /// <summary>
    /// Checks a message against its checksum to ensure it was communicated properly.
    /// Also generates checksums so the other end can do the same.
    /// </summary>
    static class MessageChecker {
        /// <summary>
        /// Checks to see if a message is corrupt
        /// </summary>
        /// <param name="message">The message object</param>
        /// <returns>Integrity - true for valid, false for invalid</returns>
        public static bool Check(LowLevelMessage message) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the checksum for message data (Obviously ignoring the checksum part if it already exists)
        /// </summary>
        /// <param name="message">The message object</param>
        /// <returns>The one-byte checksum</returns>
        public static byte GetChecksum(LowLevelMessage message) {
            throw new NotImplementedException();
        }
    }
}
