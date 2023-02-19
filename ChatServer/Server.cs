using Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer;

internal class Server
{
    public static Server Instance = null;
    private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    private object lockObj = new object();

    public int RoomNumber {
        get {
            lock (lockObj)
            {
                _roomNumber++;
            }
            
            return _roomNumber;
        }
    }

    private int _roomNumber = 0;
    //서버가 보유하고 있는 방의 목록
    public ConcurrentDictionary<int, Room> Rooms {get;} = new ConcurrentDictionary<int, Room>();
    //서버에 접속중인 유저 목록


    private Dictionary<PacketType, Action<Socket, byte[]>> _handlerMap = null;

    public Server(string ip, int port, int backLog)
    {

        CreateMap();
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(backLog);
    }
    //패킷에 따른 맵 생성
    private void CreateMap()
    {
        _handlerMap = new Dictionary<PacketType, Action<Socket, byte[]>>();
        _handlerMap.Add(PacketType.LoginRequest, ServerPacketHandler.LoginRequestHandler);
        _handlerMap.Add(PacketType.CreateRoomRequest, ServerPacketHandler.CreateRoomRequestHandler);

    }

    public async Task StartAsync()
    {
        while(true)
        {
            //Task<Socket> 타입이니 리턴 타입은 Socket이 된다.
            Socket clientSocket = await serverSocket.AcceptAsync();
            Console.WriteLine(clientSocket.RemoteEndPoint);

            ThreadPool.QueueUserWorkItem(RunAsync, clientSocket);
        }
    }

    private async void RunAsync(object? sender)
    {
        Socket clientSocket = (Socket)sender;
        byte[] headerBuffer = new byte[2];
        //나중에 이부분은 리시브 버퍼로 바꾸자.
        while (true)
        {
            #region 헤더버퍼 가져옮 
            //2바이트 헤더만 먼저 가져오고 
            int n1 = await clientSocket.ReceiveAsync(headerBuffer, SocketFlags.None);
            if (n1 < 1)
            {
                Console.WriteLine("client disconnect");
                clientSocket.Dispose();
                return;
            }
            else if (n1 == 1)
            {
                await clientSocket.ReceiveAsync(new ArraySegment<byte>(headerBuffer, 1, 1), SocketFlags.None);
            }
            #endregion
            //헤더 가져왔다면 데이터를가져옴.
            #region 데이터버퍼 가져옮
            short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headerBuffer));
            byte[] dataBuffer = new byte[dataSize];

            int totalRecv = 0;
            while (totalRecv < dataSize)
            {
                int n2 = await clientSocket.ReceiveAsync(new ArraySegment<byte>(dataBuffer, totalRecv, dataSize - totalRecv), SocketFlags.None);
                totalRecv += n2;
            }
            #endregion

            //다 받았으면 타입 가져오자.
            //맨 처음 2바이트 읽어오고
            PacketType packet = (PacketType)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(dataBuffer));

            _handlerMap[packet](clientSocket, dataBuffer);
        }
    }
}
