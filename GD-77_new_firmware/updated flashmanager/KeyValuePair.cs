using System;

namespace GD77_FlashManager
{
	[Serializable]
	public struct KeyValuePair<K, V>
	{
		public K Key { get; set; }
		public V Value { get; set; }
	}
}
