namespace DiO_CS_GliphRecognizer
{
    partial class MainForm
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
            this.tblpnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.pbMain = new System.Windows.Forms.PictureBox();
            this.lblGlyphData = new System.Windows.Forms.Label();
            this.tblpnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // tblpnlMain
            // 
            this.tblpnlMain.ColumnCount = 1;
            this.tblpnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblpnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblpnlMain.Controls.Add(this.pbMain, 0, 1);
            this.tblpnlMain.Controls.Add(this.lblGlyphData, 0, 0);
            this.tblpnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblpnlMain.Location = new System.Drawing.Point(0, 0);
            this.tblpnlMain.Name = "tblpnlMain";
            this.tblpnlMain.RowCount = 2;
            this.tblpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.4581F));
            this.tblpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.5419F));
            this.tblpnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblpnlMain.Size = new System.Drawing.Size(610, 610);
            this.tblpnlMain.TabIndex = 5;
            // 
            // pbMain
            // 
            this.pbMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbMain.Location = new System.Drawing.Point(3, 109);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(604, 498);
            this.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbMain.TabIndex = 9;
            this.pbMain.TabStop = false;
            // 
            // lblGlyphData
            // 
            this.lblGlyphData.AutoSize = true;
            this.lblGlyphData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGlyphData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGlyphData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGlyphData.Location = new System.Drawing.Point(3, 0);
            this.lblGlyphData.Name = "lblGlyphData";
            this.lblGlyphData.Size = new System.Drawing.Size(604, 106);
            this.lblGlyphData.TabIndex = 10;
            this.lblGlyphData.Text = "Glyph:";
            this.lblGlyphData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 610);
            this.Controls.Add(this.tblpnlMain);
            this.Name = "MainForm";
            this.Text = "Gliph Recognizer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tblpnlMain.ResumeLayout(false);
            this.tblpnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblpnlMain;
        private System.Windows.Forms.PictureBox pbMain;
        private System.Windows.Forms.Label lblGlyphData;

    }
}

