using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public class CreateRoomRequestPacket : IPacket
{
    public string RoomName { get; private set; }

    public CreateRoomRequestPacket(string roomName)
    {
        this.RoomName = roomName;
    }

    public CreateRoomRequestPacket(byte[] buffer)
    {
        int offset = 2;
        short nameSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        RoomName = Encoding.UTF8.GetString(buffer, offset, nameSize);
    }

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.CreateRoomRequest));
        byte[] roomName = Encoding.UTF8.GetBytes(RoomName);
        byte[] roomNameSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)roomName.Length));

        short dataSize = (short)(packetType.Length + roomNameSize.Length + roomName.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;
        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;

        Array.Copy(roomNameSize, 0, buffer, offset, roomNameSize.Length);
        offset += roomNameSize.Length;

        Array.Copy(roomName, 0, buffer, offset, roomName.Length);

        return buffer;
    }
}
