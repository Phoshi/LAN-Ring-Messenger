using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RingLAN {
    public partial class ClientUI : Form {
        private Client _client = null;
        public ClientUI() {
            InitializeComponent();

            _client = new Client();
            _client.ActionableMessageRecieved += MessageRecieved;
        }

        void MessageRecieved(object sender, MessageEventArgs e) {
            RecievedMessagesBox.AppendText(e.Message.ToString() + "\n");
        }

        public event Client.SendMessageEventHandler SendMessage;
        
        private void OnSendMessage(Message message) {
            if (SendMessage!=null) {
                MessageEventArgs args = new MessageEventArgs(message);
                SendMessage(this, args);
            }
        }

        private void SendButton_Click(object sender, EventArgs e) {
            string messagetext = InputBox.Text;
            int recipient = int.Parse(RecipientSelectBox.Text);
            Message message = new Message(messagetext, recipient, MessageType.Message);
            OnSendMessage(message);
        }
    }
}
