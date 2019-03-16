namespace GD77_FlashManager
{
	partial class MergeFileForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeFileForm));
			this.btnMerge = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblAddress = new System.Windows.Forms.Label();
			this.txtMergeAddress = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnMerge
			// 
			this.btnMerge.Location = new System.Drawing.Point(153, 51);
			this.btnMerge.Name = "btnMerge";
			this.btnMerge.Size = new System.Drawing.Size(75, 23);
			this.btnMerge.TabIndex = 0;
			this.btnMerge.Text = "Merge";
			this.btnMerge.UseVisualStyleBackColor = true;
			this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(72, 51);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblAddress
			// 
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(13, 13);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(109, 13);
			this.lblAddress.TabIndex = 1;
			this.lblAddress.Text = "Address to merge into";
			// 
			// txtMergeAddress
			// 
			this.txtMergeAddress.Location = new System.Drawing.Point(129, 11);
			this.txtMergeAddress.Name = "txtMergeAddress";
			this.txtMergeAddress.Size = new System.Drawing.Size(100, 20);
			this.txtMergeAddress.TabIndex = 2;
			this.txtMergeAddress.Text = "0";
			this.txtMergeAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// MergeFileForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(246, 90);
			this.Controls.Add(this.txtMergeAddress);
			this.Controls.Add(this.lblAddress);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnMerge);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MergeFileForm";
			this.Text = "Set merge address";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnMerge;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.TextBox txtMergeAddress;
	}
}