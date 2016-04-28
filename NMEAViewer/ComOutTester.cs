using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMEAViewer
{
    public partial class ComOutTester : Form
    {
        public static ComOutTester sm_Tester;
        Timer update;

        public ComOutTester()
        {
            sm_Tester = this;
            InitializeComponent();
            update = new Timer();
            update.Interval = 500;
            update.Start();
            update.Tick += Update_Tick;
            this.FormClosed += ComOutTester_FormClosed;
        }

        private void Update_Tick(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.WriteLine("$GPGLL,3751.65,S,14507.36,E*77");
            }
        }

        private void ComOutTester_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
        }

        private void SetDialogState()
        {
            if (serialPort1.IsOpen)
            {
                groupBox1.Enabled = false;
                button_ComCreateOrDestroy.Text = "Close";
            }
            else
            {
                groupBox1.Enabled = true;
                button_ComCreateOrDestroy.Text = "Create";
            }
        }

        private void button_ComCreateOrDestroy_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            else
            {
                serialPort1.DataBits = Convert.ToInt32(DataBitsTextBox.Text);
                serialPort1.PortName = PortNameTextBox.Text;
                serialPort1.Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), ParityChoice.Text);
                serialPort1.StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), StopBits.Text);
                serialPort1.BaudRate = Convert.ToInt32(BaudRateTextBox.Text);
                
                serialPort1.Open();
            }

            SetDialogState();
        }
    }
}
