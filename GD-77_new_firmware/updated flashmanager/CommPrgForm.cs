using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace GD77_FlashManager
{
	public class CommPrgForm : Form
	{
		//private IContainer components;
		private Label lblPrompt;
		private ProgressBar prgComm;
		private Button btnCancel;
        private Button btnOK;
		private FirmwareUpdate firmwareUpdate;
		private CodeplugComms hidComm;



		public bool IsRead
		{
			get;
			set;
		}

		public bool IsSucess
		{
			get;
			set;
		}



		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommPrgForm));
			this.lblPrompt = new System.Windows.Forms.Label();
			this.prgComm = new System.Windows.Forms.ProgressBar();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblPrompt
			// 
			this.lblPrompt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblPrompt.Location = new System.Drawing.Point(26, 37);
			this.lblPrompt.Name = "lblPrompt";
			this.lblPrompt.Size = new System.Drawing.Size(380, 26);
			this.lblPrompt.TabIndex = 0;
			this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// prgComm
			// 
			this.prgComm.Location = new System.Drawing.Point(26, 13);
			this.prgComm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.prgComm.Name = "prgComm";
			this.prgComm.Size = new System.Drawing.Size(380, 15);
			this.prgComm.TabIndex = 1;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(167, 80);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(87, 31);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(319, 80);
			this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(87, 31);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Visible = false;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// CommPrgForm
			// 
			this.ClientSize = new System.Drawing.Size(424, 122);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.prgComm);
			this.Controls.Add(this.lblPrompt);
			this.Font = new System.Drawing.Font("Arial", 10F);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CommPrgForm";
			this.ShowInTaskbar = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommPrgForm_FormClosing);
			this.Load += new System.EventHandler(this.CommPrgForm_Load);
			this.ResumeLayout(false);

		}

		public CommPrgForm()
		{
			
			this.firmwareUpdate = new FirmwareUpdate();
			this.hidComm = new CodeplugComms();
			this.InitializeComponent();
			
		}

		private void CommPrgForm_Load(object sender, EventArgs e)
		{
			this.prgComm.Minimum = 0;
			this.prgComm.Maximum = 100;
			this.firmwareUpdate.method_3(this.IsRead);
			if (this.IsRead)
			{
				this.Text = "Read";
			}
			else
			{
				this.Text = "Write";
			}
			this.hidComm.setIsRead(this.IsRead);
			if (this.IsRead)
			{
				this.hidComm.START_ADDR = new int[7]
				{
					128,
					304,
					21392,
					29976,
					32768,
					44816,
					95776
				};
				this.hidComm.END_ADDR = new int[7]
				{
					297,
					14208,
					22056,
					30208,
					32784,
					45488,
					126624
				};
			}
			else
			{
				this.hidComm.START_ADDR = new int[7]
				{
					128,
					304,
					21392,
					29976,
					32768,
					44816,
					95776
				};
				this.hidComm.END_ADDR = new int[7]
				{
					297,
					14208,
					22056,
					30208,
					32784,
					45488,
					126624
				};
			}
			this.hidComm.method_9(this.method_0);
            this.hidComm.startCodeplugReadOrWriteInNewThread();
		}

		private void CommPrgForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.hidComm.isThreadAlive())
			{
				this.hidComm.setCancelComm(true);
				this.hidComm.joinThreadIfAlive();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        private void btnOK_Click(object sender, EventArgs e)
        {
			this.Close();
        }


		private void method_0(object sender, FirmwareUpdateProgressEventArgs e)
		{
			if (this.prgComm.InvokeRequired)
			{
				base.BeginInvoke(new EventHandler<FirmwareUpdateProgressEventArgs>(this.method_0), sender, e);
			}
			else
			{
				if (e.Failed)
				{
					if (!string.IsNullOrEmpty(e.Message))
					{
						MessageBox.Show(e.Message, "");
					}
					base.Close();
				}
				else
				{
					if (!e.Closed)
					{
						this.prgComm.Value = (int)e.Percentage;
						if (e.Percentage == (float)this.prgComm.Maximum)
						{
							this.IsSucess = true;


							if (this.IsRead)
							{
								this.lblPrompt.Text = "Read Complete";
							}
							else
							{
								this.lblPrompt.Text = "Write Complete";
							}
							this.btnOK.Visible = true;
							this.btnCancel.Visible = false;
						}
						else
						{
							this.lblPrompt.Text = string.Format("{0}%", this.prgComm.Value);
						}
					}
				}
			}
		}
	}
}
