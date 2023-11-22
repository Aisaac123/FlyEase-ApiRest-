using FlyEase_ApiRest_.Models.Contexto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace FlyEase_ApiRest_.Hub
{
    public class DbListener
    {
        private readonly IHubContext<WebSocketHub> _hubContext;
        private readonly IServiceProvider _serviceProvider;

        public DbListener(IHubContext<WebSocketHub> hubContext, IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _serviceProvider = serviceProvider;
        }

        public async void IniciarEscuchaCambiosEnVuelos()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FlyEaseDataBaseContextAuthentication>();

                using (var conn = dbContext.Database.GetDbConnection() as NpgsqlConnection)
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    using (var cmd = new NpgsqlCommand("LISTEN vuelos_change", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    conn.Notification += async (sender, args) =>
                    {
                        await _hubContext.Clients.All.SendAsync("ActualizarVuelos");
                    };

                    while (true)
                    {
                        await conn.WaitAsync();
                    }
                }
            }
        }
    }
}