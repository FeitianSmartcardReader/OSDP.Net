using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace OSDP.Net.Connections
{
    public class TcpServerOsdpConnection : IOsdpConnection
    {
        private readonly TcpListener _listener;
        private TcpClient _tcpClient;

        public TcpServerOsdpConnection(int portNumber, int baudRate)
        {
            _listener = TcpListener.Create(portNumber);
            BaudRate = baudRate;
        }

        public int BaudRate { get; }

        public bool IsOpen
        {
            get
            {
                var tcpClient = _tcpClient;
                return tcpClient != null && tcpClient.Connected;
            }
        }

        public void Open()
        {
            _listener.Start();
            Task.Factory.StartNew(async () => { _tcpClient = await _listener.AcceptTcpClientAsync(); },
                TaskCreationOptions.LongRunning);
        }

        public void Close()
        {
            var tcpClient = _tcpClient;
            _tcpClient = null;
            tcpClient?.Close();
            _listener.Stop();
        }

        public async Task WriteAsync(byte[] buffer)
        {
            var tcpClient = _tcpClient;
            if (tcpClient != null)
            {
                await tcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async Task<int> ReadAsync(byte[] buffer, CancellationToken token)
        {
            var tcpClient = _tcpClient;
            if (tcpClient != null)
            {
                return await tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length, token);
            }

            return 0;
        }
    }
}