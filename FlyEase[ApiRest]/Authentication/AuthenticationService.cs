using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlyEase_ApiRest_.Authentication
{
    public class AuthenticationService : IAuthentication
    {
        private readonly FlyEaseDataBaseContextPrueba _context;
        private readonly IConfiguration _config;
        public AuthenticationService(FlyEaseDataBaseContextPrueba context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<AuthenticationResponse> Authentication([FromBody] Administrador admin = null)
        {
            try
            {
                if (admin == null) 
                {
                    var token = GenerateKeyBytes();

                    return new AuthenticationResponse() { Msg = "Usuario Valido", Succes = true, Token = { IsAdmin = false, Token = token } };
                }
                var AdminExist = await _context.Administradores
         .AnyAsync(a => a.Usuario == admin.Usuario && admin.Clave == a.Clave);
                if (!AdminExist)
                {
                    var token = GenerateKeyBytes();

                    return new AuthenticationResponse() { Msg = "No encontrado", Succes = false, Token = { IsAdmin = false, Token = "" } };
                }
                else
                {
                    var token = GenerateKeyBytes(admin.Usuario);
                    return new AuthenticationResponse() { Msg = "Administrador Valido", Succes = true, Token = { IsAdmin = true, Token = token} };
                }

            }
            catch (Exception ex)
            {
                return new AuthenticationResponse() { Msg = ex.Message, Succes = false, Token = null };
            }
        }
        private string GenerateKeyBytes(string Identifier = null)
        {
            var claims = new ClaimsIdentity();
            string stringkey;
            if (Identifier == null)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "CommonUser"));
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, "User"));
            }
            else
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Identifier));
            }
            stringkey = "FlyEaseWebApiTokenEncryptedKeyForAdmin";
            var keyBytes = Encoding.ASCII.GetBytes(stringkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokencreado = tokenHandler.WriteToken(tokenConfig);
            return tokencreado;
        }
    }
}
