using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    /// <summary>
    /// Implements communication over a COM port
    /// </summary>
    class COMInput : ICommunication {
        public void Send(Message message) {
            throw new NotImplementedException();
        }

        public event MessageRecievedHandler Recieved;
        private void OnMessageRecieved(MessageEventArgs message) {
            if (Recieved != null) {
                Recieved(this, message);
            }
        }
        private void OnMessageRecieved(Message message) {
            MessageEventArgs args = new MessageEventArgs(message);
            OnMessageRecieved(args);
        }

        /// <summary>
        /// Main loop that recieves new messages and raises events where appropriate
        /// </summary>
        private void messageLoop() {
            LowLevelMessage newMessage = getData();
            if (MessageChecker.Check(newMessage)) {
                Message message = newMessage.ToMessage();
                OnMessageRecieved(message);
            }
        }

        /// <summary>
        /// Recieves raw data over the COM port and attempts to construct a low level message object out of it
        /// </summary>
        /// <returns></returns>
        private LowLevelMessage getData() {
            throw new NotImplementedException();
        }
    }
}
