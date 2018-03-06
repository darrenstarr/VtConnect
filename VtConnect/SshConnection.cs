using Renci.SshNet;
using System;
using System.Linq;
using System.Threading.Tasks;
using VtConnect.Exceptions;

namespace VtConnect
{
    [UriScheme("ssh")]
    public class SshConnection : Connection, IDisposable, IEquatable<SshConnection>
    {
        private AuthenticationMethod AuthenticationMethod { get; set; }
        private ConnectionInfo ConnectionInfo { get; set; }
        private SshClient Client { get; set; }
        private ShellStream ClientStream { get; set; }

        public override bool IsConnected
        {
            get
            {
                return !DisposedValue && ClientStream != null && Client != null && Client.IsConnected;
            }
        }

        /// <exception cref="SocketException">Socket connection to the SSH server or proxy server could not be established, or an error occurred while resolving the hostname.</exception>
        /// <exception cref="SshConnectionException">SSH session could not be established.</exception>
        /// <exception cref="SshAuthenticationException">Authentication of SSH session failed.</exception>
        /// <exception cref="ProxyException">Failed to establish proxy connection.</exception>
        public override Task<bool> Connect(Uri destination, NetworkCredentials credentials)
        {
            Destination = destination;

            if(credentials is UsernamePasswordCredentials)
            {
                var upCredentials = credentials as UsernamePasswordCredentials;
                AuthenticationMethod = new PasswordAuthenticationMethod(upCredentials.Username, upCredentials.Password);
            }
            else
                throw new UnhandledCredentialTypeException("Unhandled credential type " + credentials.GetType().ToString());

            int port = Destination.IsDefaultPort ? 22 : Destination.Port;

            ConnectionInfo = new ConnectionInfo(destination.Host, port, credentials.Username, AuthenticationMethod);
            Client = new SshClient(ConnectionInfo);

            return Task.Run(() =>
                {
                    try
                    {
                        Client.Connect();

                        ClientStream = Client.CreateShellStream("xterm", (uint)Columns, (uint)Rows, 800, 600, 16384);
                        if (ClientStream == null)
                            throw new VtConnectException("Failed to create client stream");

                        ClientStream.DataReceived += ClientStream_DataReceived;
                        ClientStream.ErrorOccurred += ClientStream_ErrorOccurred;
                    }
                    catch (Exception e)
                    {
                        Client.Disconnect();
                        Client.Dispose();
                        Client = null;

                        ConnectionInfo = null;
                        AuthenticationMethod = null;

                        System.Diagnostics.Debug.WriteLine(e.Message);
                        return false;
                    }

                    return true;
                }
            );
        }

        public override async Task SendData(byte[] data)
        {
            await Task.Factory.FromAsync(ClientStream.BeginWrite, ClientStream.EndWrite, data, 0, data.Length, null);
        }

        private void ClientStream_ErrorOccurred(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Exception.Message);

            try
            {
                ClientStream.Close();
                ClientStream.Dispose();
            
                Client.Disconnect();
                Client.Dispose();
            }
            finally
            {
                ConnectionInfo = null;
                AuthenticationMethod = null;
                ClientStream = null;
                Client = null;
            }
        }

        public override void Disconnect()
        {
            ConnectionInfo = null;
            AuthenticationMethod = null;

            ClientStream.Close();
            ClientStream = null;

            Client.Disconnect();
            Client = null;
        }

        private void ClientStream_DataReceived(object sender, Renci.SshNet.Common.ShellDataEventArgs e)
        {
            Task.Run(async () =>
                {
                    Delegate[] invocationList = DataReceived.GetInvocationList();
                    Task[] dataReceivedTasks = new Task[invocationList.Length];

                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        dataReceivedTasks[i] =
                            ((Func<object, DataReceivedEventArgs, Task>)invocationList[i])
                                (
                                    this,
                                    new DataReceivedEventArgs
                                    {
                                        Data = e.Data.ToArray()
                                    }
                                );
                    }

                    await Task.WhenAll(dataReceivedTasks);
                }
            );
        }

        private bool DisposedValue { get; set; } // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        ClientStream.Close();
                        ClientStream.Dispose();

                        Client.Disconnect();
                        Client.Dispose();
                    }
                    finally
                    {
                        ConnectionInfo = null;
                        AuthenticationMethod = null;
                        ClientStream = null;
                        Client = null;
                    }
                }

                DisposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        public bool Equals(SshConnection other)
        {
            return ReferenceEquals(this, other);
        }

        public override void SetTerminalWindowSize(int columns, int rows, int width, int height)
        {
            ClientStream.SendWindowChangeRequest((uint)columns, (uint)rows, (uint)width, (uint)height);
        }
    }
}
