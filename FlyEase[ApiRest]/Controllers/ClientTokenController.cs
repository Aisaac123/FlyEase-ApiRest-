using FlyEase_ApiRest_.Authentication;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System.IdentityModel.Tokens.Jwt;

namespace FlyEase_ApiRest_.Controllers
{

    /// <summary>
    /// Controlador para la generación de tokens de los aplicativos registrados.
    /// </summary>
    
    [EnableCors("Reglas")]
    [Route("FlyEaseApi/[controller]")]
    [ApiController]
    public class ClientTokenController : ControllerBase
    {
        private readonly IAuthentication _aut;
        private readonly FlyEaseDataBaseContextAuthentication _context;

        /// <summary>
        /// Constructor del controlador de tokens del aplicativo.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        /// <param name="aut">Servicio de autenticación.</param>
        
        public ClientTokenController(FlyEaseDataBaseContextAuthentication context, IAuthentication aut)
        {
            _context = context;
            _aut = aut;
        }

        /// <summary>
        /// Genera un token de cliente.
        /// </summary>
        /// <param name="apiclient">Datos del cliente para generar el token.</param>
        /// <returns>Resultados de la generación del token.</returns>
        
        [HttpPost]
        [Route("GenerateClientToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateClientToken([FromBody] ApiClient apiclient)
        {
            try
            {
                using var connection = _context.Database.GetDbConnection() as NpgsqlConnection;
                await connection.OpenAsync();
                string originalClientId = apiclient.Clientid;
                string clientIdWithPrefix = $@"\x{originalClientId}";
                using var command = new NpgsqlCommand("SELECT desencriptar_cliente(@client_encryptId)", connection);

                // Configura el parámetro @client_encryptId
                var parameter = new NpgsqlParameter("@client_encryptId", NpgsqlDbType.Varchar);
                parameter.Value = clientIdWithPrefix;  // Asigna el valor correspondiente
                command.Parameters.Add(parameter);

                // Ejecuta el comando
                var clientId = (string)await command.ExecuteScalarAsync();

                var Cliente = await _context.ApiClients
                    .FirstOrDefaultAsync(item => item.Clientid == clientId && item.Activo);

                if (Cliente == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Aplicativo no registrado o se encuentra inactivo." });
                }

                if (string.IsNullOrEmpty(Cliente.Token))
                {
                    var Aut = await _aut.GetToken();
                    if (!Aut.Succes)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new { Token = "", AdminAuthorization = false });
                    }
                    Cliente.Token = Aut.Tokens.PrimaryToken;
                    _context.ApiClients.Update(Cliente);
                    await _context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, new { Cliente.Token, AdminAuthorization = false });
                }

                var Token = new JwtSecurityTokenHandler().ReadJwtToken(Cliente.Token);
                if (Token.ValidTo <= DateTime.UtcNow.AddMinutes(1))
                {
                    var Aut = await _aut.GetToken();
                    if (!Aut.Succes)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized, new { Token = "", AdminAuthorization = false });
                    }
                    Cliente.Token = Aut.Tokens.PrimaryToken;
                    _context.ApiClients.Update(Cliente);
                    await _context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, new { Cliente.Token, AdminAuthorization = false });
                }

                return StatusCode(StatusCodes.Status200OK, new { Cliente.Token, AdminAuthorization = false });
            }
            catch (PostgresException ex) when (ex.SqlState == "39000" && ex.Message.Contains("Wrong key or corrupt data"))
            {
                // Manejar la excepción específica aquí
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "ClientId Format Error" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}