using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class ServerProgram
{
    //static void Main(string[] args)
    //{
    //    AsyncMainTest();
    //}

    static void AsyncMainTest()
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(1000);
        

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += AcceptCompleted;
        //이건 동기로 할지 비동기로 할지를 결정하는거야. pending이 true이면 비동기로 가는거고 false면 즉시
        bool pending = serverSocket.AcceptAsync(args);
        if(pending == false)  //이경우는 Complete를 직접호출해야 함. 동기로 간거야
        {
            AcceptCompleted(serverSocket, args);
        }

        while(true)
        {
            Thread.Sleep(1000);
        }

    }

    //? 는 nullable을 뜻함
    private static void AcceptCompleted(object? sender, SocketAsyncEventArgs args)
    {
        //첫번째 인자는 호출되었을때 this가 들어오는데 그게 서버 소켓이다 따라서 캐스팅이 가능하다.
        Socket serverSocket = (Socket)sender;
        Socket clientSocket = args.AcceptSocket; //어셉트된 소켓

        Console.WriteLine(clientSocket.RemoteEndPoint);//접속
        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        args.AcceptSocket = null; //기존에 받았던 소켓은 지워주고 
        bool pending = serverSocket.AcceptAsync(args); //아규먼트 인스턴스는 재활용하자.
        if(pending==false)
        {
            AcceptCompleted(serverSocket, args);
        }

        SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
        recvArgs.Completed += ReceiveCompleted;
        byte[] buffer = new byte[1024];
        recvArgs.SetBuffer(buffer, 0, buffer.Length); //버퍼 설정
        bool pending2 = clientSocket.ReceiveAsync(recvArgs);
        if(pending2==false)
        {
            ReceiveCompleted(clientSocket, recvArgs);
        }
    }

    private static void ReceiveCompleted(object? sender, SocketAsyncEventArgs args)
    {
        Socket clientSocket = (Socket)sender;
        if(args.BytesTransferred < 1)  //전송된
        {
            Console.WriteLine("클라이언트 연결 종료");
            clientSocket.Dispose(); //자원 해제
            args.Dispose(); //자원 해제
            return;
        }

        Console.WriteLine(Encoding.UTF8.GetString(args.Buffer));
        byte[] buffer = new byte[1024];
        args.SetBuffer(buffer, 0, buffer.Length); //버퍼 설정 
        //Array.Clear(args.Buffer, 0, args.BytesTransferred);
        bool pending = clientSocket.ReceiveAsync(args);
        if(pending == false)
        {
            ReceiveCompleted(clientSocket, args);
        }
    }

    static void MainBufferTest()
    {
        using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("121.190.134.63"), 20000);
            serverSocket.Bind(endPoint);

            serverSocket.Listen(20);

            Socket cSocket = serverSocket.Accept();
            Console.WriteLine(cSocket.RemoteEndPoint);

            while (true)
            {
                int size = int.Parse(Console.ReadLine());
                byte[] buffer = new byte[size];
                int n = cSocket.Receive(buffer);
                Console.WriteLine($"Received : {n.ToString("#,#")}");
            }
        }
    }

    static void MainSendTest()
    {
        using(Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            //이걸 추가하면 기존서버를 죽이고 자기가 다시 올라갈 수 있다(포트 문제 해결)
            serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

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
                    //0이 나오는 경우는 Fin 패킷을 받은 경우만 이다.
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

                    //TCP헤더에서 window사이즈에 남은 공간이 들어가있게 된다. 
                    //이걸 슬라이딩 윈도우라고 한다.
                    //socket.ReceiveBufferSize, SendBufferSize등으로 설정이 가능하다.
                }
            }
        }
    }
}
