using System;
using System.Threading;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Files;
using EmbedIO.WebApi;

namespace PixelROE;

public class PixelRoeWebServer : IDisposable
{
    private readonly WebServer _webServer;
    private readonly RoeController _roeController;
    private readonly RoeWebSocket _roeWebSocket;

    private readonly Thread _webServerBodyThread;
    private readonly CancellationTokenSource _webServerCancellationToken;

    public RoeController Controller => _roeController;
    
    public PixelRoeWebServer()
    {
        _roeController = new RoeController("/index");
        _roeWebSocket = new RoeWebSocket("/refresh");
        
        _webServer = new WebServer(o => o
                .WithUrlPrefix("http://localhost:4000/")
                .WithMode(HttpListenerMode.EmbedIO))
            .WithLocalSessionManager()
            .WithModule(_roeController)
            .WithModule(_roeWebSocket)
            .WithStaticFolder("/images", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Images"),
                true, m => m.WithContentCaching(true));

        _webServerCancellationToken = new CancellationTokenSource();
        _webServerBodyThread = new Thread(ServerBody);
        _webServerBodyThread.Start();
    }

    public void Dispose()
    {
        _webServerCancellationToken.Cancel();
        _webServerBodyThread.Join();
        _webServer.Dispose();
    }

    private async void ServerBody()
    {
        await _webServer.RunAsync(_webServerCancellationToken.Token);
    }

    public async Task DoRefreshAsync()
    {
        await _roeWebSocket.DoRefreshAsync();
    }
}