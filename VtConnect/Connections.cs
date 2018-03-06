using System;
using System.Linq;
using System.Threading.Tasks;

namespace VtConnect
{
    public abstract class Connection
    {
        public Uri Destination { get; set; }

        public int Columns { get; set; } = 80;
        public int Rows { get; set; } = 25;

        public abstract bool IsConnected
        {
            get;
        }

        public Func<object, DataReceivedEventArgs, Task> DataReceived;

        public Connection()
        {
        }

        public abstract Task<bool> Connect(Uri destination, NetworkCredentials credentials);

        public abstract void Disconnect();

        public static Connection CreateConnection(Uri destination)
        {
            var result = ConnectionFactory.CreateByScheme(destination.Scheme);

            return result;
        }

        public abstract Task SendData(byte[] data);

        public abstract void SetTerminalWindowSize(int columns, int rows, int width, int height);
    }
}
