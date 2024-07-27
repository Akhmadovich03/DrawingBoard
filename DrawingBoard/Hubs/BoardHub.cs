using Microsoft.AspNetCore.SignalR;

namespace DrawingBoard.Hubs;

public class BoardHub : Hub
{
    public async Task SendDrawing(string user, string drawingData)
    {
        await Clients.Others.SendAsync("ReceiveDrawing", user, drawingData);
    }
}