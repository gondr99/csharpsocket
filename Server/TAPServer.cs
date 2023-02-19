using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server;

internal class TAPServer
{
    static async Task Main(string[] args)
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(1000);

        // TAP (작업 기반 비동기 패턴)

        while (true)
        {
            Socket clientSocket = await serverSocket.AcceptAsync();
            //제네릭 타입의 Task에 await를 걸면 해당 제네릭 타입을 반환값으로 리턴한다.
            Console.WriteLine(clientSocket.RemoteEndPoint);
            //쓰레드 풀에 일감을 넣는다.첫번째가 함수여야하고 두번째가 sender임
            ThreadPool.QueueUserWorkItem(ReadAsync, clientSocket);
        }
    }

    private static async void ReadAsync(object? sender)
    {
        Socket clientSocket = (Socket)sender;
        while (true)
        {
            byte[] buffer = new byte[256];
            int n1 = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
            if (n1 < 1)
            {
                Console.WriteLine("cliend disconnect");
                clientSocket.Dispose();
                return;
            }
            Console.WriteLine(Encoding.UTF8.GetString(buffer));
        }
    }
}
