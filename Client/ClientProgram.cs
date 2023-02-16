using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

internal class ClientProgram
{
    static void Main(string[] args)
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
