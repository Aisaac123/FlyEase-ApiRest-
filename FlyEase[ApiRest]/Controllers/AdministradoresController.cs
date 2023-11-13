using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Authentication;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class AdministradoresController : ReadController<Administrador, int, FlyEaseDataBaseContextAuthentication>
    {
        private readonly IAuthentication _aut;

        public AdministradoresController(FlyEaseDataBaseContextAuthentication context, IAuthentication aut) : base(context)
        {
            _context = context;
            _aut = aut;
        }
        [HttpGet]
        [Route("GetByUsername/{AdminUsername}")]
       // [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> GetByUsername(string AdminUsername)
        {
            try
            {
                var Admin = await _context.Administradores
         .FirstOrDefaultAsync(a => a.Usuario == AdminUsername);
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

        [HttpPost]
        [Route("Authentication")]

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

        [HttpPost]
        [Route("GetRefreshToken")]

        public async Task<IActionResult> ObtenerRefreshToken([FromBody] RefreshTokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(request.ExpiredToken);
            if (tokenExpiradoSupuestamente.ValidTo > DateTime.UtcNow)
                return BadRequest(new AuthenticationResponse { Succes = false, Msg = "Token Vigente" });
            var token = await _context.Refreshtokenviews.FirstOrDefaultAsync(a => a.Refreshtoken == request.RefreshToken && a.Token == request.ExpiredToken);
            string idUsuario = tokenExpiradoSupuestamente.Claims.First(x =>
            x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();
            var autorizacionResponse = await _aut.GetRefreshToken(int.Parse(idUsuario), request);
            if (autorizacionResponse.Succes)
                return Ok(autorizacionResponse);
            else
                return BadRequest(autorizacionResponse);
        }
    }
}