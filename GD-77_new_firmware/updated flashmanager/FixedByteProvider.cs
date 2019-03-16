using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Be.Windows.Forms;// for IByteProvider

namespace GD77_FlashManager
{
	public class FixedByteProvider : IByteProvider
	{

		bool _hasChanges;
		List<byte> _bytes;

		public FixedByteProvider(byte[] data)
			: this(new List<Byte>(data))
		{
		}

		public FixedByteProvider(List<Byte> bytes)
		{
			_bytes = bytes;
		}

		void OnChanged(EventArgs e)
		{
			_hasChanges = true;

			if (Changed != null)
				Changed(this, e);
		}

		void OnLengthChanged(EventArgs e)
		{
			if (LengthChanged != null)
				LengthChanged(this, e);
		}


		public List<Byte> Bytes
		{
			get { return _bytes; }
		}


		public bool HasChanges()
		{
			return _hasChanges;
		}

		public void ApplyChanges()
		{
			_hasChanges = false;
		}


		public event EventHandler Changed;

		public event EventHandler LengthChanged;

		public byte ReadByte(long index)
		{ return _bytes[(int)index]; }

		public void WriteByte(long index, byte value)
		{
			_bytes[(int)index] = value;
			OnChanged(EventArgs.Empty);
		}


		public void DeleteBytes(long index, long length)
		{

		}

		public void InsertBytes(long index, byte[] bs)
		{

		}

		public long Length
		{
			get
			{
				return _bytes.Count;
			}
		}

		public bool SupportsWriteByte()
		{
			return true;
		}


		public bool SupportsInsertBytes()
		{
			return false;
		}

		public bool SupportsDeleteBytes()
		{
			return false;
		}
	}
}
