namespace DiO_CS_GliphRecognizer.SettingsForms
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tpGlyphDatabase = new System.Windows.Forms.TabPage();
            this.bDefaultDatabasePath = new System.Windows.Forms.TextBox();
            this.btnOpenDefaultDatabase = new System.Windows.Forms.Button();
            this.tpMQTT = new System.Windows.Forms.TabPage();
            this.gbMQTT = new System.Windows.Forms.GroupBox();
            this.tbImageTopic = new System.Windows.Forms.TextBox();
            this.lblImageTopic = new System.Windows.Forms.Label();
            this.tbBrokerPort = new System.Windows.Forms.TextBox();
            this.lblBrokerPort = new System.Windows.Forms.Label();
            this.tbBrokerDomain = new System.Windows.Forms.TextBox();
            this.lblBrokerDomain = new System.Windows.Forms.Label();
            this.tbInputTopic = new System.Windows.Forms.TextBox();
            this.tbOutputTopic = new System.Windows.Forms.TextBox();
            this.lblOutputTopic = new System.Windows.Forms.Label();
            this.lblInputTopic = new System.Windows.Forms.Label();
            this.tbImageWidth = new System.Windows.Forms.TextBox();
            this.tbImageHeight = new System.Windows.Forms.TextBox();
            this.lblImageSize = new System.Windows.Forms.Label();
            this.tcSettings.SuspendLayout();
            this.tpGlyphDatabase.SuspendLayout();
            this.tpMQTT.SuspendLayout();
            this.gbMQTT.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcSettings
            // 
            this.tcSettings.Controls.Add(this.tpGlyphDatabase);
            this.tcSettings.Controls.Add(this.tpMQTT);
            this.tcSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSettings.Location = new System.Drawing.Point(0, 0);
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(965, 534);
            this.tcSettings.TabIndex = 3;
            // 
            // tpGlyphDatabase
            // 
            this.tpGlyphDatabase.BackColor = System.Drawing.SystemColors.Control;
            this.tpGlyphDatabase.Controls.Add(this.bDefaultDatabasePath);
            this.tpGlyphDatabase.Controls.Add(this.btnOpenDefaultDatabase);
            this.tpGlyphDatabase.Location = new System.Drawing.Point(4, 25);
            this.tpGlyphDatabase.Name = "tpGlyphDatabase";
            this.tpGlyphDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tpGlyphDatabase.Size = new System.Drawing.Size(957, 505);
            this.tpGlyphDatabase.TabIndex = 0;
            this.tpGlyphDatabase.Text = "Glyph Database";
            // 
            // bDefaultDatabasePath
            // 
            this.bDefaultDatabasePath.Location = new System.Drawing.Point(8, 16);
            this.bDefaultDatabasePath.Name = "bDefaultDatabasePath";
            this.bDefaultDatabasePath.Size = new System.Drawing.Size(303, 22);
            this.bDefaultDatabasePath.TabIndex = 9;
            // 
            // btnOpenDefaultDatabase
            // 
            this.btnOpenDefaultDatabase.Location = new System.Drawing.Point(317, 6);
            this.btnOpenDefaultDatabase.Name = "btnOpenDefaultDatabase";
            this.btnOpenDefaultDatabase.Size = new System.Drawing.Size(113, 40);
            this.btnOpenDefaultDatabase.TabIndex = 0;
            this.btnOpenDefaultDatabase.Text = "Open";
            this.btnOpenDefaultDatabase.UseVisualStyleBackColor = true;
            // 
            // tpMQTT
            // 
            this.tpMQTT.BackColor = System.Drawing.SystemColors.Control;
            this.tpMQTT.Controls.Add(this.gbMQTT);
            this.tpMQTT.Location = new System.Drawing.Point(4, 25);
            this.tpMQTT.Name = "tpMQTT";
            this.tpMQTT.Padding = new System.Windows.Forms.Padding(3);
            this.tpMQTT.Size = new System.Drawing.Size(957, 505);
            this.tpMQTT.TabIndex = 1;
            this.tpMQTT.Text = "MQTT Settings";
            // 
            // gbMQTT
            // 
            this.gbMQTT.Controls.Add(this.lblImageSize);
            this.gbMQTT.Controls.Add(this.tbImageHeight);
            this.gbMQTT.Controls.Add(this.tbImageWidth);
            this.gbMQTT.Controls.Add(this.tbImageTopic);
            this.gbMQTT.Controls.Add(this.lblImageTopic);
            this.gbMQTT.Controls.Add(this.tbBrokerPort);
            this.gbMQTT.Controls.Add(this.lblBrokerPort);
            this.gbMQTT.Controls.Add(this.tbBrokerDomain);
            this.gbMQTT.Controls.Add(this.lblBrokerDomain);
            this.gbMQTT.Controls.Add(this.tbInputTopic);
            this.gbMQTT.Controls.Add(this.tbOutputTopic);
            this.gbMQTT.Controls.Add(this.lblOutputTopic);
            this.gbMQTT.Controls.Add(this.lblInputTopic);
            this.gbMQTT.Location = new System.Drawing.Point(8, 6);
            this.gbMQTT.Name = "gbMQTT";
            this.gbMQTT.Size = new System.Drawing.Size(448, 245);
            this.gbMQTT.TabIndex = 1;
            this.gbMQTT.TabStop = false;
            this.gbMQTT.Text = "MQTT";
            // 
            // tbImageTopic
            // 
            this.tbImageTopic.Location = new System.Drawing.Point(127, 176);
            this.tbImageTopic.Name = "tbImageTopic";
            this.tbImageTopic.Size = new System.Drawing.Size(303, 22);
            this.tbImageTopic.TabIndex = 12;
            // 
            // lblImageTopic
            // 
            this.lblImageTopic.AutoSize = true;
            this.lblImageTopic.Location = new System.Drawing.Point(27, 176);
            this.lblImageTopic.Name = "lblImageTopic";
            this.lblImageTopic.Size = new System.Drawing.Size(89, 17);
            this.lblImageTopic.TabIndex = 11;
            this.lblImageTopic.Text = "Image Topic:";
            // 
            // tbBrokerPort
            // 
            this.tbBrokerPort.Location = new System.Drawing.Point(127, 58);
            this.tbBrokerPort.Name = "tbBrokerPort";
            this.tbBrokerPort.Size = new System.Drawing.Size(303, 22);
            this.tbBrokerPort.TabIndex = 10;
            // 
            // lblBrokerPort
            // 
            this.lblBrokerPort.AutoSize = true;
            this.lblBrokerPort.Location = new System.Drawing.Point(37, 61);
            this.lblBrokerPort.Name = "lblBrokerPort";
            this.lblBrokerPort.Size = new System.Drawing.Size(84, 17);
            this.lblBrokerPort.TabIndex = 9;
            this.lblBrokerPort.Text = "Broker Port:";
            // 
            // tbBrokerDomain
            // 
            this.tbBrokerDomain.Location = new System.Drawing.Point(127, 30);
            this.tbBrokerDomain.Name = "tbBrokerDomain";
            this.tbBrokerDomain.Size = new System.Drawing.Size(303, 22);
            this.tbBrokerDomain.TabIndex = 8;
            // 
            // lblBrokerDomain
            // 
            this.lblBrokerDomain.AutoSize = true;
            this.lblBrokerDomain.Location = new System.Drawing.Point(15, 33);
            this.lblBrokerDomain.Name = "lblBrokerDomain";
            this.lblBrokerDomain.Size = new System.Drawing.Size(106, 17);
            this.lblBrokerDomain.TabIndex = 7;
            this.lblBrokerDomain.Text = "Broker Domain:";
            // 
            // tbInputTopic
            // 
            this.tbInputTopic.Location = new System.Drawing.Point(127, 86);
            this.tbInputTopic.Name = "tbInputTopic";
            this.tbInputTopic.Size = new System.Drawing.Size(303, 22);
            this.tbInputTopic.TabIndex = 3;
            // 
            // tbOutputTopic
            // 
            this.tbOutputTopic.Location = new System.Drawing.Point(127, 132);
            this.tbOutputTopic.Name = "tbOutputTopic";
            this.tbOutputTopic.Size = new System.Drawing.Size(303, 22);
            this.tbOutputTopic.TabIndex = 2;
            // 
            // lblOutputTopic
            // 
            this.lblOutputTopic.AutoSize = true;
            this.lblOutputTopic.Location = new System.Drawing.Point(27, 132);
            this.lblOutputTopic.Name = "lblOutputTopic";
            this.lblOutputTopic.Size = new System.Drawing.Size(94, 17);
            this.lblOutputTopic.TabIndex = 1;
            this.lblOutputTopic.Text = "Output Topic:";
            // 
            // lblInputTopic
            // 
            this.lblInputTopic.AutoSize = true;
            this.lblInputTopic.Location = new System.Drawing.Point(39, 89);
            this.lblInputTopic.Name = "lblInputTopic";
            this.lblInputTopic.Size = new System.Drawing.Size(82, 17);
            this.lblInputTopic.TabIndex = 0;
            this.lblInputTopic.Text = "Input Topic:";
            // 
            // tbImageWidth
            // 
            this.tbImageWidth.Location = new System.Drawing.Point(127, 204);
            this.tbImageWidth.Name = "tbImageWidth";
            this.tbImageWidth.Size = new System.Drawing.Size(86, 22);
            this.tbImageWidth.TabIndex = 13;
            // 
            // tbImageHeight
            // 
            this.tbImageHeight.Location = new System.Drawing.Point(219, 204);
            this.tbImageHeight.Name = "tbImageHeight";
            this.tbImageHeight.Size = new System.Drawing.Size(86, 22);
            this.tbImageHeight.TabIndex = 14;
            // 
            // lblImageSize
            // 
            this.lblImageSize.AutoSize = true;
            this.lblImageSize.Location = new System.Drawing.Point(27, 207);
            this.lblImageSize.Name = "lblImageSize";
            this.lblImageSize.Size = new System.Drawing.Size(81, 17);
            this.lblImageSize.TabIndex = 15;
            this.lblImageSize.Text = "Image Size:";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 534);
            this.Controls.Add(this.tcSettings);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tcSettings.ResumeLayout(false);
            this.tpGlyphDatabase.ResumeLayout(false);
            this.tpGlyphDatabase.PerformLayout();
            this.tpMQTT.ResumeLayout(false);
            this.gbMQTT.ResumeLayout(false);
            this.gbMQTT.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tpGlyphDatabase;
        private System.Windows.Forms.TabPage tpMQTT;
        private System.Windows.Forms.GroupBox gbMQTT;
        private System.Windows.Forms.TextBox tbBrokerPort;
        private System.Windows.Forms.Label lblBrokerPort;
        private System.Windows.Forms.TextBox tbBrokerDomain;
        private System.Windows.Forms.Label lblBrokerDomain;
        private System.Windows.Forms.TextBox tbInputTopic;
        private System.Windows.Forms.TextBox tbOutputTopic;
        private System.Windows.Forms.Label lblOutputTopic;
        private System.Windows.Forms.Label lblInputTopic;
        private System.Windows.Forms.TextBox bDefaultDatabasePath;
        private System.Windows.Forms.Button btnOpenDefaultDatabase;
        private System.Windows.Forms.TextBox tbImageTopic;
        private System.Windows.Forms.Label lblImageTopic;
        private System.Windows.Forms.Label lblImageSize;
        private System.Windows.Forms.TextBox tbImageHeight;
        private System.Windows.Forms.TextBox tbImageWidth;
    }
}