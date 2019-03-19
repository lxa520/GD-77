using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsbLibrary;

namespace GD77_FlashManager
{
    public partial class GD77_livedisplay : Form
    {
        private const int HID_VID = 0x15A2;
        private const int HID_PID = 0x0073;

        private static readonly byte[] CMD_ACK = new byte[1] { (byte)'A' };
        private static readonly byte[] CMD_CMD1 = new byte[4] { (byte)'C', (byte)'M', (byte)'D', 1 };
        private static readonly byte[] CMD_CMD2 = new byte[4] { (byte)'C', (byte)'M', (byte)'D', 2 };

        private Thread thread;

        public GD77_livedisplay()
        {
            InitializeComponent();
        }

        bool usbCommTestRunning = false;
        bool usbCommTestStopTask = false;
        public void usbCommTest()
        {
            byte[] usbBuf = new byte[160];// buffer for individual transfers
            SpecifiedDevice specifiedDevice = null;
            try
            {
                specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);
                if (specifiedDevice == null)
                {
                    SetStatusText("USB Test ERROR: Device not found");
                }
                else
                {
                    SetStatusText("USB Test started");

                    for (int cnt = 0; cnt < 4000; cnt++)
                    {
                        specifiedDevice.SendData(GD77_livedisplay.CMD_CMD1);
                        Array.Clear(usbBuf, 0, usbBuf.Length);
                        specifiedDevice.ReceiveData(usbBuf);// Wait for response
                        if (usbBuf[0] != GD77_livedisplay.CMD_ACK[0])
                        {
                            SetStatusText("USB Test ERROR: No ACK");
                            break;
                        }

                        byte[] buffer = new byte[32];
                        for (int i = 0; i < 32; i++)
                        {
                            buffer[i] = (byte)i;
                        }

                        specifiedDevice.SendData(buffer);
                        Array.Clear(usbBuf, 0, usbBuf.Length);
                        specifiedDevice.ReceiveData(usbBuf);

                        bool error = false;
                        for (int i = 0; i < 32; i++)
                        {
                            if (usbBuf[i] != 255 - i)
                            {
                                error = true;
                                break;
                            }
                        }

                        if (error)
                        {
                            SetStatusText("USB Test ERROR: Received data does not match");
                            break;
                        }

                        SetStatusText(String.Format("USB Test: {0:0.0}%", (float)(cnt + 1) * 100 / (float)4000));

                        if (usbCommTestStopTask)
                        {
                            SetStatusText("USB Test canceled");
                            break;
                        }
                    }
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                SetStatusText("USB Test ERROR: Comms error");
            }
            finally
            {
                if (specifiedDevice != null)
                {
                    specifiedDevice.Dispose();
                }
                usbCommTestRunning = false;
            }
        }

        bool usbLivemodusRunning = false;
        bool usbLivemodusStopTask = false;
        public void usbLivemodus()
        {
            byte[] usbBuf = new byte[160];// buffer for individual transfers
            SpecifiedDevice specifiedDevice = null;
            try
            {
                specifiedDevice = SpecifiedDevice.FindSpecifiedDevice(HID_VID, HID_PID);
                if (specifiedDevice == null)
                {
                    SetStatusText("USB Livemodus ERROR: Device not found");
                }
                else
                {
                    SetStatusText("USB Livemodus started");

                    while (true)
                    {
                        specifiedDevice.SendData(GD77_livedisplay.CMD_CMD2);
                        Array.Clear(usbBuf, 0, usbBuf.Length);
                        specifiedDevice.ReceiveData(usbBuf);// Wait for response
                        if (usbBuf[0] != GD77_livedisplay.CMD_ACK[0])
                        {
                            SetStatusText("USB Livemodus ERROR: No ACK");
                            break;
                        }

                        byte[] buffer = new byte[1] { 0 };
                        if (checkBoxLEDgreen.Checked)
                        {
                            buffer[0] |= 0x01;
                        }
                        if (checkBoxLEDred.Checked)
                        {
                            buffer[0] |= 0x02;
                        }

                        specifiedDevice.SendData(buffer);
                        Array.Clear(usbBuf, 0, usbBuf.Length);
                        specifiedDevice.ReceiveData(usbBuf);

                        SetPTT((usbBuf[0] & 0x01) != 0);
                        SetSK1((usbBuf[0] & 0x02) != 0);
                        SetSK2((usbBuf[0] & 0x04) != 0);
                        SetOrange((usbBuf[0] & 0x08) != 0);

                        Thread.Sleep(50);

                        if (usbLivemodusStopTask)
                        {
                            SetStatusText("USB Livemodus stopped");
                            break;
                        }
                    }
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                SetStatusText("USB Livemodus ERROR: Comms error");
            }
            finally
            {
                if (specifiedDevice != null)
                {
                    specifiedDevice.Dispose();
                }
                usbLivemodusRunning = false;
            }
        }

        private void SetStatusText(String text)
        {
            labelStatusText.BeginInvoke(new Action(() =>
            {
                labelStatusText.Text = text;
            }));
        }

        private void SetPTT(bool status)
        {
            checkBoxPTT.BeginInvoke(new Action(() =>
            {
                checkBoxPTT.Checked = status;
            }));
        }

        private void SetSK1(bool status)
        {
            checkBoxSK1.BeginInvoke(new Action(() =>
            {
                checkBoxSK1.Checked = status;
            }));
        }

        private void SetSK2(bool status)
        {
            checkBoxSK2.BeginInvoke(new Action(() =>
            {
                checkBoxSK2.Checked = status;
            }));
        }

        private void SetOrange(bool status)
        {
            checkBoxOrange.BeginInvoke(new Action(() =>
            {
                checkBoxOrange.Checked = status;
            }));
        }

        private void buttonTestUSB_Click(object sender, EventArgs e)
        {
            if (!usbCommTestRunning && !usbLivemodusRunning)
            {
                usbCommTestRunning = true;
                usbCommTestStopTask = false;
                this.thread = new Thread(this.usbCommTest);
                this.thread.Start();
            }
            else
            {
                usbCommTestStopTask = true;
            }
        }

        private void buttonLivemodus_Click(object sender, EventArgs e)
        {
            if (!usbCommTestRunning && !usbLivemodusRunning)
            {
                usbLivemodusRunning = true;
                usbLivemodusStopTask = false;
                this.thread = new Thread(this.usbLivemodus);
                this.thread.Start();
            }
            else
            {
                usbLivemodusStopTask = true;
            }
        }

        private void GD77_livedisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (usbCommTestRunning)
            {
                usbCommTestStopTask = true;
                while (usbCommTestRunning)
                {
                    Thread.Sleep(50);
                }
            }
            if (usbLivemodusRunning)
            {
                usbLivemodusStopTask = true;
                while (usbLivemodusRunning)
                {
                    Thread.Sleep(50);
                }
            }
        }
    }
}
