using Core;
using System.Net.Sockets;

namespace ChatServer;
public class ServerPacketHandler
{
    //이쪽은 서버
    public static async void LoginRequestHandler(Socket sender, byte[] payload)
    {
        LoginRequestPacket packet = new LoginRequestPacket(payload);
        Console.WriteLine($"id: {packet.Id}, nickname: {packet.Nickname}");

        LoginResponsePacket response = new LoginResponsePacket(200);//응답 성공

        await sender.SendAsync(response.Serialize(), SocketFlags.None);
    }

    public static async void CreateRoomRequestHandler(Socket sender, byte[] payload)
    {
        CreateRoomRequestPacket packet = new CreateRoomRequestPacket(payload);

        Room room = new Room();
        int no = Server.Instance.RoomNumber;
        if(Server.Instance.Rooms.TryAdd(no, room))
        {
            Console.WriteLine(packet.RoomName);

            CreateRoomResponsePacket response = new CreateRoomResponsePacket(200);
            await sender.SendAsync(response.Serialize(), SocketFlags.None);
        }
        else
        {
            //룸생성 실패시
            Console.WriteLine("Created failed");
            CreateRoomResponsePacket response = new CreateRoomResponsePacket(500);
            await sender.SendAsync(response.Serialize(), SocketFlags.None);
        }
        

    }

}
