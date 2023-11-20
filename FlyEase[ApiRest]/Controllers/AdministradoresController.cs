using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Authentication;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    [Route("FlyEaseApi/[controller]")]
    [ApiController]

    /// <summary>
    /// Controlador para gestionar operaciones del administrador y sus respectivas autenticaciones.
    /// </summary>
    [SwaggerTag("Metodos de autenticacion y lectura para Administradores")]

    public class AdministradoresController : ReadController<Administrador, int, FlyEaseDataBaseContextAuthentication>
    {
        private readonly IAuthentication _aut;

        /// <summary>
        /// Constructor del controlador de Adminstradores.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public AdministradoresController(FlyEaseDataBaseContextAuthentication context, IAuthentication aut) : base(context)
        {
            _context = context;
            _aut = aut;
        }

        /// <summary>
        /// Obtiene un administrador por nombre de usuario.
        /// </summary>

        [HttpGet]
        [Route("GetByUsername/{AdminUsername}")]
        [SwaggerOperation("Obtener un administrador por nombre de usuario.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(Administrador))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta", typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
        public async Task<IActionResult> GetByUsername(string AdminUsername)
        {
            try
            {
                var Admin = await _context.Administradores.FirstOrDefaultAsync(a => a.Usuario == AdminUsername);
                if (Admin == null)
                {
                    return BadRequest("No se ha encontrado");
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = Admin });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Autentica un administrador.
        /// </summary>

        [HttpPost]
        [Route("Authentication")]
        [SwaggerOperation("Autenticar y autorizar un administrador por medio de JWT (Json Web Token) usando sus credenciales de usuario y contraseña.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Administrador no autorizado: credenciales erroneas o inactivo", typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
        public async Task<IActionResult> Authentication([FromBody] Administrador admin)
        {
            try
            {
                var Aut = await _aut.GetToken(admin);
                if (!Aut.Succes)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { response = Aut });
                }
                return StatusCode(StatusCodes.Status200OK, new { response = Aut });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un nuevo token de actualización.
        /// </summary>

        [HttpPost]
        [Route("GetRefreshToken")]
        [SwaggerOperation("Refrescar un nuevo token de actualización por medio de RT (Refresh Token). Recibe el token expirado y el refresh token.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(AuthenticationResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta, El token se encuentra vigente", typeof(string))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto: RefreshToken no registrado", typeof(string))]

        public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(request.ExpiredToken);
            if (tokenExpiradoSupuestamente.ValidTo > DateTime.UtcNow)
                return BadRequest(new AuthenticationResponse { Succes = false, Msg = "Token Vigente" });
            string idUsuario = tokenExpiradoSupuestamente.Claims.First(x =>
            x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();
            var autorizacionResponse = await _aut.GetRefreshToken(int.Parse(idUsuario), request);
            if (autorizacionResponse.Succes)
                return StatusCode(StatusCodes.Status200OK, new { response = autorizacionResponse });
            else
                return StatusCode(StatusCodes.Status409Conflict, new { response = autorizacionResponse });
        }

        // Documentación de métodos heredados de la clase abstracta.

        /// <summary>
        /// Obtiene todos los Administradores.
        /// </summary>

        [HttpGet("GetAll")]
        [SwaggerOperation("Obtener todos los administradores.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(List<Administrador>))]
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }

        /// <summary>
        /// Obtiene un elemento por ID heredado de la clase base.
        /// </summary>
        /// <param name="id">ID del elemento a obtener.</param>
        /// <returns>El elemento solicitado.</returns>

        [HttpGet("GetById/{id}")]
        [SwaggerOperation("Obtener un administrador por ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(Administrador))]
        public override async Task<IActionResult> GetById(int id)
        {
            var func = await base.GetById(id);
            return func;

        }
    }
}