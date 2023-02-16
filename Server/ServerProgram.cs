using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class ServerProgram
{
    static void Main(string[] args)
    {
        using(Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);
            serverSocket.Bind(endPoint);

            serverSocket.Listen(20);

            using(Socket clientSocket = serverSocket.Accept())
            {
                Console.WriteLine(clientSocket.RemoteEndPoint);

                while(true)
                {
                    ArraySegment<byte> segment = new ArraySegment<byte>(new byte[1024]);
                    
                    int cnt = clientSocket.Receive(segment);

                    if(cnt < 1)
                    {
                        Console.WriteLine("클라이언트 연결 종료");
                        return;
                    }
                    else if(cnt == 1)  //만약 헤더만큼도 못받은 거라면
                    {
                        clientSocket.Receive(segment.Array, 1, 1, SocketFlags.None);
                    }
                    //Console.WriteLine(cnt);

                    ArraySegment<byte> header = new ArraySegment<byte>(segment.Array, 0, 2);
                    short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(header));

                    //Console.WriteLine(BitConverter.ToString(segment.Array));
                    
                    while(cnt - 2 < dataSize)
                    {
                        int cnt2 = clientSocket.Receive(segment.Array, cnt, dataSize - cnt - 2, SocketFlags.None);
                        cnt += cnt2;
                    }

                    ArraySegment<byte> data = new ArraySegment<byte>(segment.Array, 2, dataSize);
                    string str = Encoding.UTF8.GetString(data);
                    //Console.WriteLine(dataSize);
                    Console.WriteLine(str);

                    clientSocket.Send(segment);
                }
            }
        }
    }
}
