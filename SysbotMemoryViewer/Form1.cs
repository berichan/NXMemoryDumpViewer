using System.Windows.Forms;

namespace SysbotMemoryViewer
{
    public partial class MemDumpView : Form
    {
        private readonly string pathTemp = @"D:/tmp";
        private ConnectionInterface Connection = default!;
        private Searcher Search = default!;

        public IValueComparison Comparer { get; private set; } = new Equals();

        private SearchConditionDirection Direction = SearchConditionDirection.Known;
        private SearchType InputType = SearchType.U64;

        public MemDumpView()
        {
            InitializeComponent();

            Search = new Searcher(Comparer, PB_Main, pathTemp);

            CB_Type.DataSource = Enum.GetValues(typeof(SearchType));
            CB_Type.SelectedIndexChanged += (o, e) => UpdateType();

            CB_Condition.DataSource = Enum.GetValues(typeof(SearchConditionDirection));
            CB_Condition.SelectedIndexChanged += (o, e) => Direction = (SearchConditionDirection)CB_Condition.SelectedItem;

            CB_Comparer.DataSource = Enum.GetValues(typeof(SearchConditionCompare));
            CB_Comparer.SelectedIndexChanged += (o, e) => UpdateComparer();

            UpdateStackView();
        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            var ip = TB_IP.Text;
            var port = ushort.Parse(TB_Port.Text);
            Connection = new ConnectionInterface(ip, port);

            GB_Search.Visible = true;
        }

        private void BTN_Dump_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Connection?.Connection.GetDump());
        }

        private void BTN_Undo_Click(object sender, EventArgs e)
        {
            if (Search.Stack.Count < 1)
            {
                MessageBox.Show("Nothing to undo!");
                return;
            }

            Search.UndoLastStack();
            UpdateStackView();
        }

        private void BTN_Restart_Click(object sender, EventArgs e)
        {
            Search.InitialiseStack();
            UpdateStackView();
        }

        private void BTN_Search_Click(object sender, EventArgs e)
        {
            string? nPath = null;
            if ((ModifierKeys & Keys.Control) != 0)
            {
                var fileDia = new OpenFileDialog()
                {
                    FileName = "Select a file",
                    Title = "Open dump file"
                };

                if (fileDia.ShowDialog() == DialogResult.OK)
                {
                    nPath = Path.ChangeExtension(fileDia.FileName, null);
                }
                else
                    return;
            }

            if (Search.Stack.Count == 0 && Search.RootComparison == default)
            {
                if (Direction == SearchConditionDirection.Previous)
                {
                    MessageBox.Show("No initial search to compare with!");
                    return;
                }

                // init with file
                var dmp = nPath == null ? Connection.Connection.GetDump().TrimEnd('\0').TrimEnd('\n') : nPath;
                var sf = new SearchFile(dmp, GetValueSize());
                Search.InitialiseStack(sf);
                if (Direction == SearchConditionDirection.Known)
                    DoCompare(GetCurrentInputValue());
                UpdateStackView();
                return;
            }

            if (Search.RootComparison != default && Search.Stack.Count == 0 && Direction == SearchConditionDirection.Previous)
            {
                // dump again
                var dmp = nPath == null ? Connection.Connection.GetDump().TrimEnd('\0').TrimEnd('\n') : nPath;
                var sf = new SearchFile(dmp, GetValueSize());
                DoCompareRoot(sf);
                UpdateStackView();
                return;
            }

            if (Direction == SearchConditionDirection.Previous || Direction == SearchConditionDirection.Known)
            {
                var dmp = nPath == null ? Connection.Connection.GetDump().TrimEnd('\0').TrimEnd('\n') : nPath;
                var sf = new SearchFile(dmp, GetValueSize());

                // populate block
                var memDiff = new MemoryDiff(pathTemp, "tmp", GetValueSize(), PB_Main);
                var lastStack = Search.GetLastStack();
                lastStack.ResetPointer();
                var kvp = lastStack.ReadNext();
                while (kvp.HasValue)
                {
                    memDiff.WriteNewDiff(kvp.Value.Key, sf.GetBytesOfAddress(kvp.Value.Key));
                    kvp = lastStack.ReadNext();
                }

                if (Direction == SearchConditionDirection.Previous)
                    DoCompareStack(memDiff);
                else
                    DoNewCompare(GetCurrentInputValue(), memDiff);
                UpdateStackView();
                return;
            }

            MessageBox.Show("Reached end of search!");
        }

        private byte[] GetCurrentInputValue()
        {
            return InputType switch
            {
                SearchType.U8 => new byte[1] { (byte)NUD_SearchValue.Value },
                SearchType.U16 => BitConverter.GetBytes((ushort)NUD_SearchValue.Value),
                SearchType.U32 or SearchType.FLT => BitConverter.GetBytes((uint)NUD_SearchValue.Value),
                _ => BitConverter.GetBytes((ulong)NUD_SearchValue.Value),
            };
        }

        private void DoCompareRoot(ISearchHashmap ish)
        {
            var lastIsh = Search.RootComparison;
            var memDiff = Search.Compare(lastIsh, ish, InputType);
            Search.AddToStack(memDiff);
            UpdateStackView();
        }

        private void DoCompareStack(MemoryDiff md)
        {
            var lastStack = Search.GetLastStack();
            var memDiff = Search.Compare(lastStack, md, InputType);
            Search.AddToStack(memDiff);
            UpdateStackView();
        }

        private void DoCompare(byte[] search)
        {
            if (Search.Stack.Count == 0)
                DoNewCompare(search, Search.RootComparison);
            else
                DoNewCompare(search, Search.GetLastStack());
        }

        private void DoNewCompare(byte[] search, ISearchHashmap addrBlock)
        {
            var memDiff = Search.Compare(addrBlock, search, InputType);
            Search.AddToStack(memDiff);
            UpdateStackView();
        }

        private void DoNewCompare(byte[] search, MemoryDiff md)
        {
            var memDiff = Search.Compare(md, search, InputType);
            Search.AddToStack(memDiff);
            UpdateStackView();
        }

        private void UpdateStackView()
        {
            AddressGridViewBinder.BindData(DGV_Values, Search.Stack.Count == 0 ? null : Search.GetLastStack(), InputType, 1024, 0);
        }

        private int GetValueSize()
        {
            var st = (SearchType)CB_Type.SelectedItem;
            return st.SearchTypeToByteLength();
        }

        private void UpdateComparer()
        {
            switch ((SearchConditionCompare)CB_Comparer.SelectedItem)
            {
                case SearchConditionCompare.Equals: Comparer = new Equals(); break;
                case SearchConditionCompare.NotEquals: Comparer = new NotEquals(); break;
                case SearchConditionCompare.GreaterThan: Comparer = new GreaterThan(); break;
                case SearchConditionCompare.LessThan: Comparer = new LessThan(); break;
                case SearchConditionCompare.GreaterThanOrEqual: Comparer = new GreaterThanOrEqual(); break;
                case SearchConditionCompare.LessThanOrEqual: Comparer = new LessThanOrEqual(); break;
            }

            Search.Comparer = Comparer;
        }

        private void UpdateType()
        {
            InputType = (SearchType)CB_Type.SelectedItem;
            var size = GetValueSize();
            NUD_SearchValue.Maximum = (decimal)(Math.Pow(256, size) - 1);
        }
    }
}