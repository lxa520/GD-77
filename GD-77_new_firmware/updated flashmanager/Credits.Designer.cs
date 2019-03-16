namespace GD77_FlashManager
{
	partial class Credits
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Credits));
			this.btnCancelCredits = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.lblInfoText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancelCredits
			// 
			this.btnCancelCredits.Location = new System.Drawing.Point(193, 218);
			this.btnCancelCredits.Name = "btnCancelCredits";
			this.btnCancelCredits.Size = new System.Drawing.Size(125, 27);
			this.btnCancelCredits.TabIndex = 0;
			this.btnCancelCredits.Text = "OK";
			this.btnCancelCredits.UseVisualStyleBackColor = true;
			this.btnCancelCredits.Click += new System.EventHandler(this.btnCancelInfo_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.CausesValidation = false;
			this.richTextBox1.Location = new System.Drawing.Point(59, 42);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(393, 155);
			this.richTextBox1.TabIndex = 1;
			this.richTextBox1.Text = "This non-commercial project is the result of a global cooperative effort of the f" +
    "ollowing ham radio amateurs:\n\nRoger Clark, VK3KYY / G4KYF\nJason Reilly, VK7ZJA\nC" +
    "olin Durbridge,G4EML";
			// 
			// lblInfoText
			// 
			this.lblInfoText.AutoSize = true;
			this.lblInfoText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfoText.Location = new System.Drawing.Point(141, 9);
			this.lblInfoText.Name = "lblInfoText";
			this.lblInfoText.Size = new System.Drawing.Size(215, 19);
			this.lblInfoText.TabIndex = 2;
			this.lblInfoText.Text = "Credits and special thanks!";
			// 
			// Credits
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(515, 261);
			this.Controls.Add(this.lblInfoText);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.btnCancelCredits);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Credits";
			this.Text = "Info";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancelCredits;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lblInfoText;
    }
}