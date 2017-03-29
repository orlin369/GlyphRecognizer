/*

Copyright (c) [2016] [Orlin Dimitrov]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Windows.Forms;

namespace DiO_CS_GliphRecognizer.SettingsForms
{
    public partial class SettingsForm : Form
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Settings Form

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.LoadFields();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveFields();
        }

        #endregion

        #region Private Methods

        private void LoadFields()
        {
            this.tbBrokerDomain.Text = Properties.Settings.Default.BrokerHost;
            this.tbBrokerPort.Text = Properties.Settings.Default.BrokerPort.ToString();
            this.tbInputTopic.Text = Properties.Settings.Default.MqttInputTopic;
            this.tbOutputTopic.Text = Properties.Settings.Default.MqttOutputTopic;
            this.tbImageTopic.Text = Properties.Settings.Default.MqttImageTopic;
        }

        private void SaveFields()
        {
            try
            {
                int borkerPort;

                // Validate baud rate.
                if (int.TryParse(this.tbBrokerPort.Text.Trim(), out borkerPort))
                {
                    if (borkerPort < 0 || borkerPort > 65535)
                    {
                        MessageBox.Show("Invalid Broker port. [0 - 65535]", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    Properties.Settings.Default.BrokerPort = borkerPort;
                }
                else
                {
                    MessageBox.Show("Invalid Broker port.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!string.IsNullOrEmpty(this.tbBrokerDomain.Text))
                {
                    Properties.Settings.Default.BrokerHost = this.tbBrokerDomain.Text;
                }

                if (!string.IsNullOrEmpty(this.tbInputTopic.Text))
                {
                    Properties.Settings.Default.MqttInputTopic = this.tbInputTopic.Text;
                }

                if (!string.IsNullOrEmpty(this.tbOutputTopic.Text))
                {
                    Properties.Settings.Default.MqttOutputTopic = this.tbOutputTopic.Text;
                }

                if (!string.IsNullOrEmpty(this.tbImageTopic.Text))
                {
                    Properties.Settings.Default.MqttImageTopic = this.tbImageTopic.Text;
                }

                // Save settings.
                Properties.Settings.Default.Save();
            }
            catch (Exception err)
            {
                MessageBox.Show(String.Format("Message: {0}\r\nSource: {1}", err.Message, err.Source), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #endregion

    }
}
