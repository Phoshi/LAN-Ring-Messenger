using System;

namespace RingLAN {
    /// <summary>
    /// Stores a pending item
    /// </summary>
    public class Pending {
        /// <summary>
        /// The message to send
        /// </summary>
        public Message Message;
        /// <summary>
        /// The time the message was last sent
        /// </summary>
        public DateTime LastSend;
        /// <summary>
        /// The number of times the message has been sent
        /// </summary>
        public int SendCount;

        /// <summary>
        /// Create a new Pending object
        /// </summary>
        /// <param name="message">The message that shall be sent</param>
        public Pending(Message message) {
            Message = message;
            LastSend = DateTime.MinValue;
            SendCount = 0;
        }
    }
}
