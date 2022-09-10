using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysbotMemoryViewer
{
    public class Searcher
    {
        public IValueComparison Comparer { get; set; }
        public List<ISearchHashmap> Stack { get; private set; } = new();

        private ProgressBar progressBar;

        public Searcher(IValueComparison c, ProgressBar pb) 
        { 
            Comparer = c;
            progressBar = pb;
        }

        public void InitialiseStack()
        {
            Stack.Clear();
        }

        public void InitialiseStack(SearchFile sf)
        {
            Stack.Clear();
            Stack.Add(sf);
        }

        public ISearchHashmap? UndoLastStack()
        {
            if (Stack.Count <= 1)
                return null;
            Stack.RemoveAt(Stack.Count - 1);
            return GetLastStack();
        }

        public void AddToStack(ISearchHashmap sh)
        {
            Stack.Add(sh);
        }

        public ISearchHashmap GetLastStack() => Stack[^1];

        #region MultipleMaps

        public SearchBlock CompareU8(ISearchHashmap a, ISearchHashmap b) => doValueSearch(a, b, 1, Comparer.CompareU8Array);
        public SearchBlock CompareU16(ISearchHashmap a, ISearchHashmap b) => doValueSearch(a, b, 2, Comparer.CompareU16);
        public SearchBlock CompareU32(ISearchHashmap a, ISearchHashmap b) => doValueSearch(a, b, 4, Comparer.CompareU32);
        public SearchBlock CompareU64(ISearchHashmap a, ISearchHashmap b) => doValueSearch(a, b, 8, Comparer.CompareU64);
        public SearchBlock CompareFlt(ISearchHashmap a, ISearchHashmap b) => doValueSearch(a, b, 4, Comparer.CompareFloat);
        public SearchBlock CompareDbl(ISearchHashmap a, ISearchHashmap b) => doValueSearch(a, b, 8, Comparer.CompareDouble);

        private SearchBlock doValueSearch(ISearchHashmap a, ISearchHashmap b, int size, Func<byte[], byte[], bool> comparer)
        {
            Dictionary<ulong, byte[]> searchMap = new();
            ISearchHashmap smaller = a.ByteSize > b.ByteSize ? b : a;
            ISearchHashmap bigger = a.ByteSize > b.ByteSize ? a : b;

            for (int i = 0; i < smaller.SearcheableRanges.Count; ++i)
            {
                var searchRange = smaller.SearcheableRanges[i];
                var searchSize = searchRange.End - searchRange.Start;

                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                progressBar.Step = 1;

                var pc1 = searchSize / 100; 

                for (ulong j = 0; j < searchSize; j += (ulong)size)
                {
                    var byteReq = new SearchRange() { Start = searchRange.Start + j, End = searchRange.Start + j + (ulong)size };
                    var sr = bigger.GetIsSearcheableRange(byteReq);
                    if (sr != null)
                    {
                        var srV = sr.Value;
                        var aB = smaller.GetBytesOfAddress(srV.Start);
                        var bB = bigger.GetBytesOfAddress(srV.Start);
                        if (aB != null && bB != null && comparer(aB, bB))
                            searchMap.Add(srV.Start, aB);
                    }

                    if (j % pc1 == 0)
                    {
                        progressBar.PerformStep();
                        GC.Collect();
                    }
                }
            }

            return new SearchBlock(searchMap, size);
        }

        #endregion

        #region SingleFixedValue

        public SearchBlock CompareU8(ISearchHashmap a, byte[] bytes) => doValueSearchFixed(a, bytes, Comparer.CompareU8Array);
        public SearchBlock CompareU16(ISearchHashmap a, byte[] bytes) => doValueSearchFixed(a, bytes, Comparer.CompareU16);
        public SearchBlock CompareU32(ISearchHashmap a, byte[] bytes) => doValueSearchFixed(a, bytes, Comparer.CompareU32);
        public SearchBlock CompareU64(ISearchHashmap a, byte[] bytes) => doValueSearchFixed(a, bytes, Comparer.CompareU64);
        public SearchBlock CompareFlt(ISearchHashmap a, byte[] bytes) => doValueSearchFixed(a, bytes, Comparer.CompareFloat);
        public SearchBlock CompareDbl(ISearchHashmap a, byte[] bytes) => doValueSearchFixed(a, bytes, Comparer.CompareDouble);

        public SearchBlock doValueSearchFixed(ISearchHashmap a, byte[] bytes, Func<byte[], byte[], bool> comparer)
        {
            Dictionary<ulong, byte[]> searchMap = new();

            for (int i = 0; i < a.SearcheableRanges.Count; ++i)
            {
                var range = a.SearcheableRanges[i];

                var searchSize = range.End - range.Start;

                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                progressBar.Step = 1;

                var pc1 = searchSize / 100;

                for (ulong j = range.Start; j < range.End; j += (ulong)bytes.Length)
                {
                    var bts = a.GetBytesOfAddress(j);
                    if (bts != null && comparer(bts, bytes))
                        searchMap.Add(j, bts);

                    if (j % pc1 == 0)
                    {
                        progressBar.PerformStep();
                        GC.Collect();
                    }
                }
            }

            return new SearchBlock(searchMap, bytes.Length);
        }

        #endregion
    }
}
