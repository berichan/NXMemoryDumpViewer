using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysbotMemoryViewer
{
    public static class AddressGridViewBinder
    {
        public static void BindData(DataGridView view, MemoryDiff? diff, SearchType st, int maxPerPage, int page)
        {
            view.ColumnCount = 0;
            view.RowCount = 1;

            if (diff == null || diff.Size == 0)
            {
                view.Name = "No matches.";
                return;
            }

            var startIndex = page * maxPerPage;
            diff.SetPointer(startIndex);

            view.Name = "view";
            view.ColumnCount = 2;
            view.Columns[0].Name = "Address";
            view.Columns[1].Name = "Value";

            var kvp = diff.ReadNext();
            int ct = 0;
            while (kvp.HasValue && ct < maxPerPage)
            {
                var v = kvp.Value;
                string addr = v.Key.ToString("X10");
                string dValue = st switch
                {
                    SearchType.U8 => v.Value[0].ToString("X2"),
                    SearchType.U16 => BitConverter.ToUInt16(v.Value).ToString("X4"),
                    SearchType.U32 => BitConverter.ToUInt32(v.Value).ToString("X8"),
                    SearchType.U64 => BitConverter.ToUInt64(v.Value).ToString("X16"),
                    SearchType.FLT => BitConverter.ToSingle(v.Value).ToString(),
                    SearchType.DBL => BitConverter.ToDouble(v.Value).ToString(),
                    _ => BitConverter.ToUInt64(v.Value).ToString("X16"),
                };
                view.Rows.Add(new string[2] { addr, dValue });

                ct++;
                kvp = diff.ReadNext();
            }
        }
    }
}
