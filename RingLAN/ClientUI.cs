using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Extensions;
using ToastNotifier;

namespace RingLAN {
    public partial class ClientUI : Form {
        /// <summary>
        /// Holds the reference to this UI's underlying client object
        /// </summary>
        private readonly Client _client;

        /// <summary>
        /// Whether the UI thinks it's logged in or not. This does not represent the underlying Client object's login state, but is rather used for UI logic.
        /// </summary>
        private bool _loggedIn;

        /// <summary>
        /// List of logged in clients. This is not a straight copy of the underlying Client object's listing, but is used for UI logic.
        /// </summary>
        private readonly List<char> _knownClients = new List<char>();

        /// <summary>
        /// The last message object that was handled. 
        /// </summary>
        private Message _lastRecievedMessage;
        /// <summary>
        /// The time the last message was recieved.
        /// </summary>
        private DateTime _lastRecievedOn;

        /// <summary>
        /// The options for the popup notifications
        /// </summary>
        private readonly NotifierOptions notificationOptions = new NotifierOptions();

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
            _client.Communications.Failed += MessageSendFailed;
            _client.Communications.CharacterRecieved += Communications_CharacterRecieved;
            SendMessage += _client.SendMessage;
        }



        ////////////////////
        // Event Handlers //
        ////////////////////

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
                    DisplayStatusMessage("{0} is online.".With(message.Sender));
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
        /// Event handler for new character reciveded
        /// </summary>
        /// <param name="sender">The communications object that raised the event</param>
        /// <param name="character">The recieved character</param>
        void Communications_CharacterRecieved(object sender, char character) {
            if (DebugModeCheck.Checked) {
                if (character == 0) {
                    character = ' ';
                }
                RecievedMessagesBox.Invoke((Action)(() => RecievedMessagesBox.Text += character));
            }
        }

        /// <summary>
        /// Event handler for failed sends
        /// </summary>
        /// <param name="sender">The communications interface that raised the failure</param>
        /// <param name="args">The MessageEventArgs object containing the failed message object</param>
        void MessageSendFailed(object sender, MessageEventArgs args) {
            Message message = args.Message;
            switch (message.Type) {
                case MessageType.Login:
                    DisplayStatusMessage("Either ident taken or network failure! Try again!");
                    this.Invoke((Action)(() => RecievedMessagesBox.Enabled = false));
                    break;
                case MessageType.Message:
                    DisplayStatusMessage("Message '{0}' to {1} send failed!".With(message.Payload, message.Recipient));
                    DisplayStatusMessage("Assuming {0} is offline.".With(message.Recipient));
                    this.Invoke((Action) HandleLogin);
                    break;

            }
        }

        /// <summary>
        /// The event to be raised when a message is requested to be sent from this client
        /// </summary>
        public event Client.SendMessageEventHandler SendMessage;
        
        /// <summary>
        /// Send a message to all listening clients
        /// </summary>
        /// <param name="message">The message object to be sent.</param>
        private void OnSendMessage(Message message) {
            if (SendMessage!=null) {
                MessageEventArgs args = new MessageEventArgs(message);
                SendMessage(this, args);
            }
        }

        /// <summary>
        /// Event handler for the log out button.
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void LogOutButton_Click(object sender, EventArgs e) {
            _client.LoggedIn = false;
        }

        /// <summary>
        /// Event handler for the debug check.
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void DebugModeCheck_CheckedChanged(object sender, EventArgs e) {
            _client.Debug = DebugModeCheck.Checked;
        }

        /// <summary>
        /// Event handler for form closed events
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void ClientUI_FormClosed(object sender, FormClosedEventArgs e) {
            _client.Close();
        }

        /// <summary>
        /// Event handler for the kick attempt button
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void AttemptKickButton_Click(object sender, EventArgs e) {
            char target = RecipientSelectBox.Text[0];
            _client.Communications.PassOn(new Message(null, target, target, MessageType.Logout));
        }

        /// <summary>
        /// Event handler for the send raw packet button.
        /// Sends the contents of the textbox as a stream of characters.
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void BringDownTheSkyButton_Click(object sender, EventArgs e) {
            byte[] buffer = new byte[InputBox.Text.Length];
            int position = 0;
            foreach (char c in InputBox.Text) {
                buffer[position] = (byte) c;
                position++;
            }
            if (buffer.Length >= 14) {
                buffer[14] = MessageChecker.GetChecksum(new Message(buffer));
            }
            ((COMInput)_client.Communications).putChars(buffer);
        }

        /// <summary>
        /// Event handler for textbox keydowns.
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void RecievedMessagesBox_KeyDown(object sender, KeyEventArgs e) {
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Event handler for input box keydowns to provide shortcuts
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void InputBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                SendButton_Click(this, null);
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Event handler for the send button, handles sending logic
        /// </summary>
        /// <param name="sender" />
        /// <param name="e" />
        private void SendButton_Click(object sender, EventArgs e) { //TODO: Refactor into seperate methods.
            if (!_client.LoggedIn) {
                if (InputBox.Text.Length == 0) {
                    return;
                }
                char loginID = InputBox.Text.ToUpper()[0];
                if (!char.IsUpper(loginID)){
                    DisplayStatusMessage("Please insert a letter!");
                    return;
                }
                _client.Address = loginID;
                DisplayStatusMessage("Attempting login with {0}".With(_client.DisplayAddress));
                RecievedMessagesBox.Enabled = true;
                InputBox.Text = "";
                return;
            }
            string messagetext = InputBox.Text;
            char recipient = (char) 0;
            if (messagetext.Length > 3 && messagetext[1] == ':' && _client.Clients.Contains(messagetext.ToUpper()[0])) {
                recipient = messagetext[0];
                messagetext = messagetext.Substring(3);
            }
            else if (RecipientSelectBox.Text.Length == 1) {
                recipient = char.Parse(RecipientSelectBox.Text);
            }
            if (recipient == 0) {
                DisplayStatusMessage("Please select a recipient!");
                return;
            }
            while (messagetext.Length > 0) {
                Message message = new Message(messagetext, recipient, _client.Address, MessageType.Message);
                OnSendMessage(message);
                if (messagetext.Length > 10) {
                    messagetext = messagetext.Substring(10);
                }
                else {
                    break;
                }
            }
            InputBox.Text = "";
        }

        ////////////////////////////////
        // User manipulation and such //
        ////////////////////////////////

        /// <summary>
        /// Rebuilds the logged in users list based on the underlying Client object's state.
        /// </summary>
        private void HandleLogin() {
            RecipientSelectBox.Items.Clear();
            foreach (char client in _client.Clients) {
                RecipientSelectBox.Items.Add(client);
            }
            if (RecipientSelectBox.Items.Count > 0){
                RecipientSelectBox.SelectedIndex = 0;
            }
        }

        ////////////////////////////
        // UI interaction methods //
        ////////////////////////////

        /// <summary>
        /// Appends text to the primary display
        /// </summary>
        /// <param name="text">The text to add</param>
        private void AddText(string text) {
            if (!DebugModeCheck.Checked) {
                RecievedMessagesBox.Invoke((Action) (() => RecievedMessagesBox.AppendText(text + "\r\n")));
            }
        }

        /// <summary>
        /// Trims the trailing whitespace from the textbox. Useful for continuing previous lines.
        /// </summary>
        private void TrimMessageBox() {
            if (!DebugModeCheck.Checked) {
                RecievedMessagesBox.Invoke((Action) (() => RecievedMessagesBox.Text = RecievedMessagesBox.Text.TrimEnd(new[] {'\r', '\n'})));
            }
        }

        /// <summary>
        /// Show a status-level message in the UI
        /// </summary>
        /// <param name="message">The string containing the message</param>
        private void DisplayStatusMessage(String message) {
            AddText(" <!> " + message);
        }

        /// <summary>
        /// Show a recieved message in the most appropriate context-sensitive way
        /// </summary>
        /// <param name="message">Message object to show</param>
        private void DisplayRecievedMessage(Message message) {
            if (message == _lastRecievedMessage) {
                DisplayStatusMessage(message.SenderAddress == _client.Address
                                         ? "Delivery failed, resending..."
                                         : "Potential Packet Duplication detected");
            }
            else if (_lastRecievedMessage.SenderAddress == message.SenderAddress && _lastRecievedMessage.Type == MessageType.Message &&
                (DateTime.UtcNow - _lastRecievedOn).TotalSeconds < 5) {
                TrimMessageBox();
                AddText(message.Payload);
                if (Notifier.TopMostNotification != null) {
                    this.Invoke((Action) (() => {
                                              Notifier.TopMostNotification.description += message.Payload;
                                              Notifier.TopMostNotification.Refresh();
                                          }));
                }
                return;
            }
            AddText(" <{0} -> {1}> {2}".With(message.Sender, message.Recipient, message.Payload));
            if (message.SenderAddress != _client.Address) {
                this.Invoke((Action) (() => {
                                          Image horse = Names.GetImage(message.SenderAddress);
                                          Notifier notifier = new Notifier(notificationOptions, "New Message from {0}".With(message.Sender), message.Payload,
                                                                           "{0} -> {1}".With(message.Sender, message.Recipient), horse)
                                                              {parentForm = this};
                                          notifier.Show();
                                      }));
            }
        }

        /// <summary>
        /// Display's a login message for the login packet, if appropriate.
        /// </summary>
        /// <param name="message">The packet to handle.</param>
        private void DisplayLoginMessage(Message message) {
            if (message.Type == MessageType.Login && message.SenderAddress != _client.Address && _knownClients.Contains(message.SenderAddress)) {
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
                if (!_knownClients.Contains(message.SenderAddress)){
                    _knownClients.Add(message.SenderAddress);
                    AddText(" > {0} has signed in!".With(message.Sender));
                }
                else{
                    AddText(" > {0} has signed in... again!".With(message.Sender));
                }
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
                Names.DelUser(message.SenderAddress);
            }
        }
    }
}
