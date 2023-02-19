using Core;
using System.Net.Sockets;

namespace ChatClient;

public class ClientPacketHandler
{
    public static void LoginResponseHandler(Socket sender, byte[] payload)
    {
        LoginResponsePacket packet = new LoginResponsePacket(payload);

        NetworkManager.Instance.LoginResponse(packet);
    }

    public static void CreateRoomResponseHandler(Socket sender, byte[] payload)
    {
        CreateRoomResponsePacket packet = new CreateRoomResponsePacket(payload);
        NetworkManager.Instance.CreateRoomResponse(packet);
    }
}
