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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tpMQTT = new System.Windows.Forms.TabPage();
            this.gbMQTT = new System.Windows.Forms.GroupBox();
            this.tbBrokerPort = new System.Windows.Forms.TextBox();
            this.lblBrokerPort = new System.Windows.Forms.Label();
            this.tbBrokerDomain = new System.Windows.Forms.TextBox();
            this.lblBrokerDomain = new System.Windows.Forms.Label();
            this.tbInputTopic = new System.Windows.Forms.TextBox();
            this.tbOutputTopic = new System.Windows.Forms.TextBox();
            this.lblOutputTopic = new System.Windows.Forms.Label();
            this.lblInputTopic = new System.Windows.Forms.Label();
            this.tcSettings.SuspendLayout();
            this.tpGlyphDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.tpGlyphDatabase.Controls.Add(this.dataGridView1);
            this.tpGlyphDatabase.Controls.Add(this.btnClear);
            this.tpGlyphDatabase.Controls.Add(this.btnAdd);
            this.tpGlyphDatabase.Location = new System.Drawing.Point(4, 25);
            this.tpGlyphDatabase.Name = "tpGlyphDatabase";
            this.tpGlyphDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tpGlyphDatabase.Size = new System.Drawing.Size(957, 505);
            this.tpGlyphDatabase.TabIndex = 0;
            this.tpGlyphDatabase.Text = "Glyph Database";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(5, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(350, 312);
            this.dataGridView1.TabIndex = 5;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(361, 67);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(164, 55);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(361, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(164, 55);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
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
            this.gbMQTT.Size = new System.Drawing.Size(448, 155);
            this.gbMQTT.TabIndex = 1;
            this.gbMQTT.TabStop = false;
            this.gbMQTT.Text = "MQTT";
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
            this.tbOutputTopic.Location = new System.Drawing.Point(127, 115);
            this.tbOutputTopic.Name = "tbOutputTopic";
            this.tbOutputTopic.Size = new System.Drawing.Size(303, 22);
            this.tbOutputTopic.TabIndex = 2;
            // 
            // lblOutputTopic
            // 
            this.lblOutputTopic.AutoSize = true;
            this.lblOutputTopic.Location = new System.Drawing.Point(27, 115);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tpMQTT.ResumeLayout(false);
            this.gbMQTT.ResumeLayout(false);
            this.gbMQTT.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tpGlyphDatabase;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAdd;
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
    }
}