using System;

namespace RingLAN {
    public class MessageEventArgs : EventArgs {
        private Message _message;

        public Message Message {
            get { return _message; }
        }

        public MessageEventArgs(Message message) {
            _message = message;
        }
    }
}