namespace SysbotMemoryViewer
{
    public partial class Form1 : Form
    {
        private ConnectionInterface Connection = default!;
        private Searcher Search = default!;

        public IValueComparison Comparer { get; private set; } = new Equals();

        private SearchConditionDirection Direction = SearchConditionDirection.Known;
        private SearchType InputType = SearchType.U64;

        public Form1()
        {
            InitializeComponent();

            Search = new Searcher(Comparer, PB_Main);

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
            if (Search.Stack.Count < 2)
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
            if (Search.Stack.Count == 0)
            {
                if (Direction == SearchConditionDirection.Previous)
                {
                    MessageBox.Show("No initial search to compare with!");
                    return;
                }

                // init with file
                var dmp = Connection.Connection.GetDump().TrimEnd('\0').TrimEnd('\n');
                var sf = new SearchFile(dmp, GetValueSize());
                Search.InitialiseStack(sf);
                if (Direction == SearchConditionDirection.Known)
                    DoCompare(GetCurrentInputValue());
                UpdateStackView();
                return;
            }

            if (Search.Stack.Count == 1 && Direction == SearchConditionDirection.Previous && Search.GetLastStack() is SearchFile)
            {
                // dump again
                var dmp = Connection.Connection.GetDump().TrimEnd('\0').TrimEnd('\n');
                var sf = new SearchFile(dmp, GetValueSize());
                DoCompare(sf);
                UpdateStackView();
                return;
            }

            if (Direction == SearchConditionDirection.Previous || Direction == SearchConditionDirection.Known)
            {
                // fetch all addresses
                var addrMap = new Dictionary<ulong, int>();

                if (Search.GetLastStack() is not SearchBlock last)
                {
                    MessageBox.Show("Too many file dumps!");
                    return;
                }

                foreach (var addr in last.AddressValues)
                    addrMap.Add(addr.Key, last.ValueSize);

                var maps = Connection.Connection.ReadBytesAbsoluteMulti(addrMap);

                // populate block
                var blockMap = new Dictionary<ulong, byte[]>();
                var keys = addrMap.Keys.ToArray();
                for (int i = 0; i < addrMap.Count; ++i)
                {
                    int bStart = i * last.ValueSize;
                    var bRange = maps[bStart..(bStart + last.ValueSize)];
                    blockMap.Add(keys[i], bRange);
                }
                var nBlock = new SearchBlock(blockMap, last.ValueSize);

                if (Direction == SearchConditionDirection.Previous)
                    DoCompare(nBlock);
                else
                    DoNewCompare(GetCurrentInputValue(), nBlock);
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

        private void DoCompare(ISearchHashmap ish)
        {
            var lastIsh = Search.GetLastStack();
            SearchBlock sb = InputType switch
            {
                SearchType.U8 => Search.CompareU8(ish, lastIsh),
                SearchType.U16 => Search.CompareU16(ish, lastIsh),
                SearchType.U32 => Search.CompareU32(ish, lastIsh),
                SearchType.U64 => Search.CompareU64(ish, lastIsh),
                SearchType.FLT => Search.CompareFlt(ish, lastIsh),
                SearchType.DBL => Search.CompareDbl(ish, lastIsh),
                // I guess
                _ => Search.CompareU8(ish, lastIsh),
            };
            Search.AddToStack(sb);
            UpdateStackView();
        }

        private void DoCompare(byte[] search)
        {
            var lastIsh = Search.GetLastStack();
            DoNewCompare(search, lastIsh);
        }

        private void DoNewCompare(byte[] search, ISearchHashmap addrBlock)
        {
            SearchBlock sb = InputType switch
            {
                SearchType.U8 => Search.CompareU8(addrBlock, search),
                SearchType.U16 => Search.CompareU16(addrBlock, search),
                SearchType.U32 => Search.CompareU32(addrBlock, search),
                SearchType.U64 => Search.CompareU64(addrBlock, search),
                SearchType.FLT => Search.CompareFlt(addrBlock, search),
                SearchType.DBL => Search.CompareDbl(addrBlock, search),
                // I guess
                _ => Search.CompareU8(addrBlock, search),
            };
            Search.AddToStack(sb);
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