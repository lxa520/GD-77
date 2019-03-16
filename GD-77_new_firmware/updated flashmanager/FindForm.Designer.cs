namespace GD77_FlashManager
{
	partial class FindForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindForm));
			this.btnFind = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtSearchFor = new System.Windows.Forms.TextBox();
			this.lblSearch = new System.Windows.Forms.Label();
			this.cmbDataType = new System.Windows.Forms.ComboBox();
			this.lblDataType = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnFind
			// 
			this.btnFind.Location = new System.Drawing.Point(206, 72);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(75, 23);
			this.btnFind.TabIndex = 0;
			this.btnFind.Text = "Find";
			this.btnFind.UseVisualStyleBackColor = true;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(125, 72);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtSearchFor
			// 
			this.txtSearchFor.Location = new System.Drawing.Point(77, 9);
			this.txtSearchFor.Name = "txtSearchFor";
			this.txtSearchFor.Size = new System.Drawing.Size(202, 20);
			this.txtSearchFor.TabIndex = 1;
			// 
			// lblSearch
			// 
			this.lblSearch.AutoSize = true;
			this.lblSearch.Location = new System.Drawing.Point(13, 15);
			this.lblSearch.Name = "lblSearch";
			this.lblSearch.Size = new System.Drawing.Size(56, 13);
			this.lblSearch.TabIndex = 2;
			this.lblSearch.Text = "Search for";
			// 
			// cmbDataType
			// 
			this.cmbDataType.FormattingEnabled = true;
			this.cmbDataType.Items.AddRange(new object[] {
            "Hex",
            "Text (ASCII)"});
			this.cmbDataType.Location = new System.Drawing.Point(77, 36);
			this.cmbDataType.Name = "cmbDataType";
			this.cmbDataType.Size = new System.Drawing.Size(202, 21);
			this.cmbDataType.TabIndex = 3;
			// 
			// lblDataType
			// 
			this.lblDataType.AutoSize = true;
			this.lblDataType.Location = new System.Drawing.Point(15, 39);
			this.lblDataType.Name = "lblDataType";
			this.lblDataType.Size = new System.Drawing.Size(53, 13);
			this.lblDataType.TabIndex = 2;
			this.lblDataType.Text = "Data type";
			// 
			// FindForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(291, 102);
			this.Controls.Add(this.cmbDataType);
			this.Controls.Add(this.lblDataType);
			this.Controls.Add(this.lblSearch);
			this.Controls.Add(this.txtSearchFor);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnFind);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FindForm";
			this.Text = "Find";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtSearchFor;
		private System.Windows.Forms.Label lblSearch;
		private System.Windows.Forms.ComboBox cmbDataType;
		private System.Windows.Forms.Label lblDataType;
	}
}