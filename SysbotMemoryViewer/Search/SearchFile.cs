using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SysbotMemoryViewer
{
    public class SearchFile : ISearchHashmap
    {
        private const string InfoLineMainCpu = "mainCpu=";
        private const string InfoLineHeapCpu = "heapCpu=";
        private const string InfoLineMain = "main=";
        private const string InfoLineHeap = "heap=";

        private BinaryReader reader;
        private string initialPath;

        private ulong mainAddrStart, mainAddrEnd;
        private ulong heapAddrStart, heapAddrEnd;
        private ulong mainFileStart, mainFileEnd;
        private ulong heapFileStart, heapFileEnd;
        private ulong mainSize;
        private ulong heapSize;

        public List<SearchRange> SearcheableRanges { get; }

        public int ValueSize { get; set; }
        public ulong ByteSize { get; }

        public SearchFile(string initialDumpPath, int valueSize)
        {
            ValueSize = valueSize;

            initialPath = initialDumpPath;
            reader = new BinaryReader(new FileStream(initialPath + ".dmp", FileMode.Open, FileAccess.Read));

            // pop addresses
            var infoText = File.ReadAllText(initialPath + ".txt");
            var infoLines = infoText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var mainCpuLine = infoLines.First(x => x.StartsWith(InfoLineMainCpu));
            var mainSplit = mainCpuLine[InfoLineMainCpu.Length..].Split(",");
            mainAddrStart = ulong.Parse(mainSplit[0], System.Globalization.NumberStyles.HexNumber);
            mainAddrEnd = ulong.Parse(mainSplit[1], System.Globalization.NumberStyles.HexNumber);

            var heapCpuLine = infoLines.First(x => x.StartsWith(InfoLineHeapCpu));
            var heapSplit = heapCpuLine[InfoLineHeapCpu.Length..].Split(",");
            heapAddrStart = ulong.Parse(heapSplit[0], System.Globalization.NumberStyles.HexNumber);
            heapAddrEnd = ulong.Parse(heapSplit[1], System.Globalization.NumberStyles.HexNumber);

            mainSize = mainAddrEnd - mainAddrStart;
            heapSize = heapAddrEnd - heapAddrStart;

            var mainFileLine = infoLines.First(x => x.StartsWith(InfoLineMain));
            var mfSplit = mainFileLine[InfoLineMain.Length..];
            mainFileStart = ulong.Parse(mfSplit, System.Globalization.NumberStyles.HexNumber);
            mainFileEnd = mainFileStart + mainSize;

            var heapFileLine = infoLines.First(x => x.StartsWith(InfoLineHeap));
            var hfSplit = heapFileLine[InfoLineHeap.Length..];
            heapFileStart = ulong.Parse(hfSplit, System.Globalization.NumberStyles.HexNumber);
            heapFileEnd = heapFileStart + heapSize;

            SearcheableRanges = new()
            {
                new SearchRange()
                {
                    Start = mainAddrStart,
                    End = mainAddrEnd
                },
                new SearchRange()
                {
                    Start = heapAddrStart,
                    End = heapAddrEnd
                }
            };

            ByteSize = mainSize + heapSize;
        }

        ~SearchFile()
        {
            if (reader != null)
                reader.Close();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetBytes(ulong fp, int size)
        {
            reader.BaseStream.Seek((long)fp, SeekOrigin.Begin);
            return reader.ReadBytes(size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool isMainAddress(ulong addr) => addr >= mainAddrStart && addr <= mainAddrEnd;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool isHeapAddress(ulong addr) => addr >= heapAddrStart && addr <= heapAddrEnd;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[]? GetBytesOfAddress(ulong address)
        {
            ulong ptr;

            if (isMainAddress(address))
                ptr = getPtrOfMainAddr(address);
            else if (isHeapAddress(address))
                ptr = getPtrOfHeapAddr(address);
            else
                return null;
            reader.BaseStream.Seek((long)ptr, SeekOrigin.Begin);
            return reader.ReadBytes(ValueSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong getPtrOfMainAddr(ulong address)
        {
            var st = address - mainAddrStart;
            return mainFileStart + st; 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong getPtrOfHeapAddr(ulong address)
        {
            var st = address - heapAddrStart;
            return heapFileStart + st;
        }

        public SearchRange? GetIsSearcheableRange(SearchRange range)
        {
            if (isMainAddress(range.Start))
                return new SearchRange()
                {
                    Start = range.Start,
                    End = mainAddrEnd
                };

            if (isHeapAddress(range.Start))
                return new SearchRange()
                {
                    Start = range.Start,
                    End = heapAddrEnd
                };

            return null;
        }
    }
}
