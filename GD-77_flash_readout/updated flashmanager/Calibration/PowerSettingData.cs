using System;
using System.Runtime.InteropServices;

namespace GD77_FlashManager
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PowerSettingData
	{
		public byte lowPower;
		public byte highPower;
	}
}
