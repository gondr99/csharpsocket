using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client;

internal class APMClient
{
    static void Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);

        socket.BeginConnect(endPoint, ConnectCompleted, socket);

        while(true)
        {
            string str = Console.ReadLine();
            socket.Send(Encoding.UTF8.GetBytes(str));
        }


    }

    private static void ConnectCompleted(IAsyncResult ar)
    {
        Socket socket = (Socket)ar.AsyncState;
        socket.EndConnect(ar); //비동기로 연결

        string str = Console.ReadLine();
        byte[] buffer = Encoding.UTF8.GetBytes(str);

        socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendComplete, socket);
    }

    private static void SendComplete(IAsyncResult ar)
    {
        Socket socket = (Socket)(ar.AsyncState);
        socket.EndSend(ar);

        string str = Console.ReadLine();
        byte[] buffer = Encoding.UTF8.GetBytes(str);

        socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendComplete, socket);

    }
}
