using System;
using System.Windows.Forms;

namespace GD77_FlashManager
{
	partial class CalibrationForm
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
			this.tabCtlBands = new System.Windows.Forms.TabControl();
			this.tabVHF = new System.Windows.Forms.TabPage();
			this.calibrationBandControlVHF = new CalibrationBandControl();
			this.tabUHF = new System.Windows.Forms.TabPage();
			this.calibrationBandControlUHF = new CalibrationBandControl();
			this.btnWrite = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tabCtlBands.SuspendLayout();
			this.tabVHF.SuspendLayout();
			this.tabUHF.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCtlBands
			// 
			this.tabCtlBands.Controls.Add(this.tabVHF);
			this.tabCtlBands.Controls.Add(this.tabUHF);
			this.tabCtlBands.Location = new System.Drawing.Point(12, 12);
			this.tabCtlBands.Name = "tabCtlBands";
			this.tabCtlBands.SelectedIndex = 0;
			this.tabCtlBands.Size = new System.Drawing.Size(921, 524);
			this.tabCtlBands.TabIndex = 0;
			// 
			// tabVHF
			// 
			this.tabVHF.Controls.Add(this.calibrationBandControlVHF);
			this.tabVHF.Location = new System.Drawing.Point(4, 22);
			this.tabVHF.Name = "tabVHF";
			this.tabVHF.Padding = new System.Windows.Forms.Padding(3);
			this.tabVHF.Size = new System.Drawing.Size(913, 498);
			this.tabVHF.TabIndex = 0;
			this.tabVHF.Text = "VHF";
			this.tabVHF.UseVisualStyleBackColor = true;
			// 
			// calibrationBandControlVHF
			// 
			this.calibrationBandControlVHF.Location = new System.Drawing.Point(5, 5);
			this.calibrationBandControlVHF.Name = "calibrationBandControlVHF";
			this.calibrationBandControlVHF.Size = new System.Drawing.Size(880, 487);
			this.calibrationBandControlVHF.TabIndex = 0;
			this.calibrationBandControlVHF.Type = "VHF";
			// 
			// tabUHF
			// 
			this.tabUHF.Controls.Add(this.calibrationBandControlUHF);
			this.tabUHF.Location = new System.Drawing.Point(4, 22);
			this.tabUHF.Name = "tabUHF";
			this.tabUHF.Padding = new System.Windows.Forms.Padding(3);
			this.tabUHF.Size = new System.Drawing.Size(913, 498);
			this.tabUHF.TabIndex = 1;
			this.tabUHF.Text = "UHF";
			this.tabUHF.UseVisualStyleBackColor = true;
			// 
			// calibrationBandControlUHF
			// 
			this.calibrationBandControlUHF.Location = new System.Drawing.Point(5, 5);
			this.calibrationBandControlUHF.Name = "calibrationBandControlUHF";
			this.calibrationBandControlUHF.Size = new System.Drawing.Size(882, 487);
			this.calibrationBandControlUHF.TabIndex = 0;
			this.calibrationBandControlUHF.Type = "UHF";
			// 
			// btnWrite
			// 
			this.btnWrite.Location = new System.Drawing.Point(746, 561);
			this.btnWrite.Name = "btnWrite";
			this.btnWrite.Size = new System.Drawing.Size(102, 23);
			this.btnWrite.TabIndex = 1;
			this.btnWrite.Text = "Write to GD-77";
			this.btnWrite.UseVisualStyleBackColor = true;
			this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(854, 561);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// CalibrationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(945, 596);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnWrite);
			this.Controls.Add(this.tabCtlBands);
			this.Name = "CalibrationForm";
			this.Text = "Calibration";
			this.Shown += new System.EventHandler(this.onFormShown);
			this.tabCtlBands.ResumeLayout(false);
			this.tabVHF.ResumeLayout(false);
			this.tabUHF.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtlBands;
		private System.Windows.Forms.TabPage tabVHF;
		private System.Windows.Forms.TabPage tabUHF;
		private System.Windows.Forms.Button btnWrite;
		private System.Windows.Forms.Button btnCancel;
		private CalibrationBandControl calibrationBandControlUHF;
		private CalibrationBandControl calibrationBandControlVHF;
	}
}