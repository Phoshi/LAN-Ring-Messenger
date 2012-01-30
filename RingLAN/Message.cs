using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    /// <summary>
    /// A high-level message object that doesn't care about the low-level packet implementation one jot.
    /// </summary>
    public class Message {
        //
        //Class variables
        //
        private int _to = -1;
        private int _from = -1;
        private string _message = "";
        private MessageType _type;

        /// <summary>
        /// Property accessor for the message type
        /// </summary>
        public MessageType Type {
            get { return _type; }
        }
        //
        //Constructors
        //
        /// <summary>
        /// Creates a new message object to another client, of specified type
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="recipient">The client ID to send to</param>
        /// <param name="type">The type of message to send</param>
        public Message(string message, int recipient, MessageType type) {
            _message = message;
            _to = recipient;
        }

        /// <summary>
        /// Returns a human-readable representation of the message suitable for displaying in some sort of UI.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return base.ToString();
        }
    }
}
