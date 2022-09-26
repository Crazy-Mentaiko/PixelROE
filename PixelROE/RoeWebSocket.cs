using System.Threading.Tasks;
using EmbedIO.WebSockets;

namespace PixelROE;

public class RoeWebSocket : WebSocketModule
{
    public RoeWebSocket(string urlPath = "/refresh") : base(urlPath, true)
    {
    }

    protected override async Task OnMessageReceivedAsync(IWebSocketContext context, byte[] buffer, IWebSocketReceiveResult result)
    {
        await context.WebSocket.SendAsync(buffer, false);
    }

    public async Task DoRefreshAsync()
    {
        await BroadcastAsync("refresh");
    }
}