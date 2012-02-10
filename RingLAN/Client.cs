using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    public class Client {
        //
        //Class Variables
        //
        /// <summary>
        /// The address of this client. Determines which messages are handled and where messages are sent from.
        /// Initialised to -1 because this needs to be set up when the LAN ring is turned on, not the client!
        /// </summary>
        private char _address = ' ';
        public char Address { get { return _address; }
            set {
                _comms.Send(new Message(value.ToString(), value, MessageType.Login));
            }
        }

        /// <summary>
        /// The comms object (implementing ICommunication) to be used for communication in this object
        /// </summary>
        private ICommunication _comms = null;

        //
        //Constructors
        //
        /// <summary>
        /// Sets up a new client object communicating using the specified interface
        /// </summary>
        /// <param name="comms">The interface (implementing ICommunication) that will handle sending and recieving messages</param>
        public Client(ICommunication comms) {
            _comms = comms;
        }

        //
        //Event Handlers
        //

        /// <summary>
        /// Event handler for actionable message events
        /// </summary>
        /// <param name="sender">The Client object that raised the event</param>
        /// <param name="args">The EventArgs object containing the message</param>
        public delegate void ActionableMessageRecievedHandler(object sender, MessageEventArgs args);

        /// <summary>
        /// Event handler for receiving message events
        /// </summary>
        /// <param name="sender">The object that sent the message</param>
        /// <param name="args">A MessageEventArgs object containing a high-level message to be sent</param>
        public delegate void SendMessageEventHandler(object sender, MessageEventArgs args);

        /// <summary>
        /// Actionable message event is raised when a message that a UI should handle is recieved.
        /// </summary>
        public event ActionableMessageRecievedHandler ActionableMessageRecieved;

        /// <summary>
        /// Triggers the actionable message event
        /// </summary>
        /// <param name="args">EventArgs object containing the message</param>
        private void OnActionableMessageRecieved(MessageEventArgs args) {
            if (ActionableMessageRecieved != null) {
                ActionableMessageRecieved(this, args);
            }
        }

        /// <summary>
        /// Handles sending messages to other clients from a UI
        /// </summary>
        /// <param name="sender">The UI object that requested the send</param>
        /// <param name="args">The MessageEventArgs object representing the message to send</param>
        public void SendMessage(object sender, MessageEventArgs args) {
            _comms.Send(args.Message);
        }

        public void RecieveMessage(object sender, MessageEventArgs args) {
            Message message = args.Message;
            if (message.Type == MessageType.Message) {
                OnActionableMessageRecieved(args);
            }
        }
    }
}
