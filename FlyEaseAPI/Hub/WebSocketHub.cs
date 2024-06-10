using Microsoft.AspNetCore.SignalR;

public class WebSocketHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        // Notificar a otros clientes sobre la conexión
        await Clients.All.SendAsync("UsuarioConectado", connectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;

        // Notificar a otros clientes sobre la desconexión
        await Clients.All.SendAsync("UsuarioDesconectado", connectionId);

        await base.OnDisconnectedAsync(exception);
    }
}