using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions;

namespace RingLAN {
    /// <summary>
    /// A high-level message object.
    /// </summary>
    public class Message {
        //
        //Class variables
        //
        private readonly char _to;
        private readonly char _from;
        private readonly string _message = "";
        private readonly MessageType _type;
        private readonly byte _checksum;
        private readonly Dictionary<MessageType, char[]> _messageTypes = new Dictionary<MessageType, char[]> {
                                                                                                      {MessageType.Login, new[]{'L'}},
                                                                                                      {MessageType.Logout, new[]{'X'}}, 
                                                                                                      {MessageType.IdentResponse, new[]{'R'}},
                                                                                                      {MessageType.Message, new[]{'D'}},
                                                                                                      {MessageType.Acknowledge, new[]{'Y', 'A'}},
                                                                                                      {MessageType.NotAcknowledgable, new[]{'N'}},
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
                return _checksum == 0 ? MessageChecker.GetChecksum(this) : _checksum;
            }
        }

        /// <summary>
        /// Gets the string representing the client which sent the message
        /// </summary>
        public string Sender {
            get { return Names.GetName(_from); }
        }

        /// <summary>
        /// Gets the sender address
        /// </summary>
        public char SenderAddress {
            get { return _from; }
        }

        /// <summary>
        /// Gets the string representing the client the message is addressed to
        /// </summary>
        public string Recipient {
            get { return Names.GetName(_to); }
        }

        /// <summary>
        /// Gets the address of the message
        /// </summary>
        public char Address {
            get { return _to; }
        }

        /// <summary>
        /// Gets the packet payload
        /// </summary>
        public string Payload {
            get { return _message; }
        }

        //
        // Properties to generate reply messages
        //

        /// <summary>
        /// Generates a non-acknowledgable message for packet failure
        /// </summary>
        public Message NotAcknowledgable {
            get {
                return new Message(null, this._from, this._to, MessageType.NotAcknowledgable);
            }
        }

        /// <summary>
        /// Generates an acknowledge message for this messaage
        /// </summary>
        public Message Acknowledge {
            get {
                return new Message(null, this._from, this._to, MessageType.Acknowledge);
            }
        }

        //
        //Constructors
        //
        /// <summary>
        /// Creates a new message object to another client, of specified type
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="recipient">The client ID to send to</param>
        /// <param name="from">The client the message is from</param>
        /// <param name="type">The type of message to send</param>
        public Message(string message, char recipient, char from, MessageType type) {
            _message = message;
            if (_message !=null && _message.Length > 10) {
                _message = _message.Substring(0, 10);
            }
            _to = recipient;
            _from = from;
            _type = type;

            if (type == MessageType.Login) {
                _from = recipient;
            }
        }

        /// <summary>
        /// Creates a new message object from the given packet.
        /// </summary>
        /// <param name="data">A byte array containing the packet data.</param>
        public Message(IList<byte> data) {
            try {
                _to = (char)data[1];
                _from = (char)data[2];
                foreach (var kvPair in _messageTypes){
                    if (kvPair.Value.Contains((char)data[3])){
                        _type = kvPair.Key;
                    }
                }
                byte[] payloadData = data.Skip(4).Take(10).ToArray();
                _message = Encoding.ASCII.GetString(payloadData).Trim('\0');
                _checksum = data[14];
            }
            catch (Exception) {
                /*  I want to be the very best,
                    Like no one ever was.
                    To catch them is my real test,
                    To train them is my cause.

                    I will travel across the land,
                    Searching far and wide.
                    Each Pokemon to understand
                    The power that's inside

                    (Gotta catch 'em all)

                    It's you and me
                    I know it's my destiny

                    Pokemon!

                    You're my best friend,
                    In a world we must defend.
                    Pokemon

                    (Gotta catch 'em all)

                    A heart so true.
                    Our courage will pull us through.
                    You teach me and I'll teach you.
                    Pokemon.

                    (Gotta catch 'em all)

                    Yeah
                    Every challenge along the way
                    with courage I will face.
                    I will battle everyday
                    to claim my rightful place.

                    Come with me the time is right.
                    There's no better team.
                    Arm and arm well win the fight.
                    It's always been our dream.

                    Pokemon!

                    (Gotta catch 'em all)

                    It's you and me
                    I know it's my destiny

                    Pokemon!

                    Oh, your my best friend,
                    in a world we must defend.

                    Pokemon!

                    A heart so true.
                    Our courage will pull us through.
                    You teach me and ill teach you.

                    Pokemon!

                    (Gotta catch 'em all)x4

                    Yeah!

                    Pokemon!

                    It's you and me
                    I know it's my destiny

                    Pokemon!

                    Oh, your my best friend,
                    in a world we must defend.

                    Pokemon!

                    A heart so true.
                    Our courage will pull us through.
                    You teach me and ill teach you.*/
            }
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
            header.Append(_messageTypes[_type].First());
            byte[] headerBytes = Encoding.ASCII.GetBytes(header.ToString());
            Array.Copy(headerBytes, byteArray, headerBytes.Length);
            byte[] payloadBytes = _message != null ? Encoding.ASCII.GetBytes(_message) : new byte[10];
            Array.Copy(payloadBytes, 0, byteArray, 4, Math.Min(payloadBytes.Length, 10));
            byteArray[14] = computeChecksum ? this.Checksum : (byte)0;
            byteArray[15] = Encoding.ASCII.GetBytes("}")[0];
            return byteArray;
        }

        /// <summary>
        /// Returns a human-readable representation of the message suitable for displaying in some sort of UI.
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() {
            StringBuilder packet = new StringBuilder();
            foreach (byte b in ToByteArray()){
                packet.Append((char)b);
            }
            return packet.ToString();
            return _message ?? "Packet type {0}".With(_messageTypes[_type]);
        }

        /// <summary>
        /// Returns equality
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>True if the packets are equal</returns>
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            Message other = (Message) obj;
            return other.ToByteArray().SequenceEqual(this.ToByteArray());
        }

        /// <summary>
        /// Returns a hashcode for this object
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode() {
            int hash = 13;
            foreach (byte b in this.ToByteArray()) {
                hash = (hash * 7) + b.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Returns whether two objects are equal
        /// </summary>
        /// <param name="me">One object</param>
        /// <param name="other">The other</param>
        /// <returns>Equality</returns>
        public static bool operator ==(Message me, Message other) {
            if (ReferenceEquals(me, other)) {
                return true;
            }
            if ((object)me == null || (object)other == null) {
                return false;
            }
            return me.Equals(other);
        }

        /// <summary>
        /// Returns whether two objects are not equal
        /// </summary>
        /// <param name="me">One object</param>
        /// <param name="other">The other</param>
        /// <returns>Not equality</returns>
        public static bool operator !=(Message me, Message other) {
            return !(me == other);
        }
    }
}
