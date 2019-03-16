using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace GD77_FlashManager
{
	public partial class CalibrationForm : Form
	{
		//private CalibrationData _vhfData;
		//private CalibrationData _uhfData;
		private int _offsetAddress = 0x8F000;
		public CalibrationForm(int offsetAddress = 0x8F000) 
		{
			_offsetAddress = offsetAddress;
			InitializeComponent();
			// Need to setup the VHF and UHF data storage class first, as its used when initialising the components
			int calibrationDataSize = Marshal.SizeOf(typeof(CalibrationData));
			byte[] array = new byte[calibrationDataSize];
			Array.Copy(MainForm.CommsBuffer, _offsetAddress, array, 0, calibrationDataSize);
			this.calibrationBandControlUHF.data  = (CalibrationData)ByteToData(array);

			array = new byte[calibrationDataSize];
			Array.Copy(MainForm.CommsBuffer, _offsetAddress+ 0x70, array, 0, calibrationDataSize);
			this.calibrationBandControlVHF.data  = (CalibrationData)ByteToData(array);
		}

		private void btnWrite_Click(object sender, EventArgs e)
		{
			if (DialogResult.Yes != MessageBox.Show("Writing the calibration data to Radioddity GD-77 or any other compatible radio, could potentially damage your radio.\n\nBy clicking 'Yes' you acknowledge that you use this feature entirely at your own risk", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2))
			{
				return;
			}

			int calibrationDataSize = Marshal.SizeOf(typeof(CalibrationData));

			byte[] array = DataToByte(this.calibrationBandControlUHF.data);
			Array.Copy(array, 0, MainForm.CommsBuffer, _offsetAddress, calibrationDataSize);

			array = DataToByte(this.calibrationBandControlVHF.data);
			Array.Copy(array, 0, MainForm.CommsBuffer, _offsetAddress + 0x70, calibrationDataSize);
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private CalibrationData ByteToData(byte[] byte_0)
		{
			int num = Marshal.SizeOf(typeof(CalibrationData));
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(byte_0, 0, intPtr, num);
			object result = Marshal.PtrToStructure(intPtr, typeof(CalibrationData));
			Marshal.FreeHGlobal(intPtr);
			return (CalibrationData)result;
		}

		public static byte[] DataToByte(CalibrationData object_0)
		{
			int num = Marshal.SizeOf(typeof(CalibrationData));
			byte[] array = new byte[num];
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.StructureToPtr(object_0, intPtr, false);
			Marshal.Copy(intPtr, array, 0, num);
			Marshal.FreeHGlobal(intPtr);
			return array;
		}

		private void onFormShown(object sender, EventArgs e)
		{
			if (DialogResult.Yes != MessageBox.Show("This feature is provided 'as is'. You use it at your own risk.\n\nMaking changes to the flash memory in the Radioddity GD-77 or any other compatible radio, using this feature, could potentially damage your radio.\n\nBy clicking 'Yes' you acknowledge that you use this software entirely at your own risk", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2))
			{
				this.Close();
			}
		}
	}
}
