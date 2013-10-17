using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

namespace SCFUdpCommandSender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tbIpAddress.Text = "192.168.1.30";
            tbPort.Text = "777";
            tbPayload.Text = "";

            lbCommand.Items.Add(new Command(0x00000000, "InvalidCommand", 0));
            lbCommand.Items.Add(new Command(0x00000001, "GetVersion", 0));
            lbCommand.Items.Add(new Command(0x00000003, "DeliverIceCube", 1));
            lbCommand.Items.Add(new Command(0x00000005, "SetLEDText", 5));
            lbCommand.Items.Add(new Command(0x00000007, "SetLEDControl", 5));
            lbCommand.Items.Add(new Command(0x00000009, "GetMainStatus", 0));
            lbCommand.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int port;
            if (!Int32.TryParse(tbPort.Text, out port))
            {
                MessageBox.Show("Invalid Port: " + tbPort.Text, "Error", MessageBoxButtons.OK);
                return;
            }

            IPAddress ipAddress;
            if (!IPAddress.TryParse(tbIpAddress.Text, out ipAddress))
            {
                MessageBox.Show("Invalid IP Address: " + tbIpAddress.Text, "Error", MessageBoxButtons.OK);
                return;
            }

            byte[] message = Command.ConvertFromString(tbUdpMessage.Text);

            SendUDPPacket(tbIpAddress.Text, port, message, 1);
        }

        /// <summary>
        /// Sends a sepcified number of UDP packets to a host or IP Address.
        /// </summary>
        /// <param name="hostNameOrAddress">The host name or an IP Address to which the UDP packets will be sent.</param>
        /// <param name="destinationPort">The destination port to which the UDP packets will be sent.</param>
        /// <param name="data">The data to send in the UDP packet.</param>
        /// <param name="count">The number of UDP packets to send.</param>
        public static void SendUDPPacket(string hostNameOrAddress, int destinationPort, byte[] message, int count)
        {
            // Validate the destination port number    
            if (destinationPort < 1 || destinationPort > 65535)
                throw new ArgumentOutOfRangeException("destinationPort", "Parameter destinationPort must be between 1 and 65,535.");
            // Resolve the host name to an IP Address    
            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostNameOrAddress);
            if (ipAddresses.Length == 0)
                throw new ArgumentException("Host name or address could not be resolved.", "hostNameOrAddress");
            // Use the first IP Address in the list    
            IPAddress destination = ipAddresses[0];
            IPEndPoint endPoint = new IPEndPoint(destination, destinationPort);
//            byte[] buffer = new byte[command.Length + payload.Length];
//            Array.Copy(command, buffer, command.Length);
//            Array.Copy(payload, 0, buffer, command.Length, payload.Length);
            // Send the packets    
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            for (int i = 0; i < count; i++)
                socket.SendTo(message, endPoint);
            socket.Close();
        }

        private void lbCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            Command selectedCommand = lbCommand.SelectedItem as Command;
            tbPayload.Enabled = selectedCommand.HasPayload;
            if(selectedCommand.HasPayload)
            {
                tbPayload.Text = new String('0', selectedCommand.PayloadLength*2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Command selectedCommand = lbCommand.SelectedItem as Command;
            byte[] command = selectedCommand.CommandNumber;
            byte[] payload = { };
            if (selectedCommand.HasPayload)
            {
                payload = Command.ConvertFromString(tbPayload.Text);
            }

            tbUdpMessage.Text = Command.ConvertToString(command) + Command.ConvertToString(payload);
        }
    }
}
