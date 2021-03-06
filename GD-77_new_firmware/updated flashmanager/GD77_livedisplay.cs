﻿using System;
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
        private static readonly byte[] CMD_CMD3 = new byte[4] { (byte)'C', (byte)'M', (byte)'D', 3 };

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

        public bool send_data(SpecifiedDevice specifiedDevice, byte[] senddata)
        {
            specifiedDevice.SendData(senddata);
            byte[] usbBuf = new byte[160];// buffer for individual transfers
            Array.Clear(usbBuf, 0, usbBuf.Length);
            specifiedDevice.ReceiveData(usbBuf);// Wait for response
            if (usbBuf[0] != GD77_livedisplay.CMD_ACK[0])
            {
                SetStatusText("USB Livemodus ERROR: No ACK");
                return false;
            }
            return true;
        }

        public bool display_clear(SpecifiedDevice specifiedDevice)
        {
            SetCheckBox(checkBoxClearDisplay, false);
            if (!send_data(specifiedDevice, GD77_livedisplay.CMD_CMD3))
                return false;
            byte[] data = new byte[1] { 1 };
            if (!send_data(specifiedDevice, data))
                return false;
            return true;
        }

        public bool display_set_cursor(SpecifiedDevice specifiedDevice, byte x, byte y)
        {
            SetCheckBox(checkBoxClearDisplay, false);
            if (!send_data(specifiedDevice, GD77_livedisplay.CMD_CMD3))
                return false;
            byte[] data = new byte[3] { 2, x, y };
            if (!send_data(specifiedDevice, data))
                return false;
            return true;
        }

        public bool display_write(SpecifiedDevice specifiedDevice, char c)
        {
            SetCheckBox(checkBoxClearDisplay, false);
            if (!send_data(specifiedDevice, GD77_livedisplay.CMD_CMD3))
                return false;
            byte[] data = new byte[2] { 3, (byte)c };
            if (!send_data(specifiedDevice, data))
                return false;
            return true;
        }

        public bool display_print(SpecifiedDevice specifiedDevice, String text)
        {
            SetCheckBox(checkBoxClearDisplay, false);
            if (!send_data(specifiedDevice, GD77_livedisplay.CMD_CMD3))
                return false;
            byte[] data = new byte[31];
            data[0] = 4;
            int i = 0;
            while (i < 29)
            {
                data[i + 1] = (byte)text[i];
                i++;
                if (i >= text.Length)
                    break;
            }
            data[i + 1] = 0;
            if (!send_data(specifiedDevice, data))
                return false;
            return true;
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
                        if (!checkBoxEnableDisplayTest.Checked)
                        {
                            specifiedDevice.SendData(GD77_livedisplay.CMD_CMD2);
                            Array.Clear(usbBuf, 0, usbBuf.Length);
                            specifiedDevice.ReceiveData(usbBuf);// Wait for response
                            if (usbBuf[0] != GD77_livedisplay.CMD_ACK[0])
                            {
                                SetStatusText("USB Livemodus ERROR: No ACK");
                                break;
                            }

                            byte[] buffer = new byte[5] { 0, 0, 0, 0, 0 };
                            if (checkBoxLEDgreen.Checked)
                            {
                                buffer[0] |= 0x01;
                            }
                            if (checkBoxLEDred.Checked)
                            {
                                buffer[0] |= 0x02;
                            }
                            if (checkBoxDisplayLight.Checked)
                            {
                                buffer[0] |= 0x04;
                            }

                            specifiedDevice.SendData(buffer);
                            Array.Clear(usbBuf, 0, usbBuf.Length);
                            specifiedDevice.ReceiveData(usbBuf);

                            SetCheckBox(checkBoxPTT, (usbBuf[0] & 0x01) != 0);
                            SetCheckBox(checkBoxSK1, (usbBuf[0] & 0x02) != 0);
                            SetCheckBox(checkBoxSK2, (usbBuf[0] & 0x04) != 0);
                            SetCheckBox(checkBoxOrange, (usbBuf[0] & 0x08) != 0);

                            int keys = (usbBuf[1] << 0) + (usbBuf[2] << 8) + (usbBuf[3] << 16) + (usbBuf[4] << 24);

                            SetCheckBox(checkBoxKey1, (keys & 0x00000001) != 0);
                            SetCheckBox(checkBoxKey2, (keys & 0x00000002) != 0);
                            SetCheckBox(checkBoxKey3, (keys & 0x00000004) != 0);
                            SetCheckBox(checkBoxKeyGreen, (keys & 0x00000008) != 0);

                            SetCheckBox(checkBoxKeyRight, (keys & 0x00000010) != 0);
                            SetCheckBox(checkBoxKey4, (keys & 0x00000020) != 0);
                            SetCheckBox(checkBoxKey5, (keys & 0x00000040) != 0);
                            SetCheckBox(checkBoxKey6, (keys & 0x00000080) != 0);

                            SetCheckBox(checkBoxKeyUp, (keys & 0x00000100) != 0);
                            SetCheckBox(checkBoxKeyLeft, (keys & 0x00000200) != 0);
                            SetCheckBox(checkBoxKey7, (keys & 0x00000400) != 0);
                            SetCheckBox(checkBoxKey8, (keys & 0x00000800) != 0);

                            SetCheckBox(checkBoxKey9, (keys & 0x00001000) != 0);
                            SetCheckBox(checkBoxKeyDown, (keys & 0x00002000) != 0);
                            SetCheckBox(checkBoxKeyStar, (keys & 0x00008000) != 0);

                            SetCheckBox(checkBoxKey0, (keys & 0x00010000) != 0);
                            SetCheckBox(checkBoxKeyHash, (keys & 0x00020000) != 0);
                            SetCheckBox(checkBoxKeyRed, (keys & 0x00040000) != 0);
                        }
                        else
                        {
                            if (checkBoxClearDisplay.Checked)
                            {
                                SetCheckBox(checkBoxClearDisplay, false);

                                if (!display_clear(specifiedDevice))
                                    break;
                            }

                            if (checkBoxDisplayTest1.Checked)
                            {
                                SetCheckBox(checkBoxDisplayTest1, false);

                                if (!display_clear(specifiedDevice))
                                    break;

                                if (!display_set_cursor(specifiedDevice, 24, 1))
                                    break;
                                if (!display_write(specifiedDevice, 'T'))
                                    break;
                                if (!display_write(specifiedDevice, 'E'))
                                    break;
                                if (!display_write(specifiedDevice, 'S'))
                                    break;
                                if (!display_write(specifiedDevice, 'T'))
                                    break;
                                if (!display_write(specifiedDevice, '1'))
                                    break;

                                if (!display_set_cursor(specifiedDevice, 24, 2))
                                    break;
                                if (!display_write(specifiedDevice, 'T'))
                                    break;
                                if (!display_write(specifiedDevice, 'E'))
                                    break;
                                if (!display_write(specifiedDevice, 'S'))
                                    break;
                                if (!display_write(specifiedDevice, 'T'))
                                    break;
                                if (!display_write(specifiedDevice, '2'))
                                    break;

                                if (!display_set_cursor(specifiedDevice, 24, 4))
                                    break;
                                if (!display_print(specifiedDevice, "Testtext!"))
                                    break;

                                if (!display_set_cursor(specifiedDevice, 24, 5))
                                    break;
                                if (!display_print(specifiedDevice, "Hello World!"))
                                    break;
                            }

                            if (checkBoxDisplayTest2.Checked)
                            {
                                SetCheckBox(checkBoxDisplayTest2, false);

                                if (!display_clear(specifiedDevice))
                                    break;

                                for (int i = 0; i < 8; i++)
                                {
                                    if (!display_set_cursor(specifiedDevice, 0, (byte)i))
                                        break;
                                    if (!display_print(specifiedDevice, "012345678901234567890"))
                                        break;
                                }
                            }
                        }

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

        private void SetCheckBox(System.Windows.Forms.CheckBox cb, bool status)
        {
            cb.BeginInvoke(new Action(() =>
            {
                cb.Checked = status;
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
