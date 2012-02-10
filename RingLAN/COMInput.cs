using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace RingLAN {
    /// <summary>
    /// Implements communication over a COM port
    /// </summary>
    class COMInput : ICommunication {
        //
        //Private Class Variables
        //
        private SerialPort _port;

        //
        //Constructors
        //

        /// <summary>
        /// Initialises the COMInput and readies it to handle data
        /// </summary>
        /// <param name="port">The port string to attempt to bind to</param>
        public COMInput(string port) {
            _port = new SerialPort(port,    //Port Name
                9600,                       //Baud Rate
                Parity.Even,                //Parity
                7,                          //Data bits
                StopBits.One);              //Stop bits
        }

        /// <summary>
        /// A default constructor that creates the object but does not ready it to send or recieve data.
        /// </summary>
        public COMInput() {
        }

        //
        //Public Methods and Events
        //
        /// <summary>
        /// Sends a message object along the COM port.
        /// </summary>
        /// <param name="message"></param>
        public void Send(Message message) {
            byte[] byteArray = message.ToByteArray();
            putChars(byteArray);
        }

        /// <summary>
        /// The Message Recieved event that will raise whenever a valid, actionable message is recieved.
        /// </summary>
        public event MessageRecievedHandler Recieved;

        //
        //Private Methods and Events
        //
        /// <summary>
        /// Raises a Message Recieved event
        /// </summary>
        /// <param name="message">The MessageEventArgs object wrapping the Message object to be sent</param>
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
            Message newMessage = getData();
            if (MessageChecker.Check(newMessage)) {
                OnMessageRecieved(newMessage);
            }
        }

        /// <summary>
        /// Recieves raw data over the COM port and attempts to construct a low level message object out of it
        /// </summary>
        /// <returns></returns>
        private Message getData() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Grab a single character over the COM port
        /// </summary>
        /// <returns>A byte representing the current character</returns>
        public virtual byte getChar() {
            int readChar = _port.ReadChar();
            return (byte) readChar;
        }

        /// <summary>
        /// Writes out a series of bytes to the COM port
        /// </summary>
        /// <param name="toWrite">A byte array</param>
        public virtual void putChars(byte[] toWrite) {
            _port.Write(toWrite, 0, toWrite.Length);
        }
    }
}
