using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

internal class ClientProgram
{
    //static void Main(string[] args)
    //{
    //    AsyncMainTest();
    //}

    //비동기 테스트
    static void AsyncMainTest()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.RemoteEndPoint = endPoint;
        args.Completed += ConnectCompleted;
        bool pending = socket.ConnectAsync(args);
        if(pending == false)
        {
            ConnectCompleted(socket, args);
        }

        while(true)
        {
            Thread.Sleep(1000);
        }
        

    }
    private static void ConnectCompleted(object? sender, SocketAsyncEventArgs args)
    {
        Socket socket = (Socket)sender;
        args.Dispose(); //한번만 쓸꺼니까 이건 바로 반환

        
        string str = Console.ReadLine();
        byte[] buffer = Encoding.UTF8.GetBytes(str);

        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
        sendArgs.SetBuffer(buffer, 0, buffer.Length);
        sendArgs.Completed += SendCompleted;
        bool pending = socket.SendAsync(sendArgs);
        if(pending==false)
        {
            SendCompleted(socket, sendArgs);
        }
    }

    private static void SendCompleted(object? sender, SocketAsyncEventArgs args)
    {
        Socket socket = (Socket)sender;

        string str = Console.ReadLine();
        byte[] buffer = Encoding.UTF8.GetBytes(str);

        args.SetBuffer(buffer, 0, buffer.Length);
        bool pending = socket.SendAsync(args);
        if (pending == false)
        {
            SendCompleted(socket, args);
        }
    }

    static void MainBufferTest()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);
        socket.Connect(endPoint);

        while(true) 
        {
            int size = int.Parse(Console.ReadLine());
            byte[] buffer = new byte[size];
            int cnt = socket.Receive(buffer);
            Console.WriteLine($"Send : {cnt.ToString("#,#")}");
        }
    }

    static void MainSendTest()
    {
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);
            ArraySegment<byte> recvBuffer = new ArraySegment<byte>(new byte[1024]);
            socket.Connect(endPoint);
            while (true)
            {
                string str = Console.ReadLine();
                if (str == "exit") 
                    return;

                byte[] strBuffer = Encoding.UTF8.GetBytes(str);
                byte[] packetBuffer = new byte[2 + strBuffer.Length];
                byte[] dateSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)strBuffer.Length));
                
                Array.Copy(dateSize, 0, packetBuffer, 0, dateSize.Length);
                Array.Copy(strBuffer, 0, packetBuffer, 2, strBuffer.Length);

                socket.Send(packetBuffer);


                socket.Receive(recvBuffer);

                ArraySegment<byte> header = new ArraySegment<byte>(recvBuffer.Array, 0, 2);
                short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(header));
                ArraySegment<byte> data = new ArraySegment<byte>(recvBuffer.Array, 2, dataSize);
                string recvStr = Encoding.UTF8.GetString(data);
                
                Console.WriteLine(recvStr);
            }
        }
    }
}
