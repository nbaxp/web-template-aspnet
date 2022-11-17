using Microsoft.AspNetCore.SignalR;

public class TestHub : Hub
{
    public override Task OnConnectedAsync()
    {
        this.Groups.AddToGroupAsync(Context.ConnectionId, Context.ConnectionId);
        this.Clients.Group(Context.ConnectionId).SendAsync("Connected", Context.ConnectionId);
        return base.OnConnectedAsync();
    }
}
