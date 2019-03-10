using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GD77_FlashManager
{
	public partial class GotoAddressForm : Form
	{
		public int Address
		{
			get;
			set;
		}

		public GotoAddressForm()
		{
			InitializeComponent();
			Address = 0;
			this.StartPosition = FormStartPosition.CenterParent;
			txtAddress.Select();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				Address = int.Parse(txtAddress.Text, System.Globalization.NumberStyles.HexNumber);
				this.DialogResult = DialogResult.OK;
			}
			catch (Exception)
			{
				Address = 0;
				txtAddress.Text = "0";
				this.DialogResult = DialogResult.Cancel;
			}
			this.Close();
		}
	}
}
