using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RingLAN {
    class InMemoryInput : COMInput {
        private List<byte> _buffer = new List<byte>();

        public InMemoryInput(string dummyName) {
            
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
            foreach (byte b in toWrite) {
                Console.Write((char)b);
            }
            _buffer.AddRange(toWrite);
        }
    }
}
