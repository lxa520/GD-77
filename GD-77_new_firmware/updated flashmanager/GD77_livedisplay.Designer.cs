namespace GD77_FlashManager
{
    partial class GD77_livedisplay
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
            this.buttonTestUSB = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelStatusText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonTestUSB
            // 
            this.buttonTestUSB.Location = new System.Drawing.Point(12, 12);
            this.buttonTestUSB.Name = "buttonTestUSB";
            this.buttonTestUSB.Size = new System.Drawing.Size(75, 23);
            this.buttonTestUSB.TabIndex = 0;
            this.buttonTestUSB.Text = "TestUSB";
            this.buttonTestUSB.UseVisualStyleBackColor = true;
            this.buttonTestUSB.Click += new System.EventHandler(this.buttonTestUSB_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(9, 428);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(37, 13);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status";
            // 
            // labelStatusText
            // 
            this.labelStatusText.AutoSize = true;
            this.labelStatusText.Location = new System.Drawing.Point(52, 428);
            this.labelStatusText.Name = "labelStatusText";
            this.labelStatusText.Size = new System.Drawing.Size(22, 13);
            this.labelStatusText.TabIndex = 3;
            this.labelStatusText.Text = "OK";
            // 
            // GD77_livedisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelStatusText);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonTestUSB);
            this.Name = "GD77_livedisplay";
            this.Text = "GD77_livedisplay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GD77_livedisplay_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTestUSB;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelStatusText;
    }
}