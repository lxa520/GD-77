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

        private void SetStatusText(String text)
        {
            labelStatusText.BeginInvoke(new Action(() =>
            {
                labelStatusText.Text = text;
            }));
        }

        private void buttonTestUSB_Click(object sender, EventArgs e)
        {
            if (!usbCommTestRunning)
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
        }
    }
}
