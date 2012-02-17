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
        public static bool Check(Message message) {
            byte correctChecksum = GetChecksum(message);
            //return true;
            return correctChecksum == message.Checksum;
        }

        /// <summary>
        /// Gets the checksum for message data (Obviously ignoring the checksum part if it already exists)
        /// </summary>
        /// <param name="message">The message object</param>
        /// <returns>The one-byte checksum</returns>
        public static byte GetChecksum(Message message) {
            int total = 0;
            byte[] byteArray = message.ToByteArray(false); //Get message bytes without a computed checksum
            foreach (byte b in byteArray) {
                total += b;
            }
            return ((byte) (~(total % 128)));
        }
    }
}
