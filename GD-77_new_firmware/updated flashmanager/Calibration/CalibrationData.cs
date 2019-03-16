using System;
using System.Runtime.InteropServices;

namespace GD77_FlashManager
{

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class CalibrationData
	{
		public UInt16 RxGain; // offset 0x00
		public UInt16 TxGainUnconfirmed;// offset 0x02

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] UnknownBlock1;// offset 0x04
		

		public UInt16 DACOscRefTune;// offset 0x08 	DAC word for frequency reference oscillator

		public byte UnknownBlock2; // offset 0x0A   Unkown byte E9 on UHF EE on VHF

		/* Power settings
		 * UHF 400 to 475 in 5Mhz stps (16 steps)
		 * VHF 136Mhz, then 140MHz -  165Mhz in steps of 5Mhz, then 172Mhz  (8 steps - upper 8 array entries contain 0xff )
		 */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] 
		public PowerSettingData[] PowerSettings;// Offset 0x0B (Note. Not all used on VHF)


		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
		public byte[] UnknownBlock3;// Offset 0x2B


		//  Analog Squelch controls
		public byte SquelchSensitivity;// offset 0x46

		public byte MuteStrictWidebandClose1;// offset 0x46
		public byte MuteStrictWidebandOpen1;// offset 0x48

		public UInt16 Unknown4;// offset 0x49

		public byte MuteNormalWidebandClose1;// offset 0x4B
		public byte MuteNormalWidebandOpen1;// offset 0x4C

		public byte MuteStrictNarrowbandClose1;// offset 0x4D
		public byte MuteStrictNarrowbandOpen1;// offset 0x4E

		public UInt16 Uknown5;//Offset 0x4F - 0x50

		public byte MuteNormalNarrowbandClose1;// Offset 0x51
		public byte MuteNormalNarrowbandOpen1;// Offset 0x52

		public byte RSSILowerThreshold;// Offset 0x53
		public byte RSSIUpperThreshold;// Offset 0x54

		/*
		 * VHF 136Mhz , 140Mhz - 165Mhz (in 5Mhz steps), 172Mhz 
		 * UHF 405Mhz - 475Mhz (in 10Mhz steps)
		 */
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] TXIandQ;// Don't adjust

		public byte DigitalRxAudioGainAndBeepVolume;// The Rx audio gain and the beep volume seem linked together.  0x1D on VHF and UHF

		public byte AnalogTxDeviationDTMF;
		public byte AnalogTxDeviation1750Toneburst;
		public byte AnalogTxDeviationCTCSSWideband;
		public byte AnalogTxDeviationCTCSSNarrowband;
		public byte AnalogTxDeviationDCSWideband;
		public byte AnalogTxDeviationDCSNarrowband;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] UnknownBlock8;

		public byte AnalogMicGain;// Both wide and narrow band
		public byte ReceiveAGCGainTarget; // Receiver AGC target. Higher values give more gain. Reducing this may improve receiver overload with strong signals, but would reduce sensitivity

		public UInt16 AnalogTxOverallDeviationWideband;// CTCSS, DCS, DTMF & voice, deviation .Normally a very low value like 0x0027
		public UInt16 AnalogTxOverallDeviationNarrband;// CTCSS, DCS, DTMF & voice, deviation .Normally a very low value like 0x0027
		
		// Not sure why there are 2 of these and what the difference is.
		public byte AnalogRxAudioGainWideband;// normally a 0x0F
		public byte AnalogRxAudioGainNarrowband;// normally a 0x0F

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] UnknownBlock7;

		public CalibrationData()
		{
			/* Superseded
			 * this.UnknownBlock1 = new byte[8];
			 */
			this.PowerSettings = new PowerSettingData[16];
			for (int i = 0; i < 16; i++)
			{
				PowerSettings[i] = new PowerSettingData();
			}
			this.UnknownBlock1 = new byte[4];
			this.UnknownBlock3 = new byte[27];

			this.TXIandQ = new byte[8];
			this.UnknownBlock7 = new byte[2];
			this.UnknownBlock8 = new byte[2];
			
		}

	}
}
