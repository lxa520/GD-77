namespace DMR
{
	partial class DMRIDForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMRIDForm));
			this.btnDownload = new System.Windows.Forms.Button();
			this.btnReadFromGD77 = new System.Windows.Forms.Button();
			this.btnWriteToGD77 = new System.Windows.Forms.Button();
			this.txtRegionId = new System.Windows.Forms.TextBox();
			this.btnClear = new System.Windows.Forms.Button();
			this.lblMessage = new System.Windows.Forms.Label();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.txtAgeMaxDays = new System.Windows.Forms.TextBox();
			this.lblRegionId = new System.Windows.Forms.Label();
			this.lblInactivityFilter = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbtnName = new System.Windows.Forms.RadioButton();
			this.rbtnCallsign = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnDownload
			// 
			this.btnDownload.Location = new System.Drawing.Point(12, 38);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new System.Drawing.Size(114, 23);
			this.btnDownload.TabIndex = 0;
			this.btnDownload.Text = "Download";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
			// 
			// btnReadFromGD77
			// 
			this.btnReadFromGD77.Location = new System.Drawing.Point(12, 565);
			this.btnReadFromGD77.Name = "btnReadFromGD77";
			this.btnReadFromGD77.Size = new System.Drawing.Size(123, 23);
			this.btnReadFromGD77.TabIndex = 1;
			this.btnReadFromGD77.Text = "Read from GD-77";
			this.btnReadFromGD77.UseVisualStyleBackColor = true;
			this.btnReadFromGD77.Click += new System.EventHandler(this.btnReadFromGD77_Click);
			// 
			// btnWriteToGD77
			// 
			this.btnWriteToGD77.Location = new System.Drawing.Point(391, 565);
			this.btnWriteToGD77.Name = "btnWriteToGD77";
			this.btnWriteToGD77.Size = new System.Drawing.Size(123, 23);
			this.btnWriteToGD77.TabIndex = 2;
			this.btnWriteToGD77.Text = "Write to GD-77";
			this.btnWriteToGD77.UseVisualStyleBackColor = true;
			this.btnWriteToGD77.Click += new System.EventHandler(this.btnWriteToGD77_Click);
			// 
			// txtRegionId
			// 
			this.txtRegionId.Location = new System.Drawing.Point(472, 44);
			this.txtRegionId.Name = "txtRegionId";
			this.txtRegionId.Size = new System.Drawing.Size(42, 20);
			this.txtRegionId.TabIndex = 3;
			this.txtRegionId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(13, 68);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(114, 23);
			this.btnClear.TabIndex = 4;
			this.btnClear.Text = "Clear list";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.Location = new System.Drawing.Point(12, 9);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(405, 23);
			this.lblMessage.TabIndex = 5;
			this.lblMessage.Text = "lblMessage";
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(13, 97);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(501, 442);
			this.dataGridView1.TabIndex = 6;
			// 
			// txtAgeMaxDays
			// 
			this.txtAgeMaxDays.Location = new System.Drawing.Point(472, 70);
			this.txtAgeMaxDays.Name = "txtAgeMaxDays";
			this.txtAgeMaxDays.Size = new System.Drawing.Size(42, 20);
			this.txtAgeMaxDays.TabIndex = 3;
			this.txtAgeMaxDays.Text = "365";
			this.txtAgeMaxDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblRegionId
			// 
			this.lblRegionId.Location = new System.Drawing.Point(362, 47);
			this.lblRegionId.Name = "lblRegionId";
			this.lblRegionId.Size = new System.Drawing.Size(102, 13);
			this.lblRegionId.TabIndex = 7;
			this.lblRegionId.Text = "Region";
			this.lblRegionId.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblInactivityFilter
			// 
			this.lblInactivityFilter.Location = new System.Drawing.Point(332, 73);
			this.lblInactivityFilter.Name = "lblInactivityFilter";
			this.lblInactivityFilter.Size = new System.Drawing.Size(132, 13);
			this.lblInactivityFilter.TabIndex = 7;
			this.lblInactivityFilter.Text = "Inactivity filter (days)";
			this.lblInactivityFilter.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbtnName);
			this.groupBox1.Controls.Add(this.rbtnCallsign);
			this.groupBox1.Location = new System.Drawing.Point(190, 545);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(183, 48);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Use call or name when writing";
			// 
			// rbtnName
			// 
			this.rbtnName.AutoSize = true;
			this.rbtnName.Location = new System.Drawing.Point(111, 20);
			this.rbtnName.Name = "rbtnName";
			this.rbtnName.Size = new System.Drawing.Size(53, 17);
			this.rbtnName.TabIndex = 1;
			this.rbtnName.Text = "Name";
			this.rbtnName.UseVisualStyleBackColor = true;
			// 
			// rbtnCallsign
			// 
			this.rbtnCallsign.AutoSize = true;
			this.rbtnCallsign.Checked = true;
			this.rbtnCallsign.Location = new System.Drawing.Point(7, 20);
			this.rbtnCallsign.Name = "rbtnCallsign";
			this.rbtnCallsign.Size = new System.Drawing.Size(61, 17);
			this.rbtnCallsign.TabIndex = 0;
			this.rbtnCallsign.TabStop = true;
			this.rbtnCallsign.Text = "Callsign";
			this.rbtnCallsign.UseVisualStyleBackColor = true;
			// 
			// DMRIDForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(526, 595);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblInactivityFilter);
			this.Controls.Add(this.lblRegionId);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.txtAgeMaxDays);
			this.Controls.Add(this.txtRegionId);
			this.Controls.Add(this.btnWriteToGD77);
			this.Controls.Add(this.btnReadFromGD77);
			this.Controls.Add(this.btnDownload);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DMRIDForm";
			this.Text = "DMR ID";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DMRIDFormNew_FormClosing);
			this.Load += new System.EventHandler(this.DMRIDForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnDownload;
		private System.Windows.Forms.Button btnReadFromGD77;
		private System.Windows.Forms.Button btnWriteToGD77;
		private System.Windows.Forms.TextBox txtRegionId;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.TextBox txtAgeMaxDays;
		private System.Windows.Forms.Label lblRegionId;
		private System.Windows.Forms.Label lblInactivityFilter;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbtnName;
		private System.Windows.Forms.RadioButton rbtnCallsign;
	}
}