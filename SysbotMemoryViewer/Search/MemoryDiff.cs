using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysbotMemoryViewer
{
    public class MemoryDiff
    {
        public int ValueSize { get; set; }

        public long Size => readWriter.Length;
        public int LineSize => fetchSize;
        public long TotalObjectSize => Size / LineSize;

        private string diffFilePath;
        private FileStream readWriter;
        private int fetchSize;
        private ProgressBar progressBar;

        public MemoryDiff(string path, string filename, int valueSize, ProgressBar pb)
        {
            ValueSize = valueSize;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            diffFilePath = Path.Combine(path, filename);

            if (File.Exists(diffFilePath))
                File.Delete(diffFilePath);

            readWriter = new FileStream(diffFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fetchSize = sizeof(ulong) + valueSize;
            progressBar = pb;
        }

        ~MemoryDiff()
        {
            if (readWriter != null)
                readWriter.Close();
        }

        public void WriteNewDiff(ulong addr, byte[] data)
        {
            readWriter.Write(BitConverter.GetBytes(addr).Concat(data).ToArray());
        }

        public void ResetPointer()
        {
            readWriter.Seek(0, SeekOrigin.Begin);
        }

        public void SetPointer(long valuePos)
        {
            var offs = valuePos * LineSize;
            if (offs < Size)
                readWriter.Seek(offs, SeekOrigin.Begin);
        }

        public KeyValuePair<ulong, byte[]>? ReadNext()
        {
            if (readWriter.Position >= readWriter.Length)
                return null;
            byte[] data = new byte[fetchSize];
            readWriter.Read(data);
            //readWriter.Position += fetchSize;
            return new(BitConverter.ToUInt64(data, 0), data[8..]);
        }

        public static MemoryDiff GenerateMemoryDiff(string path, string name, ProgressBar pb, ISearchHashmap a, ISearchHashmap b, Func<byte[], byte[], bool> comparer)
        {
            var memDiff = new MemoryDiff(path, name, a.ValueSize, pb);
            int size = a.ValueSize;

            for (int i = 0; i < a.SearcheableRanges.Count; ++i)
            {
                var searchRange = a.SearcheableRanges[i];
                var searchSize = searchRange.End - searchRange.Start;

                pb.Minimum = 0;
                pb.Maximum = 100;
                pb.Value = 0;
                pb.Step = 1;

                var pc1 = Math.Max(1, searchSize / 100);

                for (ulong j = 0; j < searchSize; j += (ulong)size)
                {
                    var byteReq = new SearchRange() { Start = searchRange.Start + j, End = searchRange.Start + j + (ulong)size };
                    var sr = b.GetIsSearcheableRange(byteReq);
                    if (sr != null)
                    {
                        var srV = sr.Value;
                        var aB = a.GetBytesOfAddress(srV.Start);
                        var bB = b.GetBytesOfAddress(srV.Start);
                        if (aB != null && bB != null && comparer(aB, bB))
                            memDiff.WriteNewDiff(srV.Start, bB);
                    }

                    if (j % pc1 == 0)
                    {
                        pb.PerformStep();
                    }
                }
            }

            return memDiff;
        }

        public static MemoryDiff GenerateMemoryDiff(string path, string name, ProgressBar pb, ISearchHashmap a, byte[] b, Func<byte[], byte[], bool> comparer)
        {
            var memDiff = new MemoryDiff(path, name, a.ValueSize, pb);

            for (int i = 0; i < a.SearcheableRanges.Count; ++i)
            {
                var range = a.SearcheableRanges[i];

                var searchSize = range.End - range.Start;

                pb.Minimum = 0;
                pb.Maximum = 100;
                pb.Value = 0;
                pb.Step = 1;

                var pc1 = Math.Max(1, searchSize / 100);

                for (ulong j = range.Start; j < range.End; j += (ulong)b.Length)
                {
                    var bts = a.GetBytesOfAddress(j);
                    if (bts != null && comparer(bts, b))
                        memDiff.WriteNewDiff(j, bts);

                    if (j % pc1 == 0)
                    {
                        pb.PerformStep();
                    }
                }
            }

            return memDiff;
        }

        public static MemoryDiff GenerateMemoryDiff(string path, string name, ProgressBar pb, MemoryDiff a, byte[] b, Func<byte[], byte[], bool> comparer)
        {
            var memDiff = new MemoryDiff(path, name, a.ValueSize, pb);
            a.ResetPointer();
            var read = a.ReadNext();
            while (read != null)
            {
                var kvp = read.Value;
                if (comparer(kvp.Value, b))
                    memDiff.WriteNewDiff(kvp.Key, kvp.Value);

                read = a.ReadNext();
            }

            return memDiff;
        }

        public static MemoryDiff GenerateMemoryDiff(string path, string name, ProgressBar pb, MemoryDiff a, MemoryDiff b, Func<byte[], byte[], bool> comparer)
        {
            if (a.Size != b.Size)
                throw new Exception("Cannot diff diffs of different sizes.");
            var memDiff = new MemoryDiff(path, name, a.ValueSize, pb);

            a.ResetPointer();
            b.ResetPointer();

            var reada = a.ReadNext();
            var readb = b.ReadNext();
            while (reada != null && readb != null)
            {
                var kvpa = reada.Value;
                var kvpb = readb.Value;
                Debug.Assert(kvpa.Key.Equals(kvpb.Key));
                if (comparer(kvpa.Value, kvpb.Value))
                    memDiff.WriteNewDiff(kvpb.Key, kvpb.Value);

                reada = a.ReadNext();
                readb = b.ReadNext();
            }

            return memDiff;
        }
    }
}
