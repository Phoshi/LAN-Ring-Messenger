﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Extensions;

namespace RingLAN {
    /// <summary>
    /// A form to allow configuration before the client starts up.
    /// </summary>
    public partial class Startup : Form {
        public Startup() {
            InitializeComponent();
        }

        private int openForms = 0;

        /// <summary>
        /// Performs initial setup and loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Startup_Load(object sender, EventArgs e) {
            VirtualItemsSelect.Text = VirtualItemsSelect.Text = "2";
            PopulatePhysicalPorts();
        }


        /// <summary>
        /// Populates the physical port list with all the serial ports on the machine
        /// </summary>
        private void PopulatePhysicalPorts() {
            foreach (string portName in SerialPort.GetPortNames()) {
                PhysicalPortSelect.Items.Add(portName);
                PhysicalPortSelect.SelectedIndex = 0;
            }
        }

        private void VirtualLaunchButton_Click(object sender, EventArgs e) {
            int numClients = int.Parse(VirtualItemsSelect.Text);
            Logger.Log("Setting up virtual ring {0} long".With(numClients));
            List<ClientUI> clients = new List<ClientUI>();
            for (int i = 1; i <= numClients; i++) {
                InMemoryInput comms = new InMemoryInput();
                ClientUI client = new ClientUI(comms);
                client.FormClosed += client_FormClosed;
                openForms++;
                clients.Add(client);
            }
            ((InMemoryInput) clients.Last().Client.Communications).Partner = ((InMemoryInput) clients.First().Client.Communications);
            for (int i = 0;  i < clients.Count - 1; i++) {
                ClientUI client = clients[i];
                ClientUI nextClient = clients[i + 1];
                ((InMemoryInput) client.Client.Communications).Partner = ((InMemoryInput) nextClient.Client.Communications);
            }

            clients.ForEach(client => client.Show());
            this.Hide();
        }

        void client_FormClosed(object sender, FormClosedEventArgs e) {
            openForms--;

            if (openForms == 0) {
                this.Close();
            }
        }

        private void PhysicalLaunchButton_Click(object sender, EventArgs e) {
            ICommunication comms = new COMInput(PhysicalPortSelect.Text);
            Logger.Log("Setting up physical client on port {0}".With(PhysicalPortSelect.Text));
            ClientUI client = new ClientUI(comms);
            client.Show();
        }
    }
}
