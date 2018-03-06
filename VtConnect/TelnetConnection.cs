using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VtConnect
{
    [UriScheme("telnet")]
    public class TelnetConnection : Connection
    {
        public override bool IsConnected => throw new NotImplementedException();

        public override Task<bool> Connect(Uri destination, NetworkCredentials credentials)
        {
            throw new NotImplementedException();
        }

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        public override Task SendData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void SetTerminalWindowSize(int columns, int rows, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
