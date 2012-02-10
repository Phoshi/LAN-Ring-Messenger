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
        private char _to;
        private char _from;
        private string _message = "";
        private MessageType _type;
        private byte _checksum = 0;
        private Dictionary<MessageType, char> _messageTypes = new Dictionary<MessageType, char>() {
                                                                                                      {MessageType.Login, 'L'},
                                                                                                      {MessageType.Logout, 'X'}, 
                                                                                                      {MessageType.IdentResponse, 'R'},
                                                                                                      {MessageType.Message, 'D'},
                                                                                                      {MessageType.Acknowledge, 'Y'},
                                                                                                      {MessageType.NotAcknowledge, 'N'},
    };

        /// <summary>
        /// Property accessor for the message type
        /// </summary>
        public MessageType Type {
            get { return _type; }
        }

        /// <summary>
        /// Accessor for the packet's checksum
        /// </summary>
        public byte Checksum {
            get {
                if (_checksum == 0) {
                    return MessageChecker.GetChecksum(this);
                }
                return _checksum;
            }
        }

        /// <summary>
        /// Gets the string representing the client which sent the message
        /// </summary>
        public string Sender {
            get { return new string(_from, 1); }
        }

        /// <summary>
        /// Gets the packet payload
        /// </summary>
        public string Payload {
            get { return _message; }
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
        public Message(string message, char recipient, MessageType type) {
            _message = message;
            _to = recipient;
            _type = type;

            if (type == MessageType.Login) {
                _from = recipient;
            }
        }

        /// <summary>
        /// Creates a new message object from the given packet.
        /// </summary>
        /// <param name="data">A byte array containing the packet data.</param>
        public Message(byte[] data) {
            _to = (char) data[1];
            _from = (char) data[2];
            _type = (from kvPair in _messageTypes where kvPair.Value == data[3] select kvPair.Key).First();
            byte[] payloadData = data.Skip(4).Take(10).ToArray();
            _message = ASCIIEncoding.ASCII.GetString(payloadData);
            _checksum = data[14];
        }

        /// <summary>
        /// Returns the packet data for this packet
        /// </summary>
        /// <returns>A byte array representing the packet</returns>
        public byte[] ToByteArray(bool computeChecksum = true) {
            /* Packet Structure
             * {
             *  Destination (Char)
             *  Source (Char)
             *  Type (Char)
             *  Payload (10 Chars)
             *  Checksum (Byte)
             * }
             */

            byte[] byteArray = new byte[16];
            StringBuilder header = new StringBuilder();
            header.Append("{");
            header.Append(_to);
            header.Append(_from);
            header.Append(_messageTypes[_type]);
            byte[] headerBytes = ASCIIEncoding.ASCII.GetBytes(header.ToString());
            Array.Copy(headerBytes, byteArray, headerBytes.Length);
            byte[] payloadBytes = ASCIIEncoding.ASCII.GetBytes(_message);
            Array.Copy(payloadBytes, 0, byteArray, 4, Math.Min(payloadBytes.Length, 10));
            byteArray[14] = computeChecksum ? MessageChecker.GetChecksum(this) : (byte)0;
            byteArray[15] = ASCIIEncoding.ASCII.GetBytes("}")[0];
            return byteArray;
        }

        /// <summary>
        /// Returns a human-readable representation of the message suitable for displaying in some sort of UI.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return _message;
        }
    }
}
