namespace SysbotMemoryViewer
{
    public interface IValueComparison
    {
        bool CompareFloat(byte[] v1, byte[] v2);
        bool CompareDouble(byte[] v1, byte[] v2);
        bool CompareU8(byte v1, byte v2);
        bool CompareU8Array(byte[] v1, byte[] v2);
        bool CompareU16(byte[] v1, byte[] v2);
        bool CompareU32(byte[] v1, byte[] v2);
        bool CompareU64(byte[] v1, byte[] v2);
    }
}
