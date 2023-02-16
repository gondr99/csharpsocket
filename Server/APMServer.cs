using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server;

internal class APMServer
{
    static void Main(string[] args)
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(1000);

        serverSocket.BeginAccept(AcceptCompleted, serverSocket);

        while(true)
        {
            Thread.Sleep(1000);
        }
    }

    //비동기 작업에 대한 정보를 가지고 있다.
    private static void AcceptCompleted(IAsyncResult ar)
    {
        Socket serverSocket = (Socket)ar.AsyncState; //AsyncState에 소켓이 전달되어 온다.
        Socket clientSocket = serverSocket.EndAccept(ar); //들어오는 연결시도를 비동기로 받아들인다. 

        Console.WriteLine(clientSocket.RemoteEndPoint);

        serverSocket.BeginAccept(AcceptCompleted, serverSocket);//다시 시작

        //이제부터 데이터 받기
        byte[] buffer = new byte[1024]; //버퍼랑 소켓을 둘다 넣어줘야 해 마지막 오브젝트로 그래서 튜플객체로 넣었음
        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCompleted, (clientSocket, buffer));
    }

    private static void ReceiveCompleted(IAsyncResult ar)
    {
        (Socket clientSocket, byte[] buffer) = ((Socket, byte[]))ar.AsyncState;
        int cnt1 = clientSocket.EndReceive(ar);//비동기로 받아오기
        if(cnt1 < 1)
        {
            Console.WriteLine("클라이언트 종료");
            clientSocket.Dispose();
            return;
        }

        Console.WriteLine(Encoding.UTF8.GetString(buffer));

        byte[] buffer2 = new byte[1024];
        clientSocket.BeginReceive(buffer2, 0, buffer2.Length, SocketFlags.None, ReceiveCompleted, (clientSocket, buffer2));
    }
}
