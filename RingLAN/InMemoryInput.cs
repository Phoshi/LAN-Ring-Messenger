using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Extensions;

namespace RingLAN {
    class InMemoryInput : COMInput {
        private List<byte> _buffer = new List<byte>();
        private const int quality = 9999;

        public List<byte> Buffer {
            get { return _buffer; }
        }

        private InMemoryInput _partner;

        public InMemoryInput Partner {
            get { return _partner; }
            set { _partner = value; }
        }

        public InMemoryInput(string dummyName = "") {
        }

        public InMemoryInput(InMemoryInput partner) {
            _partner = partner;
        }

        public override byte getChar() {
            while (_buffer.Count == 0) {
                Thread.Sleep(100);
            }
            byte firstByte = _buffer[0];
            _buffer.RemoveAt(0);
            return firstByte;
        }

        public override void putChars(byte[] toWrite) {
            Random rng = new Random();
            int failure1 = rng.Next(quality);
            int failure2 = rng.Next(quality);

            if (failure1 == 0) { //Failure!
                Logger.Log("Corrupting packet {0}".With(new Message(toWrite).ToString()), "Failure");
                int position = rng.Next(toWrite.Length);
                toWrite[position] = (byte) (~toWrite[position]); 
            }

            if (failure2 == 0) { //Critial Failure!
                Logger.Log("Deleting Packet {0}".With(new Message(toWrite).ToString()), "Failure");
                return;
            }
            Logger.Log("Sending Packet {0}".With(new Message(toWrite).ToString()), "In Memory Input");
            if (_partner == null) {
                _buffer.AddRange(toWrite);
            }
            else {
                _partner.Buffer.AddRange(toWrite);
            }
        }
    }
}
