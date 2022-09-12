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
        public ISearchHashmap RootComparison { get; private set; } = default!;
        public List<MemoryDiff> Stack { get; private set; } = new();

        private readonly ProgressBar progressBar;

        private readonly string rootMemdiffPath;
        private int idx = 0;
        private int NextFileNameIndex { get => ++idx; }

        public Searcher(IValueComparison c, ProgressBar pb, string pathRoot) 
        { 
            Comparer = c;
            progressBar = pb;
            rootMemdiffPath = pathRoot;

            if (!Directory.Exists(rootMemdiffPath))
                Directory.CreateDirectory(rootMemdiffPath);
        }

        public void InitialiseStack()
        {
            RootComparison = default!;
            Stack.Clear();
        }

        public void InitialiseStack(ISearchHashmap sf)
        {
            Stack.Clear();
            RootComparison = sf;
        }

        public MemoryDiff? UndoLastStack()
        {
            if (Stack.Count <= 1)
                return null;
            Stack.RemoveAt(Stack.Count - 1);
            return GetLastStack();
        }

        public void AddToStack(MemoryDiff sh) => Stack.Add(sh);

        public MemoryDiff GetLastStack() => Stack[^1];

        public MemoryDiff Compare(ISearchHashmap old, ISearchHashmap @new, SearchType st) => 
            MemoryDiff.GenerateMemoryDiff(rootMemdiffPath, GetNextDiffFilename(), progressBar, old, @new, SearchTypeToComparer(st));

        public MemoryDiff Compare(ISearchHashmap a, byte[] bytes, SearchType st) =>
            MemoryDiff.GenerateMemoryDiff(rootMemdiffPath, GetNextDiffFilename(), progressBar, a, bytes, SearchTypeToComparer(st));

        public MemoryDiff Compare(MemoryDiff old, MemoryDiff @new, SearchType st) =>
            MemoryDiff.GenerateMemoryDiff(rootMemdiffPath, GetNextDiffFilename(), progressBar, old, @new, SearchTypeToComparer(st));

        public MemoryDiff Compare(MemoryDiff a, byte[] bytes, SearchType st) =>
            MemoryDiff.GenerateMemoryDiff(rootMemdiffPath, GetNextDiffFilename(), progressBar, a, bytes, SearchTypeToComparer(st));

        private string GetNextDiffFilename() => $"diff{NextFileNameIndex}";

        private Func<byte[], byte[], bool> SearchTypeToComparer(SearchType st) =>
            st switch
            {
                SearchType.U8 => Comparer.CompareU8Array,
                SearchType.U16 => Comparer.CompareU16,
                SearchType.U32 => Comparer.CompareU32,
                SearchType.U64 => Comparer.CompareU64,
                SearchType.FLT => Comparer.CompareFloat,
                SearchType.DBL => Comparer.CompareDouble,
                _ => Comparer.CompareU32,
            };
    }
}
