using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Extensions;

namespace RingLAN {
    public partial class ClientUI : Form {
        /// <summary>
        /// Holds the reference to this UI's underlying client object
        /// </summary>
        private Client _client = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClientUI() {
            InitializeComponent();

            InMemoryInput input = new InMemoryInput("COM1");
            _client = new Client(input);
            _client.ActionableMessageRecieved += MessageRecieved;
            DisplayStatusMessage("Please insert your logon ID!");
        }

        //
        // Event Handlers
        //

        /// <summary>
        /// Event Handler for new displayable message 
        /// </summary>
        /// <param name="sender">The Client object that raised the event</param>
        /// <param name="e">The Message Event Args object containing the message object to be displayed</param>
        void MessageRecieved(object sender, MessageEventArgs e) {
            RecievedMessagesBox.AppendText(e.Message.ToString() + "\n");
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

        private void SendButton_Click(object sender, EventArgs e) {
            if (_client.Address == ' ') {
                _client.Address = char.Parse(InputBox.Text);
                return;
            }
            string messagetext = InputBox.Text;
            char recipient = char.Parse(RecipientSelectBox.Text);
            Message message = new Message(messagetext, recipient, MessageType.Message);
            OnSendMessage(message);
        }

        //
        // Private UI Methods
        //

        /// <summary>
        /// Show a status-level message in the UI
        /// </summary>
        /// <param name="message">The string containing the message</param>
        private void DisplayStatusMessage(String message) {
            RecievedMessagesBox.AppendText(" -!- " + message + "\n");
        }

        private void DisplayRecievedMessage(Message message) {
            RecievedMessagesBox.AppendText(" -{0}- {1}".With(message.Sender, message.Payload));
        }
    }
}
