using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient;

internal class NetworkManager
{
    public string Id { get; set; } = null;
    public string NickName { get; set; } = null;
    private Socket _socket;
    
    private Dictionary<PacketType, Action<Socket, byte[]>> _handlerMap = null;

    public event EventHandler<EventArgs>? LoginResponsed = null;
    public event EventHandler<EventArgs>? CreateRoomResponsed = null;

    public Socket Socket
    {
        get
        {
            if(_socket == null)
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return _socket;
        }
    }

    private static NetworkManager? _instance;

    public static NetworkManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new NetworkManager();
            return _instance;
        }
    }

    private NetworkManager()
    {
        //생성자 잠금
        //최초 생성시에 핸들러 맵 셋팅
        CreateMap();
    }

    //패킷에 따른 맵 생성
    private void CreateMap()
    {
        _handlerMap = new Dictionary<PacketType, Action<Socket, byte[]>>();
        _handlerMap.Add(PacketType.LoginResponse, ClientPacketHandler.LoginResponseHandler);
        _handlerMap.Add(PacketType.CreateRoomResponse, ClientPacketHandler.CreateRoomResponseHandler);
    }

    public async Task ConnectAsync()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("220.74.33.193"), 20000);

        await Socket.ConnectAsync(endPoint);

        //쓰레드 풀에 작업 할당, sender에는 소켓 보내줌.
        ThreadPool.QueueUserWorkItem(ReceiveAsync, Socket);
    }

    private async void ReceiveAsync(object? receiver)
    {
        Socket socket = (Socket)receiver;
        byte[] headerBuffer = new byte[2];

        while (true)
        {
            #region 헤더버퍼 가져옮 
            //2바이트 헤더만 먼저 가져오고 
            int n1 = await socket.ReceiveAsync(headerBuffer, SocketFlags.None);
            if (n1 < 1)
            {
                Console.WriteLine("server disconnect");
                socket.Dispose();
                return;
            }
            else if (n1 == 1)
            {
                await socket.ReceiveAsync(new ArraySegment<byte>(headerBuffer, 1, 1), SocketFlags.None);
            }
            #endregion
            //헤더 가져왔다면 데이터를가져옴.
            #region 데이터버퍼 가져옮
            short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headerBuffer));
            byte[] dataBuffer = new byte[dataSize];

            int totalRecv = 0;
            while (totalRecv < dataSize)
            {
                int n2 = await socket.ReceiveAsync(new ArraySegment<byte>(dataBuffer, totalRecv, dataSize - totalRecv), SocketFlags.None);
                totalRecv += n2;
            }
            #endregion
            //다 받았으면 타입 가져오자.
            //맨 처음 2바이트 읽어오고
            PacketType packet = (PacketType)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(dataBuffer));

            _handlerMap[packet](socket, dataBuffer);
        }
    }


    public void LoginResponse(LoginResponsePacket packet)
    {
        LoginResponsed?.Invoke(null, packet);
    }
    public void CreateRoomResponse(CreateRoomResponsePacket packet)
    {
        CreateRoomResponsed?.Invoke(null, packet);
    }
}
