namespace ChatServer;

internal class Program
{
    const string ip = "220.74.33.193";
    static async Task Main(string[] args)
    {
        Server.Instance = new Server(ip, 20000, 10);
        await Server.Instance.StartAsync(); //여기서 아무것도 반환 안하니까 걍 하염없이 기다리게 돼.
    }
}
