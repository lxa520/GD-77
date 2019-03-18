using System.ComponentModel.Design;
using System.Windows.Forms;
using System;
namespace GD77_FlashManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.hexBox = new Be.Windows.Forms.HexBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartAddr = new System.Windows.Forms.TextBox();
            this.txtLen = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnCalibration = new System.Windows.Forms.Button();
            this.btnReadCalibration = new System.Windows.Forms.Button();
            this.btnWriteCalibration = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.readFlashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeFlashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.livedisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readFromRadioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeToRadioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxReadInternalFlash = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hexBox
            // 
            this.hexBox.ColumnInfoVisible = true;
            this.hexBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox.LineInfoVisible = true;
            this.hexBox.Location = new System.Drawing.Point(110, 22);
            this.hexBox.Name = "hexBox";
            this.hexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox.Size = new System.Drawing.Size(669, 599);
            this.hexBox.StringViewVisible = true;
            this.hexBox.TabIndex = 0;
            this.hexBox.UseFixedBytesPerLine = true;
            this.hexBox.VScrollBarVisible = true;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(21, 34);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open File";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(21, 63);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "SaveFile";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start address (hex)";
            // 
            // txtStartAddr
            // 
            this.txtStartAddr.Location = new System.Drawing.Point(21, 114);
            this.txtStartAddr.Name = "txtStartAddr";
            this.txtStartAddr.Size = new System.Drawing.Size(75, 20);
            this.txtStartAddr.TabIndex = 3;
            this.txtStartAddr.Text = "80000";
            this.txtStartAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLen
            // 
            this.txtLen.Location = new System.Drawing.Point(21, 163);
            this.txtLen.Name = "txtLen";
            this.txtLen.Size = new System.Drawing.Size(75, 20);
            this.txtLen.TabIndex = 5;
            this.txtLen.Text = "10000";
            this.txtLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Length (hex)";
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(21, 198);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 0;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(21, 227);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 0;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnCalibration
            // 
            this.btnCalibration.Location = new System.Drawing.Point(21, 528);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(75, 44);
            this.btnCalibration.TabIndex = 0;
            this.btnCalibration.Text = "Adjust Calibration";
            this.btnCalibration.UseVisualStyleBackColor = true;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // btnReadCalibration
            // 
            this.btnReadCalibration.Location = new System.Drawing.Point(21, 482);
            this.btnReadCalibration.Name = "btnReadCalibration";
            this.btnReadCalibration.Size = new System.Drawing.Size(75, 40);
            this.btnReadCalibration.TabIndex = 0;
            this.btnReadCalibration.Text = "Read Calibration";
            this.btnReadCalibration.UseVisualStyleBackColor = true;
            this.btnReadCalibration.Click += new System.EventHandler(this.btnReadCalibration_Click);
            // 
            // btnWriteCalibration
            // 
            this.btnWriteCalibration.Location = new System.Drawing.Point(21, 578);
            this.btnWriteCalibration.Name = "btnWriteCalibration";
            this.btnWriteCalibration.Size = new System.Drawing.Size(75, 40);
            this.btnWriteCalibration.TabIndex = 0;
            this.btnWriteCalibration.Text = "Write Calibration";
            this.btnWriteCalibration.UseVisualStyleBackColor = true;
            this.btnWriteCalibration.Click += new System.EventHandler(this.btnWriteCalibration_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.livedisplayToolStripMenuItem,
            this.calibrationToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(797, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.mergeToolStripMenuItem,
            this.toolStripSeparator1,
            this.readFlashToolStripMenuItem,
            this.writeFlashToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // mergeToolStripMenuItem
            // 
            this.mergeToolStripMenuItem.Name = "mergeToolStripMenuItem";
            this.mergeToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.mergeToolStripMenuItem.Text = "Merge";
            this.mergeToolStripMenuItem.Click += new System.EventHandler(this.mergeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(129, 6);
            // 
            // readFlashToolStripMenuItem
            // 
            this.readFlashToolStripMenuItem.Name = "readFlashToolStripMenuItem";
            this.readFlashToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.readFlashToolStripMenuItem.Text = "Read Flash";
            this.readFlashToolStripMenuItem.Click += new System.EventHandler(this.readFlashToolStripMenuItem_Click);
            // 
            // writeFlashToolStripMenuItem
            // 
            this.writeFlashToolStripMenuItem.Name = "writeFlashToolStripMenuItem";
            this.writeFlashToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.writeFlashToolStripMenuItem.Text = "Write Flash";
            this.writeFlashToolStripMenuItem.Click += new System.EventHandler(this.writeFlashToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(129, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // livedisplayToolStripMenuItem
            // 
            this.livedisplayToolStripMenuItem.Name = "livedisplayToolStripMenuItem";
            this.livedisplayToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.livedisplayToolStripMenuItem.Text = "Livedisplay";
            this.livedisplayToolStripMenuItem.Click += new System.EventHandler(this.livedisplayToolStripMenuItem_Click);
            // 
            // calibrationToolStripMenuItem
            // 
            this.calibrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readFromRadioToolStripMenuItem,
            this.adjustSettingsToolStripMenuItem,
            this.writeToRadioToolStripMenuItem});
            this.calibrationToolStripMenuItem.Name = "calibrationToolStripMenuItem";
            this.calibrationToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.calibrationToolStripMenuItem.Text = "Calibration";
            this.calibrationToolStripMenuItem.Visible = false;
            // 
            // readFromRadioToolStripMenuItem
            // 
            this.readFromRadioToolStripMenuItem.Name = "readFromRadioToolStripMenuItem";
            this.readFromRadioToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.readFromRadioToolStripMenuItem.Text = "Read from radio";
            // 
            // adjustSettingsToolStripMenuItem
            // 
            this.adjustSettingsToolStripMenuItem.Name = "adjustSettingsToolStripMenuItem";
            this.adjustSettingsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.adjustSettingsToolStripMenuItem.Text = "Adjust settings";
            this.adjustSettingsToolStripMenuItem.Click += new System.EventHandler(this.adjustSettingsToolStripMenuItem_Click);
            // 
            // writeToRadioToolStripMenuItem
            // 
            this.writeToRadioToolStripMenuItem.Name = "writeToRadioToolStripMenuItem";
            this.writeToRadioToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.writeToRadioToolStripMenuItem.Text = "Write to radio";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.creditsToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.creditsToolStripMenuItem.Text = "Credits";
            this.creditsToolStripMenuItem.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // checkBoxReadInternalFlash
            // 
            this.checkBoxReadInternalFlash.AutoSize = true;
            this.checkBoxReadInternalFlash.Location = new System.Drawing.Point(21, 264);
            this.checkBoxReadInternalFlash.Name = "checkBoxReadInternalFlash";
            this.checkBoxReadInternalFlash.Size = new System.Drawing.Size(88, 30);
            this.checkBoxReadInternalFlash.TabIndex = 7;
            this.checkBoxReadInternalFlash.Text = "Read\r\ninternal Flash";
            this.checkBoxReadInternalFlash.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 633);
            this.Controls.Add(this.checkBoxReadInternalFlash);
            this.Controls.Add(this.txtLen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStartAddr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCalibration);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnWriteCalibration);
            this.Controls.Add(this.btnReadCalibration);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.hexBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "GD-77 Flash manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Be.Windows.Forms.HexBox hexBox;
//		private ByteViewer _bv;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtStartAddr;
		private System.Windows.Forms.TextBox txtLen;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnRead;
		private System.Windows.Forms.Button btnWrite;
		private Button btnCalibration;
		private Button btnReadCalibration;
		private Button btnWriteCalibration;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem toolStripMenuItem1;
		private ToolStripMenuItem newToolStripMenuItem;
		private ToolStripMenuItem saveToolStripMenuItem;
		private ToolStripMenuItem openToolStripMenuItem;
		private ToolStripMenuItem exitToolStripMenuItem;
		private ToolStripMenuItem calibrationToolStripMenuItem;
		private ToolStripMenuItem readFromRadioToolStripMenuItem;
		private ToolStripMenuItem adjustSettingsToolStripMenuItem;
		private ToolStripMenuItem writeToRadioToolStripMenuItem;
		private ToolStripMenuItem aboutToolStripMenuItem;
		private ToolStripMenuItem infoToolStripMenuItem;
		private ToolStripMenuItem creditsToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem readFlashToolStripMenuItem;
		private ToolStripMenuItem writeFlashToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem mergeToolStripMenuItem;
		private CheckBox checkBoxReadInternalFlash;
        private ToolStripMenuItem livedisplayToolStripMenuItem;
    }
}

