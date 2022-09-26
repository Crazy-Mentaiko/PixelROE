using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.WebApi;

namespace PixelROE;

public class RoeController : WebModuleBase
{
    public override bool IsFinalHandler { get; }

    public List<string> ShowImageList { get; } = new();
    
    public RoeController(string baseRoute = "/")
        : base(baseRoute)
    {
    }
    
    protected override async Task OnRequestAsync(IHttpContext context)
    {
        await using var outputStream = context.Response.OutputStream;
        await using var streamWriter = new StreamWriter(outputStream, Encoding.UTF8, -1, true);

        await streamWriter.WriteLineAsync("<!DOCTYPE html>");
        await streamWriter.WriteLineAsync("<html>");
        await streamWriter.WriteLineAsync("<head>");
        await streamWriter.WriteLineAsync("<meta charset=\"utf-8\" />");
        await streamWriter.WriteLineAsync("<title>PixelROE</title>");
        await streamWriter.WriteLineAsync("<script>");
        await streamWriter.WriteLineAsync("var socket = new WebSocket(\"ws://localhost:4000/refresh\");");
        await streamWriter.WriteLineAsync("socket.onopen = function(event) {");
        await streamWriter.WriteLineAsync("    console.log(\"[PixelROE] AUTO REFRESH READY.\");");
        await streamWriter.WriteLineAsync("}");
        await streamWriter.WriteLineAsync("socket.onmessage = function(event) {");
        await streamWriter.WriteLineAsync("    location.reload();");
        await streamWriter.WriteLineAsync("}");
        await streamWriter.WriteLineAsync("socket.onclose = function(event) {");
        await streamWriter.WriteLineAsync("    console.log(\"[PixelROE] AUTO REFRESH DISCONNECTED.\");");
        await streamWriter.WriteLineAsync("}");
        await streamWriter.WriteLineAsync("socket.onerror = function(error) {");
        await streamWriter.WriteLineAsync("    console.log(`[PixelROE] AUTO REFRESH ERROR: ${error.message}`);");
        await streamWriter.WriteLineAsync("}");
        await streamWriter.WriteLineAsync("</script>");
        await streamWriter.WriteLineAsync("</head>");
        await streamWriter.WriteLineAsync("<body style=\"margin:0px;background-color:#ff00ff;\">");
        foreach (var path in ShowImageList)
        {
            await streamWriter.WriteLineAsync($"<img src=\"images/{path["Assets\\Images\\".Length..]}\" />");
        }
        await streamWriter.WriteLineAsync("</body>");
        await streamWriter.WriteLineAsync("</html>");
        
        await streamWriter.FlushAsync();
    }
}