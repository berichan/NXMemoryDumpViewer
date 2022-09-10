namespace SysbotMemoryViewer
{
    public struct SearchRange
    {
        public ulong Start;
        public ulong End;
    }

    public interface ISearchHashmap
    {
        public int ValueSize { get; }
        public ulong ByteSize { get; }
        public List<SearchRange> SearcheableRanges { get; }
        public byte[]? GetBytesOfAddress(ulong address);

        /// <summary>
        /// Returns null if no range, a start and end value if exists
        /// </summary>
        public SearchRange? GetIsSearcheableRange(SearchRange range);

    }
}
