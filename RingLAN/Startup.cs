using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private int openForms;

        /// <summary>
        /// Performs initial setup and loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Startup_Load(object sender, EventArgs e) {
            VirtualItemsSelect.SelectedIndex = 0;
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
            PhysicalPortSelect.Items.Add("COM7");
        }

        /// <summary>
        /// Launch a virtual ring
        /// </summary>
        /// <param name="sender"/>
        /// <param name="e"/>
        private void VirtualLaunchButton_Click(object sender, EventArgs e) {
            Logger.Logging = false;
            int numClients = int.Parse(VirtualItemsSelect.Text);
            int lineQuality = (int) NoisePotential.Value;
            Logger.Log("Setting up virtual ring {0} long".With(numClients));
            List<ClientUI> clients = new List<ClientUI>();
            for (int i = 1; i <= numClients; i++) {
                InMemoryInput comms = new InMemoryInput {quality = lineQuality};
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

        /// <summary>
        /// Handle the closing of each form, so we can shut down when they're all gone
        /// </summary>
        /// <param name="sender"/>
        /// <param name="e"/>
        void client_FormClosed(object sender, FormClosedEventArgs e) {
            openForms--;

            if (openForms == 0) {
                this.Close();
            }
        }

        /// <summary>
        /// Launch a physical client and attempt to connect over a COM port
        /// </summary>
        /// <param name="sender"/>
        /// <param name="e"/>
        private void PhysicalLaunchButton_Click(object sender, EventArgs e) {
            Logger.Log("Setting up physical client on port {0}".With(PhysicalPortSelect.Text));
            try {
                ICommunication comms = new COMInput(PhysicalPortSelect.Text);
                ClientUI client = new ClientUI(comms);
                client.FormClosed += client_FormClosed;
                openForms++;
                client.Show();
                this.Hide();
            }
            catch (ArgumentException ex) {
                Logger.Log(ex.Message, "Failure");
                MessageBox.Show(ex.Message, @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex) {
                Logger.Log(ex.Message, "Failure");
                MessageBox.Show(ex.Message, @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }
    }
}
