namespace CsharpSimpleWebServer.Server
{
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    public class Server
    {
        private readonly IPAddress ipAddress;

        private readonly int port;

        private readonly TcpListener tcpListener;
        public Server(string ipAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);

            this.port = 8080;

            this.tcpListener = new TcpListener(this.ipAddress, port);
        }

        public async Task Start()
        {

            this.tcpListener.Start();


            Console.WriteLine("Server started...");

            Console.WriteLine("Listening for requests....");

            while (true)
            {
                TcpClient connection = await this.tcpListener.AcceptTcpClientAsync();

                NetworkStream networkStream = connection.GetStream();

                var request = await this.ReadRequest(networkStream);

                Console.WriteLine(request);

                await WriteResponse(networkStream);

                connection.Close();
            }
        }
        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            int bufferLength = 1024;

            byte[] buffer = new byte[bufferLength];

            StringBuilder requestBuilder = new StringBuilder();

            while (networkStream.DataAvailable)
            {
                var bytesReader = await networkStream.ReadAsync(buffer, 0, bufferLength);

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesReader));
            }

            return requestBuilder.ToString();
        }

        private async Task WriteResponse(NetworkStream networkStream)
        {
            string content = "Кааак е уе!!!";

            int contentLength = Encoding.UTF8.GetByteCount(content);

            string response = $@"HTTP/1.1 200 OK
Server: My Web Server
Date: {DateTime.UtcNow.ToString("r")}
Content-Length: {contentLength}
Content-Type: text/plain; charset=UTF8

{content}";

            var HttpResponsebytes = Encoding.UTF8.GetBytes(response);

            await networkStream.WriteAsync(HttpResponsebytes);
        }
    }
}
