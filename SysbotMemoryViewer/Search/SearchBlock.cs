using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysbotMemoryViewer
{
    public class SearchBlock : ISearchHashmap
    {
        public readonly Dictionary<ulong, byte[]> AddressValues = new();

        public List<SearchRange> SearcheableRanges { get; }
        public int ValueSize { get; }
        public ulong ByteSize { get; }

        public SearchBlock(Dictionary<ulong, byte[]> addressValues, int vSize)
        {
            AddressValues = addressValues;
            SearcheableRanges = new List<SearchRange>();
            foreach (var av in addressValues.Keys)
                SearcheableRanges.Add(new SearchRange() { Start = av, End = av + (ulong)ValueSize });

            ValueSize = vSize;
            ByteSize = (ulong)(addressValues.Count * ValueSize);
        }

        public byte[]? GetBytesOfAddress(ulong address) // fixed alignment
        {
            if (AddressValues.ContainsKey(address))
                return AddressValues[address];
            return null;
        }

        public SearchRange? GetIsSearcheableRange(SearchRange range)
        {
            var addresses = AddressValues.Keys.Where(x => x >= range.Start && x <= range.End);
            if (!addresses.Any())
                return null;

            var sorted = addresses.OrderBy(x => x);
            return new SearchRange()
            {
                Start = sorted.First(),
                End = sorted.Last()
            };
        }
    }
}
