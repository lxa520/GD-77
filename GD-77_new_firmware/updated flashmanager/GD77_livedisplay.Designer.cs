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
            this.buttonLivemodus = new System.Windows.Forms.Button();
            this.checkBoxLEDgreen = new System.Windows.Forms.CheckBox();
            this.checkBoxLEDred = new System.Windows.Forms.CheckBox();
            this.checkBoxPTT = new System.Windows.Forms.CheckBox();
            this.checkBoxSK2 = new System.Windows.Forms.CheckBox();
            this.checkBoxSK1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonTestUSB
            // 
            this.buttonTestUSB.Location = new System.Drawing.Point(713, 12);
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
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status:";
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
            // buttonLivemodus
            // 
            this.buttonLivemodus.Location = new System.Drawing.Point(12, 12);
            this.buttonLivemodus.Name = "buttonLivemodus";
            this.buttonLivemodus.Size = new System.Drawing.Size(75, 23);
            this.buttonLivemodus.TabIndex = 4;
            this.buttonLivemodus.Text = "Livemodus";
            this.buttonLivemodus.UseVisualStyleBackColor = true;
            this.buttonLivemodus.Click += new System.EventHandler(this.buttonLivemodus_Click);
            // 
            // checkBoxLEDgreen
            // 
            this.checkBoxLEDgreen.AutoSize = true;
            this.checkBoxLEDgreen.Location = new System.Drawing.Point(107, 16);
            this.checkBoxLEDgreen.Name = "checkBoxLEDgreen";
            this.checkBoxLEDgreen.Size = new System.Drawing.Size(77, 17);
            this.checkBoxLEDgreen.TabIndex = 5;
            this.checkBoxLEDgreen.Text = "LED green";
            this.checkBoxLEDgreen.UseVisualStyleBackColor = true;
            // 
            // checkBoxLEDred
            // 
            this.checkBoxLEDred.AutoSize = true;
            this.checkBoxLEDred.Location = new System.Drawing.Point(107, 39);
            this.checkBoxLEDred.Name = "checkBoxLEDred";
            this.checkBoxLEDred.Size = new System.Drawing.Size(65, 17);
            this.checkBoxLEDred.TabIndex = 6;
            this.checkBoxLEDred.Text = "LED red";
            this.checkBoxLEDred.UseVisualStyleBackColor = true;
            // 
            // checkBoxPTT
            // 
            this.checkBoxPTT.AutoCheck = false;
            this.checkBoxPTT.AutoSize = true;
            this.checkBoxPTT.Location = new System.Drawing.Point(204, 16);
            this.checkBoxPTT.Name = "checkBoxPTT";
            this.checkBoxPTT.Size = new System.Drawing.Size(47, 17);
            this.checkBoxPTT.TabIndex = 7;
            this.checkBoxPTT.Text = "PTT";
            this.checkBoxPTT.UseVisualStyleBackColor = true;
            // 
            // checkBoxSK2
            // 
            this.checkBoxSK2.AutoCheck = false;
            this.checkBoxSK2.AutoSize = true;
            this.checkBoxSK2.Location = new System.Drawing.Point(204, 62);
            this.checkBoxSK2.Name = "checkBoxSK2";
            this.checkBoxSK2.Size = new System.Drawing.Size(46, 17);
            this.checkBoxSK2.TabIndex = 8;
            this.checkBoxSK2.Text = "SK2";
            this.checkBoxSK2.UseVisualStyleBackColor = true;
            // 
            // checkBoxSK1
            // 
            this.checkBoxSK1.AutoCheck = false;
            this.checkBoxSK1.AutoSize = true;
            this.checkBoxSK1.Location = new System.Drawing.Point(204, 39);
            this.checkBoxSK1.Name = "checkBoxSK1";
            this.checkBoxSK1.Size = new System.Drawing.Size(46, 17);
            this.checkBoxSK1.TabIndex = 9;
            this.checkBoxSK1.Text = "SK1";
            this.checkBoxSK1.UseVisualStyleBackColor = true;
            // 
            // GD77_livedisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkBoxSK1);
            this.Controls.Add(this.checkBoxSK2);
            this.Controls.Add(this.checkBoxPTT);
            this.Controls.Add(this.checkBoxLEDred);
            this.Controls.Add(this.checkBoxLEDgreen);
            this.Controls.Add(this.buttonLivemodus);
            this.Controls.Add(this.labelStatusText);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonTestUSB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private System.Windows.Forms.Button buttonLivemodus;
        private System.Windows.Forms.CheckBox checkBoxLEDgreen;
        private System.Windows.Forms.CheckBox checkBoxLEDred;
        private System.Windows.Forms.CheckBox checkBoxPTT;
        private System.Windows.Forms.CheckBox checkBoxSK2;
        private System.Windows.Forms.CheckBox checkBoxSK1;
    }
}