using System;
using System.Collections.Generic;
using Mono.Collections.Generic;

namespace Mono.Cecil.PE
{
	public class ResourceEntry
	{
		public uint Id { get; set; }
		public string Name { get; set; }
		public ResourceDirectory Directory { get; set; }
		public uint CodePage { get; set; }
		public uint Reserved { get; set; }
		public byte[] Data { get; set; }
	}

	public class ResourceDirectory
	{
		private readonly Collection<ResourceEntry> _entries = new Collection<ResourceEntry>();

		public Collection<ResourceEntry> Entries
		{
			get
			{
				return _entries;
			}
		}

		public ushort SortEntries()
		{
			_entries.Sort(EntryComparer.Instance);
			for (ushort i = 0; i < _entries.Count; i++)
				if (_entries[i].Name == null)
					return i;
			return 0;
		}

		public ushort NumNameEntries { get; set; }
		public ushort NumIdEntries { get; set; }
		public ushort MinVersion { get; set; }
		public ushort MajorVersion { get; set; }
		public uint Characteristics { get; set; }
		public uint TimeDateStamp { get; set; }
	}

	class EntryComparer : IComparer<ResourceEntry>
	{
		internal static readonly EntryComparer Instance = new EntryComparer();
		public int Compare(ResourceEntry x, ResourceEntry y)
		{
			if (x.Name != null && y.Name == null)
				return -1;
			if (x.Name == null && y.Name != null)
				return 1;
			if (x.Name == null)
				return (int)(x.Id - y.Id);
			return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
		}
	}
}