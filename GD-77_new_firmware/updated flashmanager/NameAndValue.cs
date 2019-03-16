using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD77_FlashManager
{
	public class NameAndValue
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public NameAndValue()
		{
			Name = "";
			Value = 0;
		}
		public NameAndValue(string name, int value)
		{
			Name = name;
			Value = value;
		}
	}
}
