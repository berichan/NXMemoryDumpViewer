using System.Runtime.CompilerServices;

namespace SysbotMemoryViewer
{
    public class GreaterThan : IValueComparison
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CompareDouble(byte[] v1, byte[] v2)
        {
            var a = BitConverter.ToDouble(v1);
            var b = BitConverter.ToDouble(v2);
            return !double.IsNaN(a) && !double.IsNaN(b) && a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CompareFloat(byte[] v1, byte[] v2)
        {
            var a = BitConverter.ToSingle(v1);
            var b = BitConverter.ToSingle(v2);
            return !float.IsNaN(a) && !float.IsNaN(b) && a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CompareU16(byte[] v1, byte[] v2) => BitConverter.ToUInt16(v1) > BitConverter.ToUInt16(v2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CompareU32(byte[] v1, byte[] v2) => BitConverter.ToUInt32(v1) > BitConverter.ToUInt32(v2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CompareU64(byte[] v1, byte[] v2) => BitConverter.ToUInt64(v1) > BitConverter.ToUInt64(v2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CompareU8(byte v1, byte v2) => v1 > v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CompareU8Array(byte[] v1, byte[] v2) => !v1.SequenceEqual(v2);
    }
}
