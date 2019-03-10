using GD77_FlashManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UsbLibrary;

internal class CodeplugComms
{
	public delegate void Delegate1(object sender, FirmwareUpdateProgressEventArgs e);

	private const int HEAD_LEN = 4;
	private const int MAX_COMM_LEN = 32;
	private const byte CMD_WRITE = 87;
	private const byte CMD_READ = 82;
	private const byte CMD_CMD = 67;
	private const byte CMD_BASE = 66;
	private const int MaxReadTimeout = 5000;
	private const int MaxWriteTimeout = 1000;
	private const int MaxBuf = 160;
	private const float IndexListPercent = 5f;
	private const int HID_VID = 0x15A2;
	private const int HID_PID = 0x0073;
	private static readonly byte[] CMD_ENDR =   Encoding.ASCII.GetBytes("ENDR");
	private static readonly byte[] CMD_ENDW =   Encoding.ASCII.GetBytes("ENDW");
	private static readonly byte[] CMD_ACK=     new byte[1] {65};
    private static readonly byte[] CMD_PRG =    new byte[7] { 2, (byte)'P', (byte)'R', (byte)'O', (byte)'G', (byte)'R', (byte)'A' };// 80,82,79,71,82,65}
	private static readonly byte[] CMD_PRG2 =   new byte[2] {77,2};
    private static readonly byte[] CMD_PRG255 = new byte[2] { 77, 255 };


	public int[] START_ADDR;
	public int[] END_ADDR;

	private Thread thread;

	private Delegate1 OnFirmwareUpdateProgress;

    bool _CancelComm;
	[CompilerGenerated]
	public bool getCancelComm()
	{
		return this._CancelComm;
	}

	[CompilerGenerated]
	public void setCancelComm(bool bool_0)
	{
		this._CancelComm = bool_0;
	}

    bool _IsRead;

	[CompilerGenerated]
	public bool setIsRead()
	{
		return this._IsRead;
	}

	[CompilerGenerated]
	public void setIsRead(bool bool_0)
	{
		this._IsRead = bool_0;
	}

	public bool isThreadAlive()
	{
		if (this.thread != null)
		{
			return this.thread.IsAlive;
		}
		return false;
	}

	public void joinThreadIfAlive()
	{
		if (this.isThreadAlive())
		{
			this.thread.Join();
		}
	}

	public void startCodeplugReadOrWriteInNewThread()
	{
		if (this.setIsRead())
		{
            if (!MainForm.readInternalFlash)
            {
                this.thread = new Thread(this.readData);
            }
            else
            {
                MessageBox.Show("Reading from the internal flash will only work if the properly patched flash readout firmware has already been uploaded!");
                this.thread = new Thread(this.readDataFlash);
            }
		}
		else
		{
			this.thread = new Thread(this.writeData);
		}
		this.thread.Start();
	}

	public static string ByteArrayToString(byte[] ba)
	{
		string hex = BitConverter.ToString(ba);
		return hex.Replace("-", "");
	}

	// Test function added by Roger Clark to read all 1Mb from the external Flash chip on the GD-77.
	// It outputs the data dummp to the file c:\\gd-77_datadump.bin
	
	// However depending on the mode in which the GD-77 is booted,  addresses 0x000000 - 0x01FFFF  (for codeplug mode)
	// or addresses 0x030000 - 0x03FFFF for DMR-ID mode are returned with anything other than 0x00 in them

	// Its strange that address 0x020000 - 0x02FFFF does not seem to be accessible
	// Note. Valid transfer lengths seem to be 8,16 or 32 bytes.  64 bytes does not work, as it just returns 0x00 in addresses after 32 bytes

    public void readDataFlash()
    {
        byte[] usbBuf = new byte[160];// buffer for individual transfers
        int blockLength = 16;
        SpecifiedDevice specifiedDevice = null;
        try
        {
            specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);//0x152A HID_PID
            if (specifiedDevice == null)
            {
                if (this.OnFirmwareUpdateProgress != null)
                {
                    this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Device not found", true, true));
                }
            }
            else
            {
                int numBlocks = MainForm.transferLength / blockLength;
                int startBlock = MainForm.startAddress / blockLength;
                byte[] flashreadcommand = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x00 }; // CMD (1=read, otherwise stop) + Address as LSB to MSB
                int block_counter = 0;
                for (int block = startBlock; block < (startBlock + numBlocks); block++)
                {
                    if (block_counter == 0)
                    {
                        Array.Clear(usbBuf, 0, usbBuf.Length);
                        specifiedDevice.SendData(CodeplugComms.CMD_PRG);// Send PROGRA command to initiate comms
                        specifiedDevice.ReceiveData(usbBuf);// Wait for response
                        if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
                        {
                            if (this.OnFirmwareUpdateProgress != null)
                            {
                                this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "No ACK1", true, true));
                            }
                            break;// Exit if not ack
                        }

                        Array.Clear(usbBuf, 0, usbBuf.Length);
                        specifiedDevice.SendData(CodeplugComms.CMD_PRG255);// Send command to initiate flash readout
                        specifiedDevice.ReceiveData(usbBuf);// Wait for response
                        if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
                        {
                            if (this.OnFirmwareUpdateProgress != null)
                            {
                                this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "No ACK2", true, true));
                            }
                            break;// Exit if not ack
                        }
                    }

                    ulong adr = (ulong)(block * 16);
                    flashreadcommand[0] = (byte)1;                                          // READ
                    flashreadcommand[1] = (byte)((adr & 0x000000ff) >> 0);
                    flashreadcommand[2] = (byte)((adr & 0x0000ff00) >> 8);
                    flashreadcommand[3] = (byte)((adr & 0x00ff0000) >> 16);
                    flashreadcommand[4] = (byte)((adr & 0xff000000) >> 24);
                    specifiedDevice.SendData(flashreadcommand);
                    Array.Clear(usbBuf, 0, usbBuf.Length);
                    specifiedDevice.ReceiveData(usbBuf);

                    Buffer.BlockCopy(usbBuf, 0, MainForm.CommsBuffer, (block * blockLength), blockLength);// Extract the 16 bytes from the response

                    if (this.OnFirmwareUpdateProgress != null)
                    {
                        this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(block + 1 - startBlock) * 100 / (float)numBlocks, "", false, false));
                    }

                    block_counter++;
                    if (block_counter>=256)
                    {
                        block_counter = 0;

                        flashreadcommand[0] = (byte)2;                                      // STOP
                        flashreadcommand[1] = (byte)0;
                        flashreadcommand[2] = (byte)0;
                        flashreadcommand[3] = (byte)0;
                        flashreadcommand[4] = (byte)0;
                        specifiedDevice.SendData(flashreadcommand);
                        specifiedDevice.ReceiveData(usbBuf);// Wait for response
                        if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
                        {
                            if (this.OnFirmwareUpdateProgress != null)
                            {
                                this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "No STOPACK1", true, true));
                            }
                            break;// Exit if not ack
                        }
                    }

                    if (getCancelComm())
                    {
                        break;
                    }
                }

                if (block_counter != 0)
                {
                    flashreadcommand[0] = (byte)2;                                      // FINAL STOP
                    flashreadcommand[1] = (byte)0;
                    flashreadcommand[2] = (byte)0;
                    flashreadcommand[3] = (byte)0;
                    flashreadcommand[4] = (byte)0;
                    specifiedDevice.SendData(flashreadcommand);
                    specifiedDevice.ReceiveData(usbBuf);// Wait for response
                    if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
                    {
                        if (this.OnFirmwareUpdateProgress != null)
                        {
                            this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "No STOPACK2", true, true));
                        }
                    }
                }
            }
        }
        catch (TimeoutException ex)
        {
            Console.WriteLine(ex.Message);
            if (this.OnFirmwareUpdateProgress != null)
            {
                this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Comms error", true, true));
            }
        }
        finally
        {
            if (specifiedDevice != null)
            {
                specifiedDevice.Dispose();
            }
        }

        if (this.OnFirmwareUpdateProgress != null)
        {
            this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, "", false, true));
        }
    }

	public void readData()
	{
		byte[] usbBuf = new byte[160];// buffer for individual transfers
		int blockLength = 0;
		int addr32 = 0;
		int addr16 = 0;
		int pageAddr = 0;
		SpecifiedDevice specifiedDevice = null;
		try
		{
			specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);//0x152A HID_PID
			if (specifiedDevice == null)
			{
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Device not found", true, true));
				}
			}
			else
			{
				while (true)
				{
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.SendData(CodeplugComms.CMD_PRG);// Send PROGRA command to initiate comms
					specifiedDevice.ReceiveData(usbBuf);// Wait for response
					if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
					{
						break;// Exit if not ack
					}
					specifiedDevice.SendData(CodeplugComms.CMD_PRG2);// Send second half of comms init sequence
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.ReceiveData(usbBuf);// GD77 send back device information
					byte[] array3 = new byte[8];
					Buffer.BlockCopy(usbBuf, 0, array3, 0, 8);// Extract the first 8 bytes from the response
					// REMOVED CURRENT MODEL CHECK !!!  if (array3.smethod_4(Settings.CUR_MODEL))
					{
						// its the correct model number
						specifiedDevice.SendData(CodeplugComms.CMD_ACK);// send ACK
						Array.Clear(usbBuf, 0, usbBuf.Length);
						specifiedDevice.ReceiveData(usbBuf);// Wait for response (of ACK)


						if (usbBuf[0] == CodeplugComms.CMD_ACK[0])
						{
							// --------------- removed the password checking
							blockLength = 32;// Max transfer length is 32 bytes
							int currentPage = 0;
							int bankSize = 65536;
							int numBlocks = MainForm.transferLength / blockLength;
							int startBlock = MainForm.startAddress / blockLength;
							for (int block = startBlock; block < (startBlock + numBlocks); block++)
							{
								if (currentPage != (block * blockLength) / bankSize)
								{
									currentPage = (block * blockLength) / bankSize;
									addr32 = blockLength * block;
									byte[] array4 = new byte[8] { (byte)'C', (byte)'W', (byte)'B', 4, 0, 0, 0, 0 };
									pageAddr = addr32 >> 16 << 16;
									array4[4] = (byte)(pageAddr >> 24);
									array4[5] = (byte)(pageAddr >> 16);
									array4[6] = (byte)(pageAddr >> 8);
									array4[7] = (byte)pageAddr;
									Console.WriteLine(SpecifiedDevice.ByteArrayToString(array4));
									Array.Clear(usbBuf, 0, usbBuf.Length);
									specifiedDevice.SendData(array4, 0, array4.Length);
									specifiedDevice.ReceiveData(usbBuf);
									if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
									{
										goto end_IL_02a2;
									}
								}

								addr16 = (block * blockLength) & 0xffff;
								// Send request for dcata
								byte[] data2 = new byte[4] { 82, (byte)(addr16 >> 8), (byte)addr16, (byte)blockLength };
								
								Array.Clear(usbBuf, 0, usbBuf.Length);
								specifiedDevice.SendData(data2, 0, 4);
								if (!specifiedDevice.ReceiveData(usbBuf))
								{
									goto end_IL_02a2;
								}

								Buffer.BlockCopy(usbBuf, 4, MainForm.CommsBuffer, (block * blockLength), blockLength);// Extract the first 8 bytes from the response

								if (this.OnFirmwareUpdateProgress != null)
								{
									this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(block+1-startBlock) *100 / (float)numBlocks, "", false, false));
								}
							}
							// SEND END OF READ
							specifiedDevice.SendData(CodeplugComms.CMD_ENDR);
							specifiedDevice.ReceiveData(usbBuf);
						}
						break;
					}
					return;
				end_IL_02a2:
					break;
				}

			}
		}
		catch (TimeoutException ex)
		{
			Console.WriteLine(ex.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Comms error", false, false));
			}
		}
		finally
		{
			if (specifiedDevice != null)
			{
				specifiedDevice.Dispose();
			}
		}

		if (this.OnFirmwareUpdateProgress != null)
		{
			this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, "", false, true));
		}
	}


	public void writeData()
	{
		//byte[] MainForm.eeprom;// no need to allocate this as its read in from file. = new byte[128 * 1024];// whole of the codeplug
		byte[] usbBuf = new byte[160];// buffer for individual usb transfers

		int blockLength = 0;
		int addr32 = 0;
		int addr16 = 0;
		int pageAddr = 0;
		SpecifiedDevice specifiedDevice = null;
		try
		{
			specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);//0x152A HID_PID
			if (specifiedDevice == null)
			{
				if (this.OnFirmwareUpdateProgress != null)
				{
					this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Device not found", true, true));
				}
			}
			else
			{
				while (true)
				{
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.SendData(CodeplugComms.CMD_PRG);// Send PROGRA command to initiate comms
					//Console.WriteLine("Send PRG1");
					specifiedDevice.ReceiveData(usbBuf);// Wait for response
					if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
					{
						break;// Exit if not ack
					}
					specifiedDevice.SendData(CodeplugComms.CMD_PRG2);// Send second half of comms init sequence
					//Console.WriteLine("Send PRG2");
					Array.Clear(usbBuf, 0, usbBuf.Length);
					specifiedDevice.ReceiveData(usbBuf);// GD77 send back device information
					byte[] array3 = new byte[8];
					Buffer.BlockCopy(usbBuf, 0, array3, 0, 8);// Extract the first 8 bytes from the response
					{
						// its the correct model number
						specifiedDevice.SendData(CodeplugComms.CMD_ACK);// send ACK
						Array.Clear(usbBuf, 0, usbBuf.Length);
						specifiedDevice.ReceiveData(usbBuf);// Wait for response (of ACK)

						if (usbBuf[0] == CodeplugComms.CMD_ACK[0])
						{
							//Console.WriteLine("Got ACK");
							// --------------- removed the password checking
							blockLength = 32;// Max transfer length is 32 bytes
							int currentPage = 0;
							int bankSize = 65536;
							int numBlocks = MainForm.transferLength / blockLength;
							int startBlock = MainForm.startAddress / blockLength;
							for (int block = startBlock; block < (startBlock + numBlocks); block++)
							{
								//Console.Write("Processing block " + block + " end block " + (startBlock + numBlocks));
								if (currentPage != (block * blockLength) / bankSize)
								{
									currentPage = (block * blockLength) / bankSize;
									addr32 = blockLength * block;
									byte[] array4 = new byte[8] { (byte)'C', (byte)'W', (byte)'B', 4, 0, 0, 0, 0 };
									pageAddr = addr32 >> 16 << 16;
									array4[4] = (byte)(pageAddr >> 24);
									array4[5] = (byte)(pageAddr >> 16);
									array4[6] = (byte)(pageAddr >> 8);
									array4[7] = (byte)pageAddr;
									//Console.WriteLine("Send address changed to 0x" + pageAddr.ToString("X"));
									Array.Clear(usbBuf, 0, usbBuf.Length);
									specifiedDevice.SendData(array4, 0, array4.Length);
									specifiedDevice.ReceiveData(usbBuf);
									if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
									{
										goto end_IL_02a2;
									}
								}

								addr16 = (block * blockLength) & 0xffff;
								// Send request for dcata
								byte[] data2 = new byte[4+32] { 87, (byte)(addr16 >> 8), (byte)addr16, (byte)blockLength,
								0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

								Array.Clear(usbBuf, 0, usbBuf.Length);
								Buffer.BlockCopy(MainForm.CommsBuffer, (block * blockLength), data2, 4, blockLength);
								specifiedDevice.SendData(data2, 0, 4+32);
								//Console.WriteLine("Send Data to address 0x"+addr16.ToString("X"));

								specifiedDevice.ReceiveData(usbBuf);
								if (usbBuf[0] != CodeplugComms.CMD_ACK[0])
								{
									goto end_IL_02a2;
								}
								if (this.OnFirmwareUpdateProgress != null)
								{
									this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs((float)(block + 1 - startBlock) * 100 / (float)numBlocks, "", false, false));
								}
							}
							// SEND END OF WRITE
							specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
							specifiedDevice.ReceiveData(usbBuf);
						}
						break;
					}
					return;
				end_IL_02a2:
					// SEND END OF WRITE
					specifiedDevice.SendData(CodeplugComms.CMD_ENDW);
					specifiedDevice.ReceiveData(usbBuf);
					break;
				}
			}
		}
		catch (TimeoutException ex)
		{
			Console.WriteLine(ex.Message);
			if (this.OnFirmwareUpdateProgress != null)
			{
				this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(0f, "Error", false, false));
			}
		}
		finally
		{
			if (specifiedDevice != null)
			{
				specifiedDevice.Dispose();
			}
		}

		if (this.OnFirmwareUpdateProgress != null)
		{
			this.OnFirmwareUpdateProgress(this, new FirmwareUpdateProgressEventArgs(100f, "", false, true));
		}
	}



	
	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_9(Delegate1 delegate1_0)
	{
		this.OnFirmwareUpdateProgress = (Delegate1)Delegate.Combine(this.OnFirmwareUpdateProgress, delegate1_0);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void method_10(Delegate1 delegate1_0)
	{
		this.OnFirmwareUpdateProgress = (Delegate1)Delegate.Remove(this.OnFirmwareUpdateProgress, delegate1_0);
	}

	public CodeplugComms()
	{
		this.START_ADDR = new int[0];
		this.END_ADDR = new int[0];
	}
}
