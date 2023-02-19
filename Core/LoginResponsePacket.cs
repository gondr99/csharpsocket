using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public class LoginResponsePacket : EventArgs, IPacket
{
    public int Code { get; private set; }

    public LoginResponsePacket(int code)
    {
        this.Code = code;
    }

    public LoginResponsePacket(byte[] buffer)
    {
        Code = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buffer, 2));
    }

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.LoginResponse));
        byte[] code = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Code));

        short dataSize = (short)(packetType.Length + code.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));
        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;

        Array.Copy(code, 0, buffer, offset, code.Length);

        return buffer;
    }
}
