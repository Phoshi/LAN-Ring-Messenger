using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Extensions;
using ToastNotifier;

namespace RingLAN {
    public partial class ClientUI : Form {
        /// <summary>
        /// Holds the reference to this UI's underlying client object
        /// </summary>
        private Client _client;

        private bool _loggedIn;
        private List<char> _knownClients = new List<char>();

        private Message _lastRecievedMessage;
        private DateTime _lastRecievedOn;

        private NotifierOptions notificationOptions = new NotifierOptions();

        /// <summary>
        /// Provides access to the underlying Client object
        /// </summary>
        public Client Client {
            get { return _client; }
        }

        /// <summary>
        /// Constructor for Client UI object
        /// </summary>
        /// <param name="comms">The interface to bind communications to</param>
        public ClientUI(ICommunication comms) {
            InitializeComponent();

            _client = new Client(comms);
            _client.ActionableMessageRecieved += MessageRecieved;
            SendMessage += _client.SendMessage;
        }

        //
        // Event Handlers
        //

        /// <summary>
        /// Event handler for form show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientUI_Load(object sender, EventArgs e) {
            DisplayStatusMessage("Please insert your unique login ID!");
        }

        /// <summary>
        /// Event Handler for new displayable message 
        /// </summary>
        /// <param name="sender">The Client object that raised the event</param>
        /// <param name="e">The Message Event Args object containing the message object to be displayed</param>
        void MessageRecieved(object sender, MessageEventArgs e) {
            Message message = e.Message;
            switch (message.Type) {
                case MessageType.Message:
                    DisplayRecievedMessage(message);
                    break;
                case MessageType.Login:
                case MessageType.Logout:
                    DisplayLoginMessage(message);
                    this.Invoke((Action)HandleLogin);
                    break;
                case MessageType.IdentResponse:
                    this.Invoke((Action)HandleLogin);
                    break;
                case MessageType.Acknowledge:
                    DisplayStatusMessage("Ident taken! Try again.");
                    this.Invoke((Action)(() => RecievedMessagesBox.Enabled = false));
                    break;

            }
            _lastRecievedMessage = e.Message;
            _lastRecievedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// The event to be raised when a message is requested to be sent from this client
        /// </summary>
        public event Client.SendMessageEventHandler SendMessage;
        
        /// <summary>
        /// Send a message to all listening clients
        /// </summary>
        /// <param name="message"></param>
        private void OnSendMessage(Message message) {
            if (SendMessage!=null) {
                MessageEventArgs args = new MessageEventArgs(message);
                SendMessage(this, args);
            }
        }


        private void LogOutButton_Click(object sender, EventArgs e) {
            _client.LoggedIn = false;
        }

        private void DebugModeCheck_CheckedChanged(object sender, EventArgs e) {
            _client.Debug = DebugModeCheck.Checked;
        }

        private void ClientUI_FormClosed(object sender, FormClosedEventArgs e) {
            _client.LoggedIn = false;
            _client.Close();
        }

        private void RecievedMessagesBox_KeyDown(object sender, KeyEventArgs e) {
            e.SuppressKeyPress = true;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                SendButton_Click(this, null);
                e.SuppressKeyPress = true;
            }
        }

        private void SendButton_Click(object sender, EventArgs e) {
            if (!_client.LoggedIn) {
                if (InputBox.Text.Length == 0) {
                    return;
                }
                _client.Address = InputBox.Text[0];
                DisplayStatusMessage("Attempting login with {0}".With(_client.DisplayAddress));
                RecievedMessagesBox.Enabled = true;
                InputBox.Text = "";
                return;
            }
            string messagetext = InputBox.Text;
            List<char> targets = new List<char>();
            if (RecipientSelectBox.Text == @"All") {
                targets.AddRange(_client.Clients);
            }
            else {
                char recipient = (char) 0;
                if (messagetext.Length > 3 && messagetext[1] == ':' && _client.Clients.Contains(messagetext[0])) {
                    recipient = messagetext[0];
                    messagetext = messagetext.Substring(3);
                }
                else if (RecipientSelectBox.Text.Length == 1) {
                    recipient = char.Parse(RecipientSelectBox.Text);
                }
                if (recipient != (char) 0) {
                    targets.Add(recipient);
                }
            }
            if (targets.Count == 0) {
                DisplayStatusMessage("Please select a recipient!");
                return;
            }
            while (messagetext.Length > 0) {
                foreach (char recipient in targets) {
                    Message message = new Message(messagetext, recipient, _client.Address, MessageType.Message);
                    OnSendMessage(message);
                }
                if (messagetext.Length > 10) {
                    messagetext = messagetext.Substring(10);
                }
                else {
                    break;
                }
            }
            InputBox.Text = "";
        }

        //
        // User manipulation and such
        //

        private void HandleLogin() {
            RecipientSelectBox.Items.Clear();
            RecipientSelectBox.Items.Add("All");
            foreach (char client in _client.Clients) {
                RecipientSelectBox.Items.Add(client);
            }
        }

        //
        // Private UI Methods
        //

        /// <summary>
        /// Appends text to the primary display
        /// </summary>
        /// <param name="text">The text to add</param>
        private void AddText(string text) {
            RecievedMessagesBox.Invoke((Action)(() => RecievedMessagesBox.AppendText(text + "\r\n")));
        }

        private void TrimMessageBox() {
            RecievedMessagesBox.Invoke((Action) (() => RecievedMessagesBox.Text = RecievedMessagesBox.Text.TrimEnd(new[] {'\r', '\n'})));
        }

        /// <summary>
        /// Show a status-level message in the UI
        /// </summary>
        /// <param name="message">The string containing the message</param>
        private void DisplayStatusMessage(String message) {
            AddText(" -!- " + message);
        }

        /// <summary>
        /// Show a recieved message in the most appropriate context-sensitive way
        /// </summary>
        /// <param name="message">Message object to show</param>
        private void DisplayRecievedMessage(Message message) {
            if (message == _lastRecievedMessage) {
                DisplayStatusMessage("Potential Packet Duplication detected");
            }
            else if (_lastRecievedMessage.SenderAddress == message.SenderAddress && _lastRecievedMessage.Type == MessageType.Message &&
                (DateTime.UtcNow - _lastRecievedOn).TotalSeconds < 5) {
                TrimMessageBox();
                AddText(message.Payload);
                return;
            }
            AddText(" -{0} -> {1}- {2}".With(message.Sender, message.Recipient, message.Payload));
            if (message.SenderAddress != _client.Address) {
                this.Invoke((Action) (() => {
                                          Notifier notifier = new Notifier(notificationOptions, "New Message", message.Payload,
                                                                           "{0} -> {1}".With(message.Sender, message.Recipient));
                                          notifier.parentForm = this;
                                          notifier.Show();
                                      }));
            }
        }

        private void DisplayLoginMessage(Message message) {
            if (message.SenderAddress != _client.Address && _knownClients.Contains(message.SenderAddress)) {
                DisplayStatusMessage("Login collision for {0} detected!".With(message.Sender));
                return;
            }
            if (message.Type == MessageType.Login) {
                if (message.Address == _client.Address) {
                    if (!_loggedIn) {
                        this.Invoke((Action) (() => this.Text = "({0}) {1}".With(_client.Address, this.Text)));
                        this.Invoke((Action) (() => SendButton.Text = @"Send"));
                        DisplayStatusMessage("Login Successful.");
                        _loggedIn = true;
                    }
                    else {
                        DisplayStatusMessage("Login collision detected!");
                    }
                    return;
                }
                _knownClients.Add(message.SenderAddress);
                AddText(" > {0} has signed in!".With(message.Sender));
            }
            else {
                if (!_client.LoggedIn) {
                    this.Invoke((Action)(() => this.Text = "Ring LAN UI".With(_client.Address, this.Text)));
                    this.Invoke((Action) (() => RecievedMessagesBox.Enabled = false));
                    this.Invoke((Action)(() => SendButton.Text = @"Login"));
                    DisplayStatusMessage("Logout Successful.");
                    _loggedIn = false;
                    return;
                }
                _knownClients.Remove(message.SenderAddress);
                AddText(" > {0} has signed out!".With(message.Sender));
            }
        }

    }
}
