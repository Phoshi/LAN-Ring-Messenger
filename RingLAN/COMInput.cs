﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using Extensions;

namespace RingLAN {
    /// <summary>
    /// Implements communication over a COM port
    /// </summary>
    class COMInput : ICommunication {

        //
        //Private Class Variables
        //

        private SerialPort _port;
        private Client _parent;
        private List<Pending> _pending = new List<Pending>();
        private bool _closed;
        private Thread readThread, writeThread;

        /// <summary>
        /// Gets and sets the client object tied to this COMInput
        /// </summary>
        public Client Parent {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Returns whether the port is closed.
        /// </summary>
        public bool Closed {
            get { return _closed; }
        }

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
        /// Dummy constructor to allow subclasses to override behaviour
        /// </summary>
        public COMInput() {
            this.Recieved += RecieveMessage;

            readThread = new Thread(messageLoop);
            readThread.Start();

            writeThread = new Thread(sendLoop);
            writeThread.Start();
        }

        //
        //Public Methods and Events
        //

        /// <summary>
        /// Grab a single character over the COM port
        /// </summary>
        /// <returns>A byte representing the current character</returns>
        public virtual byte getChar() {
            int readChar = _port.ReadChar();
            return (byte)readChar;
        }

        /// <summary>
        /// Writes out a series of bytes to the COM port
        /// </summary>
        /// <param name="toWrite">A byte array</param>
        public virtual void putChars(byte[] toWrite) {
            _port.Write(toWrite, 0, toWrite.Length);
        }

        /// <summary>
        /// Sends a message object along the COM port.
        /// </summary>
        /// <param name="message"></param>
        public void Send(Message message) {
            Logger.Log("Sending message '{0}' to {1}".With(message.ToString(), message.Recipient), _parent.DisplayAddress);
            _pending.Add(new Pending(message));
        }

        /// <summary>
        /// Sends a message along to the next client without any additional processing
        /// Will not attempt to retry.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void PassOn(Message message) {
            Logger.Log("Passing on message '{0}' to {1}".With(message.ToString(), message.Recipient), _parent.DisplayAddress);
            putChars(message.ToByteArray());
        }

        /// <summary>
        /// Shuts the COM port and performs cleanup
        /// </summary>
        public void Close() {
            if (_port != null) {
                _port.Close();
            }
            _closed = true;
            readThread.Abort();
            writeThread.Abort();
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
            while (!_closed) {
                Message newMessage = getData();
                if (newMessage == null) { //Packet was corrupt, should have already requested a resend.
                    continue;
                }
                if (!_parent.LoggedIn && _parent.Address != newMessage.Address) {
                    PassOn(newMessage);
                    continue;
                }
                if (newMessage.Address != _parent.Address) {
                    //If it's not for me, send it on
                    PassOn(newMessage);
                }
                OnMessageRecieved(newMessage);
            }
        }

        /// <summary>
        /// Main loop that sends and resends messages
        /// </summary>
        private void sendLoop() {
            while (!_closed) {
                IEnumerable<Pending> messages;
                lock (this) {
                    _pending = _pending.Where(item => item.SendCount <= 5).ToList();
                    messages =
                        _pending.Where(item => item.LastSend == DateTime.MinValue || item.LastSend < DateTime.UtcNow.AddSeconds(-2)).OrderByDescending(
                            item => item.LastSend).Unique(
                                item => item.Message.Sender);
                }

                foreach (Pending message in messages) {
                    message.SendCount++;
                    message.LastSend = DateTime.UtcNow;
                    byte[] buffer = message.Message.ToByteArray();
                    putChars(buffer);
                }
                Thread.Sleep(400);
            }
        }

        /// <summary>
        /// Recieves raw data over the COM port and attempts to construct a low level message object out of it
        /// </summary>
        /// <returns></returns>
        private Message getData() {
            byte[] buffer = new byte[16];
            int recievedBytes = 0;
            while (recievedBytes <= 15) {
                byte character = getChar();
                if (character == '{') {
                    recievedBytes = 0;
                }
                buffer[recievedBytes] = character;
                recievedBytes++;
            }
            Message newMessage = new Message(buffer);
            if (!MessageChecker.Check(newMessage)) {
                PassOn(newMessage.NotAcknowledgable);
                return null;
            }
            return newMessage;
        }

        //
        // Event Handlers
        //

        private void RecieveMessage(object sender, MessageEventArgs args) {
            Message message = args.Message;

            switch (message.Type) {
                case MessageType.Login:
                    if (message.Address == _parent.Address && message.SenderAddress == _parent.Address) {
                        Logger.Log("Got my login packet back", _parent.DisplayAddress);
                        lock (this) {
                            _pending = _pending.Where(item => item.Message != message).ToList();
                        }
                    }
                    break;
                case MessageType.Message:
                    if (message.Address == _parent.Address) {
                        Logger.Log("Responding to '{0}' with Ack".With(message.ToString()), _parent.DisplayAddress);
                        PassOn(message.Acknowledge);
                    }
                    break;
                case MessageType.Acknowledge:
                    if (message.Address == _parent.Address) {
                        char address = message.SenderAddress;
                        Logger.Log("Received Ack from {0}".With(address), _parent.DisplayAddress);
                        lock (this) {
                            try {
                                Message toRemove = _pending.NextTo(address).Message;
                                Logger.Log("Matched this to message '{0}'".With(toRemove.ToString()), _parent.DisplayAddress);
                                _pending = _pending.Where(item => item.Message != toRemove).ToList();
                            }
                            catch (NullReferenceException) {
                                Logger.Log("Recieved Ack for unknown packet from {0}!".With(message.SenderAddress), _parent.DisplayAddress);
                            }
                        }
                    }
                    break;
                case MessageType.NotAcknowledgable:
                    if (message.Address == _parent.Address) {
                        Logger.Log("Recieved Nack from {0}".With(message.SenderAddress), _parent.DisplayAddress);
                        Pending pending = _pending.NextTo(message.SenderAddress);
                        if (pending != null) {
                            Logger.Log("Matched this to '{0}'".With(pending.Message.ToString()), _parent.DisplayAddress);
                            pending.LastSend = DateTime.MinValue;
                        }
                    }
                    break;

            }
        }
    }
}
