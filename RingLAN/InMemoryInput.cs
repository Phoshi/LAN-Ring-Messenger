using System;
using System.Collections.Generic;
using System.Threading;
using Extensions;

namespace RingLAN {
    /// <summary>
    /// Class to provide in-memory ring support
    /// </summary>
    class InMemoryInput : COMInput {
        /// <summary>
        /// An internal buffer to simulate the RS-232 cable
        /// </summary>
        private readonly List<byte> _buffer = new List<byte>();

        /// <summary>
        /// The chance of random simulated failures on the line
        /// </summary>
        public int quality = 10;

        /// <summary>
        /// Gets the simulated buffer
        /// </summary>
        public List<byte> Buffer {
            get { return _buffer; }
        }

        /// <summary>
        /// The In-Memory partner object to "transmit" to
        /// </summary>
        private InMemoryInput _partner;

        /// <summary>
        /// Gets and sets the partner object to "transmit" to
        /// </summary>
        public InMemoryInput Partner {
            get { return _partner; }
            set { _partner = value; }
        }

        /// <summary>
        /// Dummy constructors
        /// </summary>
        /// <param name="dummyName">A dummy string that nothing is done with</param>
        public InMemoryInput(string dummyName = "") {
        }

        /// <summary>
        /// A constructor to set up the object with a transmission partner
        /// </summary>
        /// <param name="partner"></param>
        public InMemoryInput(InMemoryInput partner) {
            _partner = partner;
        }

        /// <summary>
        /// Override to pull a byte out of the simulated buffer
        /// </summary>
        /// <returns>A byte from the transmitting partner object</returns>
        public override byte getByte() {
            while (_buffer.Count == 0) {
                Thread.Sleep(100);
            }
            byte firstByte = _buffer[0];
            _buffer.RemoveAt(0);
            return firstByte;
        }

        /// <summary>
        /// Override to insert a series of bytes into the buffer
        /// Will also simulate line noise, random corruption, and switch Ack packets to simulate different clients
        /// following different standards
        /// </summary>
        /// <param name="toWrite">Byte array to insert</param>
        public override void putChars(byte[] toWrite) {
            Random rng = new Random();
            int failure1 = rng.Next(quality);
            int failure2 = rng.Next(quality);
            int failure3 = rng.Next(quality);
            int swapAck = rng.Next(2);

            if (failure1 == 0) { //Failure!
                Logger.Log("Corrupting packet {0}".With(new Message(toWrite).ToString()), "Failure");
                int position = rng.Next(toWrite.Length);
                toWrite[position] = (byte) (~toWrite[position]); 
            }

            if (failure2 == 0) { //Critial Failure!
                Logger.Log("Deleting Packet {0}".With(new Message(toWrite).ToString()), "Failure");
                return;
            }

            if (failure3 == 0) { //Packet hilariously malformed!
                Logger.Log("Deforming packet {0}".With(new Message(toWrite).ToString()), "Failure");
                byte[] newBuffer = new byte[20];
                Array.Copy(toWrite, newBuffer, 10);
                Array.Copy(toWrite, 10, newBuffer, 14, 6);
                toWrite = newBuffer;
            }

            if (toWrite[3] == (byte)'Y' && swapAck > 0){
                toWrite[3] = (byte)'A';
                Logger.Log("Swapping Y ack packet for an A ack packet!", "Failure");
            }

            Logger.Log("Sending Packet {0}".With(new Message(toWrite).ToString()), "In Memory Input");
            if (_partner == null) {
                _buffer.AddRange(toWrite);
            }
            else {
                _partner.Buffer.AddRange(toWrite);
            }
            Thread.Sleep(500);
        }
    }
}
