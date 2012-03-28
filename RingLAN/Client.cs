using System.Collections.Generic;

namespace RingLAN {
    /// <summary>
    /// A client object performs the high level but not user-interactive messaging logic.
    /// </summary>
    public class Client {
        //
        //Class Variables
        //
        /// <summary>
        /// The address of this client. Determines which messages are handled and where messages are sent from.
        /// Initialised to -1 because this needs to be set up when the LAN ring is turned on, not the client!
        /// </summary>
        private char _address = ' ';

        /// <summary>
        /// Stores the debug mode state
        /// </summary>
        private bool _promiscuous;

        /// <summary>
        /// Stores whether this client is logged in
        /// </summary>
        private bool _loggedIn;

        /// <summary>
        /// Stores the list of known users on the ring
        /// </summary>
        private readonly List<char> _identifiedUsers = new List<char>();

        /// <summary>
        /// Gets and sets the debug mode status of this client
        /// </summary>
        public bool Debug {
            get { return _promiscuous; }
            set { _promiscuous = value; }
        }

        /// <summary>
        /// Return a list of known clients
        /// </summary>
        public List<char> Clients {
            get { return _identifiedUsers; }
        }

        /// <summary>
        /// Gets and sets the client's address.
        /// </summary>
        public char Address {
            get { return _address; }
            set {
                Communications.Send(new Message(null, value, value, MessageType.Login));
                _address = value;
            }
        }

        /// <summary>
        /// Returns whether the client is logged in
        /// </summary>
        public bool LoggedIn {
            get { return _loggedIn; }
            set {
                if (!value && _loggedIn) {
                    Communications.PassOn(new Message(null, Address, Address, MessageType.Logout));
                }
            }
        }

        /// <summary>
        /// Gets an address suitable for display
        /// </summary>
        public string DisplayAddress {
            get { return Names.GetName(Address); }
        }

        /// <summary>
        /// Gets the ICommunication object implementing communications
        /// </summary>
        public ICommunication Communications { get; private set; }

        //
        //Constructors
        //
        /// <summary>
        /// Sets up a new client object communicating using the specified interface
        /// </summary>
        /// <param name="comms">The interface (implementing ICommunication) that will handle sending and recieving messages</param>
        public Client(ICommunication comms) {
            Logger.Log("New Client starting up.");
            Communications = comms;
            comms.Parent = this;
            comms.Recieved += RecieveMessage;
            comms.Failed += FailedMessage;
        }

        //
        // Public Methods
        //

        /// <summary>
        /// Shuts down this client.
        /// </summary>
        public void Close() {
            Communications.Close();
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
            Communications.Send(args.Message);
            if (args.Message.Address != Address) {
                OnActionableMessageRecieved(args);
            }
        }

        /// <summary>
        /// Event handler for recieving a new message.
        /// Passes events up to the UI if neccesary, otherwise responds as appropriate
        /// </summary>
        /// <param name="sender">Communications object that raised the event</param>
        /// <param name="args">MessageEventArgs object containing the recieved message</param>
        public void RecieveMessage(object sender, MessageEventArgs args) {
            Message message = args.Message;
            if (_promiscuous || message.Address == Address || message.SenderAddress == Address) {
                if (message.Type == MessageType.Message) {
                    OnActionableMessageRecieved(args);
                }
            }
            if (message.Type == MessageType.Login) {
                if (message.SenderAddress == Address) {
                    if (LoggedIn) {
                        Communications.PassOn(message.Acknowledge);
                    }
                    else {
                        _loggedIn = true;
                    }
                }

                if (!_identifiedUsers.Contains(message.Address)) {
                    _identifiedUsers.Add(message.Address);
                }
                if (message.SenderAddress != Address) {
                    Communications.PassOn(new Message(null, message.Address, this.Address, MessageType.IdentResponse));
                }
                OnActionableMessageRecieved(args);
            }
            if (message.Type == MessageType.Logout) {
                if (message.Address == Address) {
                    _loggedIn = false;
                    _address = ' ';
                }
                _identifiedUsers.Remove(message.Address);
                OnActionableMessageRecieved(args);
            }
            if (message.Type == MessageType.IdentResponse) {
                if (!_identifiedUsers.Contains(message.SenderAddress)) {
                    _identifiedUsers.Add(message.SenderAddress);
                }
                OnActionableMessageRecieved(args);
            }
            if (message.Type == MessageType.Acknowledge) {
                if (!LoggedIn) {
                    _address = ' ';
                    OnActionableMessageRecieved(args);
                }
            }
        }

        /// <summary>
        /// Handles message send failure.
        /// </summary>
        /// <param name="sender">The communications object that raised the error</param>
        /// <param name="args">The message that failed</param>
        private void FailedMessage(object sender, MessageEventArgs args) {
            Message message = args.Message;
            if (message.Type == MessageType.Login) {
                if (!LoggedIn) {
                    _address = ' ';
                }
            }
            if (message.Type == MessageType.Message) {
                //If 6 attempts haven't gotten through, they probably logged out already
                _identifiedUsers.Remove(message.Address);
            }
        }
    }
}