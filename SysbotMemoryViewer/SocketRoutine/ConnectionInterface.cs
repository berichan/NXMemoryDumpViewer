using NHSE.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysbotMemoryViewer
{
    public class ConnectionInterface
    {
        public IRAMReadWriter Connection { get; private set; }
        public ConnectionInterface(string ip, ushort port)
        {
            Connection = Connect(ip, port);
        }

        public SysBot Connect(string ip, ushort port)
        {
            var sb = new SysBot();
            sb.Connect(ip, port);
            return sb;
        }

    }
}
