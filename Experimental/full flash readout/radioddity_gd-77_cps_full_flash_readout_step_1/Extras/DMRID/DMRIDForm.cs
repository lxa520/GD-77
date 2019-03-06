using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

namespace DMR
{
	public partial class DMRIDForm : Form
	{
		public static byte[] DMRIDBuffer = new byte[0x20000];

		static  List<DMRDataItem> DataList = null;
		private static byte[] SIG_PATTERN_BYTES;
		private WebClient _wc;
		private bool _isDownloading = false;
		const int MAX_RECORDS = 10920;

		public static void ClearStaticData()
		{
			DMRIDBuffer = new byte[0x20000];
		}

		public DMRIDForm()
		{
			SIG_PATTERN_BYTES = new byte[] { 0x49, 0x44, 0x2D, 0x56, 0x30, 0x30, 0x31, 0x00 };
			InitializeComponent();

			txtRegionId.Text = (int.Parse(GeneralSetForm.data.RadioId) / 10000).ToString();

			DataList = new List<DMRDataItem>();

			dataGridView1.AutoGenerateColumns = false;
			DataGridViewCell cell = new DataGridViewTextBoxCell();
			DataGridViewTextBoxColumn colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Id", // internal name
				HeaderText = "ID",// Column header text
				DataPropertyName = "DMRID" // object property
			};
			dataGridView1.Columns.Add(colFileName);

			cell = new DataGridViewTextBoxCell();
			colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Call",// internal name
				HeaderText = "Callsign",// Column header text
				DataPropertyName = "Callsign"  // object property
			};
			dataGridView1.Columns.Add(colFileName);

			cell = new DataGridViewTextBoxCell();
			colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Name",// internal name
				HeaderText = "Name",// Column header text
				DataPropertyName = "Name"  // object property
			};
			dataGridView1.Columns.Add(colFileName);


			cell = new DataGridViewTextBoxCell();
			colFileName = new DataGridViewTextBoxColumn()
			{
				CellTemplate = cell,
				Name = "Age",// internal name
				HeaderText = "Last heard (days ago)",// Column header text
				DataPropertyName = "AgeInDays",  // object property
				Width = 140,
				ValueType = typeof(int)
			};
			dataGridView1.Columns.Add(colFileName);
			dataGridView1.UserDeletedRow += new DataGridViewRowEventHandler(dataGridRowDeleted);

			rebindData();	
		}

		private void dataGridRowDeleted(object sender, DataGridViewRowEventArgs e)
		{
			updateTotalNumberMessage();
		}

		private void rebindData()
		{
			var bindingList = new BindingList<DMRDataItem>(DataList);
			var source = new BindingSource(bindingList, null);
			dataGridView1.DataSource = source;
			updateTotalNumberMessage();
		}

		private void btnDownload_Click(object sender, EventArgs e)
		{
			if (DataList == null || _isDownloading)
			{
				return;
			}

			_wc = new WebClient();
			try
			{
				lblMessage.Text = Settings.dicCommon["DownloadContactsDownloading"];
				Cursor.Current = Cursors.WaitCursor;
				this.Refresh();
				Application.DoEvents();
				_wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DMRMARCDownloadCompleteHandler);
				_wc.DownloadStringAsync(new Uri("http://ham-digital.org/user_by_lh.php?id=" + txtRegionId.Text));
	
			}
			catch (Exception )
			{
				Cursor.Current = Cursors.Default;
				MessageBox.Show(Settings.dicCommon["UnableDownloadFromInternet"]);
				return;
			}
			_isDownloading = true;

		}


		private void DMRMARCDownloadCompleteHandler(object sender, DownloadStringCompletedEventArgs e )
		{
			string ownRadioId = GeneralSetForm.data.RadioId;
			string csv;// = e.Result;
			int maxAge = Int32.MaxValue;


			try
			{
				csv = e.Result;
			}
			catch(Exception)
			{
				MessageBox.Show(Settings.dicCommon["UnableDownloadFromInternet"]);
				return;
			}

			try
			{
				maxAge = Int32.Parse(this.txtAgeMaxDays.Text);
			}
			catch(Exception)
			{

			}

			try
			{
				bool first = true;
				foreach (var csvLine in csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (first)
					{
						first = false;
						continue;
					}
					DMRDataItem item = (new DMRDataItem()).FromHamDigital(csvLine);
					if (item.AgeAsInt <= maxAge)
					{
						DataList.Add(item);
					}
				}
				DataList = DataList.Distinct().ToList();

				rebindData();
				//DataToCodeplug();
				Cursor.Current = Cursors.Default;
	
			}
			catch (Exception ex)
			{
				MessageBox.Show(Settings.dicCommon["ErrorParsingData"]);
			}
			finally
			{
				_wc = null;
				_isDownloading = false;
				Cursor.Current = Cursors.Default;
			}
		}

		private void updateTotalNumberMessage()
		{
			string message = Settings.dicCommon["DMRIdContcatsTotal"];// "Total number of IDs = {0}. Max of MAX_RECORDS can be uploaded";
			lblMessage.Text = string.Format(message, DataList.Count);
		}

		private void downloadProgressHandler(object sender, DownloadProgressChangedEventArgs e)
		{
			try
			{
				BeginInvoke((Action)(() =>
				{
					lblMessage.Text = Settings.dicCommon["DownloadContactsDownloading"] + e.ProgressPercentage + "%";
				}));
			}
			catch (Exception)
			{
				// No nothing
			}
		}

		private void btnReadFromGD77_Click(object sender, EventArgs e)
		{
			MainForm.CommsBuffer = new byte[0x100000];// 128k buffer
			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
			CommPrgForm commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			CodeplugComms.startAddress = 0x50100;
			CodeplugComms.transferLength = 0x20;
			DialogResult result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x50100, DMRIDForm.DMRIDBuffer, 0, 0x20);
			if (!isInMemoryAccessMode(DMRIDForm.DMRIDBuffer))
			{
				MessageBox.Show(Settings.dicCommon["EnableMemoryAccessMode"]);
				return;
			}

			CodeplugComms.startAddress = 0x30000;
			CodeplugComms.transferLength = 0x20;
			result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x30000, DMRIDForm.DMRIDBuffer, 0, 0x20);


			int numRecords = BitConverter.ToInt32(DMRIDForm.DMRIDBuffer, 8);
			
			CodeplugComms.startAddress = 0x30000;
			CodeplugComms.transferLength = Math.Min(0x20000,12 + (numRecords+2)*12);

			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
			result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x30000, DMRIDForm.DMRIDBuffer, 0, CodeplugComms.transferLength);
			radioToData();
			rebindData();
			//DataToCodeplug();
		}

		private void radioToData()
		{
			byte []buf = new byte[12];
			DataList = new List<DMRDataItem>();
			int numRecords = BitConverter.ToInt32(DMRIDForm.DMRIDBuffer, 8);
			for (int i = 0; i < numRecords; i++)
			{
				Array.Copy(DMRIDForm.DMRIDBuffer, 0x0c + i*12, buf, 0, 12);
				DataList.Add((new DMRDataItem()).FromRadio(buf));
			}
		}

		public void CodeplugToData()
		{
			byte[] buf = new byte[12];
			DataList = new List<DMRDataItem>();
			int numRecords = BitConverter.ToInt32(DMRIDForm.DMRIDBuffer, 8);// Number of records is stored at offset 8
			for (int i = 0; i < numRecords; i++)
			{
				Array.Copy(DMRIDForm.DMRIDBuffer, 0x0c + i * 12, buf, 0, 12);
				DataList.Add(new DMRDataItem(buf));
			}
		}

#if false
		public static void DataToCodeplug()
		{
			int numRecords = Math.Min(DataList.Count,MAX_RECORDS);// max records is 109020 (as thats all that will fit in 0x20000
			Array.Copy(SIG_PATTERN_BYTES, DMRIDBuffer, SIG_PATTERN_BYTES.Length);
			Array.Copy(BitConverter.GetBytes(numRecords), 0, DMRIDBuffer, 8, 4);// Update number of records field
			for (int i = 0; i< numRecords;i++)
			{
				Array.Copy(DataList[i].getCodeplugData(), 0, DMRIDBuffer, 0x0c + i * 12, 12);
			}
		}
#endif


		private byte[] GenerateUploadData()
		{
			int numRecords = Math.Min(DataList.Count, MAX_RECORDS);
			int dataSize = numRecords * 12 + 12;
			dataSize = ((dataSize / 32)+1) * 32;
			byte[] buffer = new byte[dataSize];

			Array.Copy(SIG_PATTERN_BYTES, buffer, SIG_PATTERN_BYTES.Length);
			Array.Copy(BitConverter.GetBytes(numRecords), 0, buffer, 8, 4);

			if (DataList == null)
			{
				return buffer;
			}
			List<DMRDataItem> uploadList = new List<DMRDataItem>(DataList);// Need to make a copy so we can sort it and not screw up the list in the dataGridView
			uploadList.Sort();
			for (int i = 0; i < numRecords; i++)
			{
				Array.Copy(uploadList[i].getRadioData(rbtnName.Checked), 0, buffer, 0x0c + i * 12, 12);
			}
			return buffer;
		}

		private bool isInMemoryAccessMode(byte []buffer)
		{
			for (int i = 0; i < 0x20; i++)
			{
				if (buffer[i] != 00)
				{
					return true;
				}
			}
			return false;
		}

		private bool hasSig()
		{
			
			for (int i = 0; i < SIG_PATTERN_BYTES.Length; i++)
			{
				if (DMRIDForm.DMRIDBuffer[i] != SIG_PATTERN_BYTES[i])
				{
				return false;
				}
			}
			return true;
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			DataList = new List<DMRDataItem>();
			rebindData();
			//DataToCodeplug();

		}

		private void btnWriteToGD77_Click(object sender, EventArgs e)
		{
			MainForm.CommsBuffer = new byte[0x100000];// 128k buffer
			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataRead;
			CommPrgForm commPrgForm = new CommPrgForm(true);// true =  close download form as soon as download is complete
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			CodeplugComms.startAddress = 0x50100;
			CodeplugComms.transferLength = 0x20;
			DialogResult result = commPrgForm.ShowDialog();
			Array.Copy(MainForm.CommsBuffer, 0x50100, DMRIDForm.DMRIDBuffer, 0, 0x20);
			if (!isInMemoryAccessMode(DMRIDForm.DMRIDBuffer))
			{
				MessageBox.Show(Settings.dicCommon["EnableMemoryAccessMode"]); 
				return;
			}

			byte []uploadData = GenerateUploadData();
			Array.Copy(uploadData, 0, MainForm.CommsBuffer, 0x30000, (uploadData.Length/32)*32);
			CodeplugComms.CommunicationMode = CodeplugComms.CommunicationType.dataWrite;

			CodeplugComms.startAddress = 0x30000;
			CodeplugComms.transferLength = (uploadData.Length/32)*32;
			commPrgForm.StartPosition = FormStartPosition.CenterParent;
			result = commPrgForm.ShowDialog();
		}

		private void DMRIDFormNew_FormClosing(object sender, FormClosingEventArgs e)
		{
			//DataToCodeplug();
		}

		private void DMRIDForm_Load(object sender, EventArgs e)
		{
			Settings.smethod_59(base.Controls);
			Settings.smethod_68(this);// Update texts etc from language xml file
		}
	}
}

