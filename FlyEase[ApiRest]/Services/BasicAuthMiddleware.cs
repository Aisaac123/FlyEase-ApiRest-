using FlyEase_ApiRest_.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.Text;

namespace FlyEase_ApiRest_.Services
{
    using FlyEase_ApiRest_.Models.Contexto;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _realm;

        public BasicAuthMiddleware(RequestDelegate next, string realm, string username, string password)
        {
            _next = next;
            _realm = realm;
        }

        public async Task Invoke(HttpContext context, FlyEaseDataBaseContextAuthentication _context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            context.Response.Headers["Cache-Control"] = "no-store, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            context.Response.Headers["Expires"] = "0";
            context.Response.Headers["Max-Age"] = "0";
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int separatorIndex = usernamePassword.IndexOf(':');
                string username = usernamePassword.Substring(0, separatorIndex);

                string password = usernamePassword.Substring(separatorIndex + 1);
                using var connection = _context.Database.GetDbConnection() as NpgsqlConnection;
                await connection.OpenAsync();

                string clientIdWithPrefix;
                var command = new NpgsqlCommand();

                try
                {
                    clientIdWithPrefix = $@"\x{username}";
                    command = new NpgsqlCommand("SELECT desencriptar_cliente(@client_encryptId)", connection);
                    var parameter = new NpgsqlParameter("@client_encryptId", NpgsqlDbType.Varchar);
                    parameter.Value = clientIdWithPrefix;
                    command.Parameters.Add(parameter);

                    // Ejecuta el comando
                    var clientId = (string)await command.ExecuteScalarAsync();

                    var Cliente = await _context.ApiClients
                        .FirstOrDefaultAsync(item => item.Clientid == clientId && item.Activo);

                    if (Cliente == null)
                    {
                        // Mostrar mensaje de error si el cliente no se encuentra
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Cliente no encontrado");
                        return;
                    }
                    else
                    {
                        await _next.Invoke(context);
                        return;
                    }
                }
                catch (Exception)
                {
                    var list = await _context.Set<Administrador>().ToListAsync();

                    // Mostrar mensaje de error si la lista de administradores está vacía
                    if (list == null || !list.Exists(item => item.Usuario == username && item.Clave == password))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Error de autenticación");
                        return;
                    }
                    else
                    {

                        await _next.Invoke(context);
                        return;
                    }
                }
            }
            // Unauthorized access
            context.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
            context.Response.StatusCode = 401;
        }
    }

    public static class BasicAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder builder, string realm, string username, string password)
        {
            return builder.UseMiddleware<BasicAuthMiddleware>(realm, username, password);
        }
    }
}


//// Extract credentials
//string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
//Encoding encoding = Encoding.GetEncoding("UTF-8");
//string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

//int separatorIndex = usernamePassword.IndexOf(':');
//string username = usernamePassword.Substring(0, separatorIndex);

//string password = usernamePassword.Substring(separatorIndex + 1);
//using var connection = _context.Database.GetDbConnection() as NpgsqlConnection;
//await connection.OpenAsync();

//string clientIdWithPrefix;
//var command = new NpgsqlCommand();

//try
//{
//    clientIdWithPrefix = $@"\x{username}";
//    command = new NpgsqlCommand("SELECT desencriptar_cliente(@client_encryptId)", connection);
//    var parameter = new NpgsqlParameter("@client_encryptId", NpgsqlDbType.Varchar);
//    parameter.Value = clientIdWithPrefix;
//    command.Parameters.Add(parameter);

//    // Ejecuta el comando
//    var clientId = (string)await command.ExecuteScalarAsync();

//    var Cliente = await _context.ApiClients
//        .FirstOrDefaultAsync(item => item.Clientid == clientId && item.Activo);

//    if (Cliente == null)
//    {
//        // Mostrar mensaje de error si el cliente no se encuentra
//        context.Response.StatusCode = 401;
//        await context.Response.WriteAsync("Cliente no encontrado");
//        return;
//    }
//}
//catch (Exception)
//{
//    var list = await _context.Set<Administrador>().ToListAsync();

//    // Mostrar mensaje de error si la lista de administradores está vacía
//    if (list == null || !list.Exists(item => item.Usuario == username && item.Clave == password))
//    {
//        context.Response.StatusCode = 401;
//        await context.Response.WriteAsync("Error de autenticación");
//        return;
//    }
//}
//            }
//            await _next.Invoke(context);
//return;
//// Unauthorized access
//context.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
//context.Response.StatusCode = 401;
//        }