using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client;

internal class TAPClient
{
    static async Task Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);

        await socket.ConnectAsync(endPoint);

        while (true)
        {
            string str = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            await socket.SendAsync(buffer, SocketFlags.None);
        }
    }
}
