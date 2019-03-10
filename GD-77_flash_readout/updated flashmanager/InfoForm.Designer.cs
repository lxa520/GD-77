namespace GD77_FlashManager
{
	partial class InfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.btnCancelInfo = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lblInfoText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancelInfo
            // 
            this.btnCancelInfo.Location = new System.Drawing.Point(192, 576);
            this.btnCancelInfo.Name = "btnCancelInfo";
            this.btnCancelInfo.Size = new System.Drawing.Size(125, 27);
            this.btnCancelInfo.TabIndex = 0;
            this.btnCancelInfo.Text = "Cancel";
            this.btnCancelInfo.UseVisualStyleBackColor = true;
            this.btnCancelInfo.Click += new System.EventHandler(this.btnCancelInfo_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.CausesValidation = false;
            this.richTextBox1.Location = new System.Drawing.Point(49, 62);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(414, 488);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // lblInfoText
            // 
            this.lblInfoText.AutoSize = true;
            this.lblInfoText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoText.Location = new System.Drawing.Point(66, 23);
            this.lblInfoText.Name = "lblInfoText";
            this.lblInfoText.Size = new System.Drawing.Size(383, 19);
            this.lblInfoText.TabIndex = 2;
            this.lblInfoText.Text = "Please read carefully before using FlashManager!";
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 647);
            this.Controls.Add(this.lblInfoText);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnCancelInfo);
            this.Name = "InfoForm";
            this.Text = "Info";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCancelInfo;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lblInfoText;
    }
}