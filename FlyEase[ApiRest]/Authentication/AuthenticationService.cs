using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlyEase_ApiRest_.Authentication
{
    public class AuthenticationService : IAuthentication
    {
        private readonly FlyEaseDataBaseContextAuthentication _context;
        private readonly IConfiguration _config;
        public AuthenticationService(FlyEaseDataBaseContextAuthentication context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public Task<AuthenticationResponse> RefreshTokenAuthorization(int idusuario, RefreshTokenRequest refreshtoken = null)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResponse> TokenAuthorization([FromBody] Administrador admin = null)
        {
            try
            {
                if (admin == null)
                {
                    var token = GenerateToken();

                    return new AuthenticationResponse() { Msg = "Usuario Valido", Succes = true, Token = { AdminAuthentication = false, Token = token } };
                }
                var AdminExist = await _context.Administradores
         .AnyAsync(a => a.Usuario == admin.Usuario && admin.Clave == a.Clave);
                if (!AdminExist)
                {
                    var token = GenerateToken();

                    return new AuthenticationResponse() { Msg = "No encontrado", Succes = false, Token = { AdminAuthentication = false, Token = "" } };
                }
                else
                {
                    var token = GenerateToken(admin.Usuario);
                    return new AuthenticationResponse() { Msg = "Administrador Valido", Succes = true, Token = { AdminAuthentication = true, Token = token } };
                }

            }
            catch (Exception ex)
            {
                return new AuthenticationResponse() { Msg = ex.Message, Succes = false, Token = null };
            }
        }
        private string GenerateToken(string Identifier = null)
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

        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }
        private async Task<AuthenticationResponse> SaveRefreshToken(int idUsuario,string token,string refreshToken
)
        {
            var RefreshToken = new Refreshtoken
            {
                IdUser = idUsuario,
                Token = token,
                RefreshtokenAtributte = refreshToken,
                Fechacreacion = DateTime.UtcNow,
                Fechaexpiracion = DateTime.UtcNow.AddMinutes(2)
            };
            await _context.Refreshtokens.AddAsync(RefreshToken);
            await _context.SaveChangesAsync();
            return new AuthenticationResponse { Token = new TokenClass { Token = token, RefreshToken = refreshToken, AdminAuthentication = true}, Succes = true, Msg = "Ok" };
        }
    }
}
