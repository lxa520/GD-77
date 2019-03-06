using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DMR
{
	public class DigitalKeyContactForm : DockContent, IDisp
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public class NumKeyContact
		{
			private ushort index;

			private ushort reserve;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			private ushort[] contact;

			public ushort this[int index]
			{
				get
				{
					return this.contact[index];
				}
				set
				{
					this.method_0(index, value);
					this.contact[index] = value;
				}
			}

			private void method_0(int int_0, int int_1)
			{
				ushort num = (ushort)(~(1 << int_0));
				this.index &= num;
				if (int_1 != 0)
				{
					this.index |= (ushort)(1 << int_0);
				}
			}

			private bool method_1(int int_0)
			{
				try
				{
					BitArray bitArray = new BitArray(BitConverter.GetBytes(this.index));
					return bitArray[int_0];
				}
				catch
				{
					return false;
				}
			}

			public NumKeyContact()
			{
				
				//base._002Ector();
				this.contact = new ushort[10];
			}

			public void Verify()
			{
				int num = 0;
				int num2 = 0;
				for (num = 0; num < this.contact.Length; num++)
				{
					if (this.method_1(num))
					{
						num2 = this.contact[num] - 1;
						if (!ContactForm.data.DataIsValid(num2))
						{
							this.contact[num] = 0;
							Settings.smethod_17(ref this.index, num, 1);
						}
					}
				}
			}
		}

		private const int CNT_NUM_KEY_CONTACT = 10;

		public const string SZ_DIGIT_KEY_NAME = "DigitKey";

		private static string SZ_DIGIT_KEY_TEXT;

		private Dictionary<string, string> dicCom;

		public static NumKeyContact data;

		//private IContainer components;

		private DataGridView dgvContact;

		private DataGridViewComboBoxColumn cmbContact;

		public TreeNode Node
		{
			get;
			set;
		}

		public void SaveData()
		{
			try
			{
				int num = 0;
				this.dgvContact.EndEdit();
				for (num = 0; num < this.dgvContact.Rows.Count; num++)
				{
					if (this.dgvContact.Rows[num].Cells[0].Value != null)
					{
						DigitalKeyContactForm.data[num] = (ushort)(int)this.dgvContact.Rows[num].Cells[0].Value;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void DispData()
		{
			this.method_0();
			try
			{
				int num = 0;
				for (num = 0; num < this.dgvContact.Rows.Count; num++)
				{
					this.dgvContact.Rows[num].Cells[0].Value = (int)DigitalKeyContactForm.data[num];
				}
				this.dgvContact.EndEdit();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void RefreshName()
		{
		}

		public DigitalKeyContactForm()
		{
			
			this.dicCom = new Dictionary<string, string>();
			//base._002Ector();
			this.InitializeComponent();
			base.Scale(Settings.smethod_6());
		}

		private void method_0()
		{
			int i = 0;
			string text = "";
			this.dgvContact.RowCount = 10;
			this.cmbContact.Items.Clear();
			this.cmbContact.Items.Add(new NameValuePair(Settings.SZ_NONE, 0));
			for (i = 0; i < 1024; i++)
			{
				if (ContactForm.data.DataIsValid(i))
				{
					text = ContactForm.data[i].Name;
					this.cmbContact.Items.Add(new NameValuePair(text, i + 1));
				}
			}
			this.cmbContact.DisplayMember = "Text";
			this.cmbContact.ValueMember = "Value";
			for (i = 0; i < this.dgvContact.Rows.Count; i++)
			{
				this.dgvContact.Rows[i].HeaderCell.Value = DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT + i.ToString();
			}
		}

		public static void RefreshCommonLang()
		{
			string name = typeof(DigitalKeyContactForm).Name;
			Settings.smethod_77("DigitKey", ref DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT, name);
		}

		private void DigitalKeyContactForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);
			this.DispData();
		}

		private void DigitalKeyContactForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveData();
		}

		private void method_1(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			try
			{
				DataGridView dataGridView = sender as DataGridView;
				if (e.RowIndex >= dataGridView.FirstDisplayedScrollingRowIndex)
				{
					using (SolidBrush brush = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor))
					{
						string s = DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT + e.RowIndex.ToString();
						e.Graphics.DrawString(s, e.InheritedRowStyle.Font, brush, (float)(e.RowBounds.Location.X + 15), (float)(e.RowBounds.Location.Y + 5));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void dgvContact_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			DataGridView dataGridView = sender as DataGridView;
			if (e.Context == DataGridViewDataErrorContexts.Formatting && dataGridView != null)
			{
				dataGridView[e.ColumnIndex, e.RowIndex].Value = 0;
				e.Cancel = true;
			}
		}

		protected override void Dispose(bool disposing)
		{
            /*
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}*/
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.dgvContact = new DataGridView();
			this.cmbContact = new DataGridViewComboBoxColumn();
			((ISupportInitialize)this.dgvContact).BeginInit();
			base.SuspendLayout();
			this.dgvContact.AllowUserToAddRows = false;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Arial", 10f, FontStyle.Regular);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvContact.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvContact.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvContact.Columns.AddRange(this.cmbContact);
			this.dgvContact.EditMode = DataGridViewEditMode.EditOnEnter;
			this.dgvContact.Location = new Point(38, 12);
			this.dgvContact.Name = "dgvContact";
			this.dgvContact.RowHeadersWidth = 150;
			this.dgvContact.RowTemplate.Height = 23;
			this.dgvContact.Size = new Size(402, 289);
			this.dgvContact.TabIndex = 16;
			this.dgvContact.DataError += this.dgvContact_DataError;
			this.cmbContact.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.cmbContact.HeaderText = "Contact";
			this.cmbContact.Name = "cmbContact";
			base.AutoScaleDimensions = new SizeF(7f, 16f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(475, 345);
			base.Controls.Add(this.dgvContact);
			this.Font = new Font("Arial", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "DigitalKeyContactForm";
			this.Text = "Number Key Quick Contact Access";
			base.Load += this.DigitalKeyContactForm_Load;
			base.FormClosing += this.DigitalKeyContactForm_FormClosing;
			((ISupportInitialize)this.dgvContact).EndInit();
			base.ResumeLayout(false);
		}

		static DigitalKeyContactForm()
		{
			
			DigitalKeyContactForm.SZ_DIGIT_KEY_TEXT = "Number Key";
			DigitalKeyContactForm.data = new NumKeyContact();
		}
	}
}
