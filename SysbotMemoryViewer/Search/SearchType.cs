namespace SysbotMemoryViewer
{
    public enum SearchType
    {
        U8,
        U16,
        U32,
        U64,
        FLT,
        DBL
    }

    public static class SearchTypeExtensions
    {
        public static int SearchTypeToByteLength(this SearchType st) => 
        st switch
        {
            SearchType.U8 => 1,
            SearchType.U16 => 2,
            SearchType.U32 or SearchType.FLT => 4,
            SearchType.U64 or SearchType.DBL => 8,
            _ => -1,
        };
        
    }
}
