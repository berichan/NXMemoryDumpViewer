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
        public static void BindData(DataGridView view, ISearchHashmap? map, SearchType st, int maxPerPage, int page)
        {
            view.ColumnCount = 1;
            view.RowCount = 1;

            if (map is not SearchBlock sb)
            {
                view.Name = "No reason to show unfiltered file data.";
                return;
            }

            if (sb.AddressValues.Count == 0)
            {
                view.Name = "No matches.";
                return;
            }

            var startIndex = page * maxPerPage;
            if (sb.AddressValues.Count < startIndex)
            {
                view.Name = "This is the beyond realm.";
                return;
            }

            var endIndex = Math.Min(sb.AddressValues.Count, startIndex + maxPerPage);
            var nRange = sb.AddressValues.Skip(startIndex).Take(endIndex - startIndex).ToDictionary(k => k.Key, v => v.Value);

            view.Name = "view";
            view.ColumnCount = 2;
            view.Columns[0].Name = "Address";
            view.Columns[1].Name = "Value";

            foreach(var v in nRange)
            {
                string addr = v.Key.ToString("X16");
                string dValue;
                switch (st)
                {
                    case SearchType.U8: dValue = v.Value[0].ToString("X16"); break;
                    case SearchType.U16: dValue = BitConverter.ToUInt16(v.Value).ToString("X16"); break;
                    case SearchType.U32: dValue = BitConverter.ToUInt32(v.Value).ToString("X16"); break;
                    case SearchType.U64: dValue = BitConverter.ToUInt64(v.Value).ToString("X16"); break;
                    case SearchType.FLT: dValue = BitConverter.ToSingle(v.Value).ToString(); break;
                    case SearchType.DBL: dValue = BitConverter.ToDouble(v.Value).ToString(); break;
                    default: dValue = BitConverter.ToUInt64(v.Value).ToString("X16"); break;
                }
                view.Rows.Add(new string[2] { addr, dValue });
            }
        }
    }
}
