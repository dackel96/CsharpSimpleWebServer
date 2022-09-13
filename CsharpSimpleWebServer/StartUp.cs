// 127.0.0.1 <-- localhost Default

// Http://localhost:6969

// TcpClient/Listener is for bytes transfer

//GetStream flow of bytes

using CsharpSimpleWebServer.Server;
using System.Net;
using System.Net.Sockets;
using System.Text;

var server = new Server("127.0.0.1", 8080);

await server.Start();