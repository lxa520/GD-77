using GD77_FlashManager;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UsbLibrary;

internal class FirmwareUpdate : Win32Usb, IFirmwareUpdate
{
	public event FirmwareUpdateProgressEventHandler OnFirmwareUpdateProgress;

	[CompilerGenerated]
	public bool method_0()
	{
		return this.CCancelComm;
	}

    bool CCancelComm;
	[CompilerGenerated]
	public void method_1(bool bool_0)
	{
		this.CCancelComm = bool_0;
	}

    private bool _IsRead;
	[CompilerGenerated]
	public bool method_2()
	{
		return this._IsRead;
	}

	[CompilerGenerated]
	public void method_3(bool bool_0)
	{
		this._IsRead = bool_0;
	}

	public FirmwareUpdate()
	{
	}	
}
