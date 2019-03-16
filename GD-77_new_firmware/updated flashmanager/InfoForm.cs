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
	public partial class InfoForm : Form
	{
		public InfoForm()
		{
			InitializeComponent();
			//this.StartPosition = FormStartPosition.CenterParent;
		}

        private void btnCancelInfo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
