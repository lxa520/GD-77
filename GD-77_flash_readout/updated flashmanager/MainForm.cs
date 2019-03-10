using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.IO;
using Be.Windows.Forms;

namespace GD77_FlashManager
{
	public partial class MainForm : Form
	{
		public static byte [] CommsBuffer = new byte[1024 * 1024];
		public static int startAddress;
		public static int transferLength;
        public static bool readInternalFlash;
		private FixedByteProvider _dbp;
		private bool _hexboxHasChanged = false;
		FindOptions	_findOptions; 
        public static String AppName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        public static String FileName = "";
		
		public MainForm()
		{
			InitializeComponent();
			// Hide calibration from normal users
			this.btnReadCalibration.Visible = false;
			this.btnWriteCalibration.Visible = false;
			this.btnCalibration.Visible = false;

			hexBox.ByteProvider = _dbp = new FixedByteProvider(CommsBuffer);
			
			_dbp.Changed += new EventHandler(onDataProviderChanged);// No point doing something every time this changes, as it only alerts to the fact that something has changed and not what specific byte has changed
		}

		public void onDataProviderChanged(object sender, EventArgs e)
		{
			_hexboxHasChanged = true;
		}

		private void btnRead_Click(object sender, EventArgs e)
		{
			CommPrgForm commPrgForm = new CommPrgForm();
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			commPrgForm.IsRead = true;
            MainForm.readInternalFlash = checkBoxReadInternalFlash.Checked;
			MainForm.startAddress = int.Parse(txtStartAddr.Text, System.Globalization.NumberStyles.HexNumber);
			if (MainForm.startAddress % 32 != 0)
			{
				MessageBox.Show("Start address must be a multiple of 0x20");
				return;
			}

			MainForm.transferLength = int.Parse(txtLen.Text, System.Globalization.NumberStyles.HexNumber);
			if (MainForm.transferLength == 0)
			{
				MessageBox.Show("Length cant be zero");
				return;
			}
			if (MainForm.transferLength % 32 != 0)
			{
				MessageBox.Show("Length must be a multplie of 0x20");
				return;
			}

			if (MainForm.startAddress + MainForm.transferLength > MainForm.CommsBuffer.Length)
			{
				MessageBox.Show("Start address and length settings would result in reading beyond the end of memory");
				return;
			}
			commPrgForm.ShowDialog();
			hexBox.ByteProvider = _dbp = new FixedByteProvider(CommsBuffer);
			hexBox.ScrollByteToTop(MainForm.startAddress);
			_hexboxHasChanged = false;
		}

		private void btnReadCalibration_Click(object sender, EventArgs e)
		{
			CommPrgForm commPrgForm = new CommPrgForm();
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			commPrgForm.IsRead = true;
			MainForm.startAddress = 0x80000;
			MainForm.transferLength = 0x10000;
			commPrgForm.ShowDialog();
			hexBox.ByteProvider = _dbp = new FixedByteProvider(CommsBuffer);
			hexBox.ScrollByteToTop(0x8F000);
			_hexboxHasChanged = false;
		}

		private void btnWriteCalibration_Click(object sender, EventArgs e)
		{
			hexBox.ScrollByteToTop(0x8F000);
			CommPrgForm commPrgForm = new CommPrgForm();
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			commPrgForm.IsRead = false;
			MainForm.startAddress = 0x80000;
			MainForm.transferLength = 0x10000;
			copyHexboxToEeprom();
			commPrgForm.ShowDialog();
		}


		private void btnWrite_Click(object sender, EventArgs e)
		{
			CommPrgForm commPrgForm = new CommPrgForm();
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			commPrgForm.IsRead = false;

			MainForm.startAddress = int.Parse(txtStartAddr.Text, System.Globalization.NumberStyles.HexNumber);
			if (MainForm.startAddress % 32 != 0)
			{
				MessageBox.Show("Start address must be a multiple of 0x20");
				return;
			}
			

			MainForm.transferLength = int.Parse(txtLen.Text, System.Globalization.NumberStyles.HexNumber);
			if (MainForm.transferLength % 32 != 0)
			{
				MessageBox.Show("Length must be a multplie of 0x20");
				return;
			}

			if (MainForm.startAddress + MainForm.transferLength > MainForm.CommsBuffer.Length)
			{
				MessageBox.Show("Start address and length settings would result in writing beyond the end of memory");
				return;
			}
			copyHexboxToEeprom();
			commPrgForm.ShowDialog();
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "binary files (*.bin)|*.bin|All files (*.*)|*.*";
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					byte []tmp = File.ReadAllBytes(openFileDialog1.FileName);
					if (tmp.Length != MainForm.CommsBuffer.Length)
					{
						MessageBox.Show("File is smaller than the size of the flash memory.\nOnly the fisrt "+ tmp.Length + " bytes have been updated", "Warning");
					}
					CommsBuffer = new byte[1024 * 1024];// Clear the whole memory buffer my making a new one.
					tmp.CopyTo(MainForm.CommsBuffer, 0);// copy the new data into the empty buffer
					MainForm.FileName = openFileDialog1.FileName;
					MainForm.ActiveForm.Text = AppName + " - Current File: " + FileName;
					hexBox.ByteProvider = _dbp = new FixedByteProvider(CommsBuffer);
					_hexboxHasChanged = false;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.Filter = "binary files (*.bin)|*.bin|All files (*.*)|*.*";
			saveFileDialog1.RestoreDirectory = true;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				copyHexboxToEeprom();
				File.WriteAllBytes(saveFileDialog1.FileName, MainForm.CommsBuffer);
			}
		}
		private void btnCalibration_Click(object sender, EventArgs e)
		{
			CalibrationForm cf = new CalibrationForm();
			cf.ShowDialog();
			hexBox.ByteProvider = _dbp = new FixedByteProvider(CommsBuffer);
			hexBox.ScrollByteToTop(0x8F000);
		}

		private void copyHexboxToEeprom()
		{
		//	if (_hexboxHasChanged)
			{
				Console.WriteLine("HexBox changed");
				Array.Copy(_dbp.Bytes.ToArray<byte>(), MainForm.CommsBuffer, MainForm.CommsBuffer.Length);
				_hexboxHasChanged = false;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			if (DialogResult.Yes != MessageBox.Show("This software is provided 'as is'. You use it at your own risk.\n\nMaking changes to the flash memory in the Radioddity GD-77 or any other compatibile radio, using this tool, could potentially damage your radio.\n\nBy clicking 'Yes' you acknoledge that you use this software entirely at your own risk", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2))
			{
				if (System.Windows.Forms.Application.MessageLoop)
				{
					// Use this since we are a WinForms app
					System.Windows.Forms.Application.Exit();
				}
				else
				{
					// Use this since we are a console app
					System.Environment.Exit(1);
				}
			}
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
				switch (e.KeyCode)
				{
					case Keys.G:
						if (e.Modifiers  == Keys.Control)
						{
						GotoAddressForm gaf = new GotoAddressForm();
							if (gaf.ShowDialog() == DialogResult.OK)
							{
								hexBox.ScrollByteToTop(gaf.Address);
							}
						}
						break;
					case Keys.F:
						if (e.Modifiers  == Keys.Control)
						{
							_findOptions = new FindOptions();
							FindForm findForm = new FindForm(_findOptions);
							if (findForm.ShowDialog() == DialogResult.OK)
							{
								if (-1 == hexBox.Find(_findOptions))
								{
									MessageBox.Show("Pattern not found");
								}
							}
						}
						break;
					case Keys.F3:
						if (_findOptions != null)
						{
							if (-1 == hexBox.Find(_findOptions))
							{
								MessageBox.Show("Pattern not found");
							}
						}
						break;
					case Keys.F12:
						if (e.Modifiers == Keys.Alt)
						{
							if (DialogResult.OK == MessageBox.Show("The Calibration feature is still in development.\nNot all paramaters have been tested\nYou use this feature at your own risk.", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
							{
								this.btnReadCalibration.Visible = true;
								this.btnWriteCalibration.Visible = true;
								this.btnCalibration.Visible = true;
							}
						}
						break;
				}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == MessageBox.Show("Confirm to exit FlashManager now.\n\nPress Cancel if you want to save settings before!", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
			{
				// Use this since we are a console app
				System.Environment.Exit(0);
			}
		}

		private void mergeFile()
		{
			byte[] tmp;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "binary files (*.bin)|*.bin|All files (*.*)|*.*";
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					tmp = File.ReadAllBytes(openFileDialog1.FileName);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
					return;
				}
				MainForm.FileName = openFileDialog1.FileName;
				MainForm.ActiveForm.Text = AppName + " - Current File: " + FileName;
				MergeFileForm mff = new MergeFileForm();

				if (DialogResult.OK == mff.ShowDialog())
				{
					try
					{
						tmp.CopyTo(MainForm.CommsBuffer, int.Parse(mff.MergeAddress, System.Globalization.NumberStyles.HexNumber));

						hexBox.ByteProvider = _dbp = new FixedByteProvider(CommsBuffer);
						_hexboxHasChanged = false;
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error: Data could be merged.  System error: " + ex.Message);
					}
				}
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			btnOpen.PerformClick();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			btnSave.PerformClick();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			btnCalibration.PerformClick();
		}

		private void adjustSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == MessageBox.Show("The Calibration feature is still in development.\nNot all paramaters have been tested\nYou use this feature at your own risk.", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
			{
				this.btnReadCalibration.Visible = true;
				this.btnWriteCalibration.Visible = true;
				this.btnCalibration.Visible = true;
				this.calibrationToolStripMenuItem.Visible = true;
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void infoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InfoForm inf = new InfoForm();
			inf.ShowDialog();
		}

		private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Credits cred = new Credits();
			cred.ShowDialog();
		}

		private void readFlashToolStripMenuItem_Click(object sender, EventArgs e)
		{
			btnRead.PerformClick();
		}

		private void writeFlashToolStripMenuItem_Click(object sender, EventArgs e)
		{
			btnWrite.PerformClick();
		}

		private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			mergeFile();
		}

	}
}
