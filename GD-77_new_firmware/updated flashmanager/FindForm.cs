using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Windows.Forms;

namespace GD77_FlashManager
{
	public partial class FindForm : Form
	{
		private FindOptions _findOptions;
		public FindForm()
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterParent;
			txtSearchFor.Select();
		}

		public FindForm(FindOptions findOptions)
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterParent;
			txtSearchFor.Select();
			_findOptions = findOptions;
		}
	
		private void findHexInit(string hexStr)
		{
			byte [] hexBytes = new byte[hexStr.Length / 2];// assume that the hex is well formed hence 2 chars per byte
			for(int i=0;i<hexStr.Length/2;i++)
			{
				hexBytes[i] = Byte.Parse(hexStr.Substring(i*2,2), System.Globalization.NumberStyles.HexNumber);
			}
			_findOptions.Type = FindType.Hex;
			_findOptions.Hex = hexBytes;
		}

		private void findTextInit(string textStr)
		{
			_findOptions.Type = FindType.Text;
			_findOptions.Text = textStr;
		}

		private void btnFind_Click(object sender, EventArgs e)
		{
			string s = txtSearchFor.Text;
			s = s.Replace(" ", "");// remove spaces
			if (0 != s.Length)
			{
				switch (cmbDataType.SelectedIndex)
				{
					case 0://Hex
						if (1 == s.Length % 2)
						{
							s = "0" + s;// add missing leading zero
						}
						findHexInit(s);
						break;
					case 1:// Text ASCII
						findTextInit(s);
						break;
				}
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

/* 
 * Unused code to find bytes. No longer used as HexBox has this feature built in
 * 	private int findBytes(byte[] pattern)
		{
			int i;
			var len = pattern.Length;
			var limit = MainForm.eeprom.Length - len;
			for (i = 0; i <= limit; i++)
			{
				int j = 0;
				for (; j < len; j++)
				{
					if (pattern[j] != MainForm.eeprom[i + j])
					{
						break;
					}
				}
				if (j == len)
				{
					return i;
				}
			}
			return -1;
		}
*/
	}
}
