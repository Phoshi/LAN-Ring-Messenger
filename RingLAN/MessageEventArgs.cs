using System;

namespace RingLAN {
    /// <summary>
    /// MessageEventArgs objects just store a message object
    /// </summary>
    public class MessageEventArgs : EventArgs {
        public Message Message { get; private set; }

        public MessageEventArgs(Message message) {
            Message = message;
        }
    }
}