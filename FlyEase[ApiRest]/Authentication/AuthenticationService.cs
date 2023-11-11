﻿using FlyEase_ApiRest_.Abstracts_and_Interfaces;
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

        public async Task<AuthenticationResponse> GetRefreshToken(int idusuario, RefreshTokenRequest refreshtoken = null)
        {
            var refreshTokenEncontrado = _context.Refreshtokens.FirstOrDefault(x =>
                x.Token == refreshtoken.ExpiredToken &&
                x.RefreshtokenAtributte == refreshtoken.RefreshToken &&
                x.IdUser == idusuario);

            if (refreshTokenEncontrado == null) return new AuthenticationResponse() { Succes = false, Msg = "No existe refresh Token" };

            var refreshTokenCreado = GenerateRefreshToken();
            var tokenCreado = GenerateToken(idusuario.ToString());
            return await SaveRefreshToken(idusuario, tokenCreado, refreshTokenCreado);
        }

        public async Task<AuthenticationResponse> GetToken([FromBody] Administrador admin = null)
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
                    return new AuthenticationResponse() { Msg = "No encontrado", Succes = false, Token = { AdminAuthentication = false, Token = null, RefreshToken = null } };
                }
                else
                {
                    admin = await _context.Administradores.FirstOrDefaultAsync(a => a.Usuario == admin.Usuario);
                    var token = GenerateToken(admin.Idadministrador.ToString());
                    var refreshtoken = GenerateRefreshToken();
                    await SaveRefreshToken(admin.Idadministrador, token, refreshtoken);
                    return new AuthenticationResponse() { Msg = "Administrador Valido", Succes = true, Token = { AdminAuthentication = true, Token = token, RefreshToken = refreshtoken } };
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
            DateTime? dateTime;
            string stringkey;
            if (Identifier == null)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "CommonUser"));
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, "User"));
                dateTime = DateTime.MaxValue;
            }
            else
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Identifier));
                dateTime = DateTime.UtcNow.AddSeconds(25);
            }
            stringkey = "FlyEaseWebApiTokenEncryptedKeyString";
            var keyBytes = Encoding.ASCII.GetBytes(stringkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = dateTime,
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

        private async Task<AuthenticationResponse> SaveRefreshToken(int idAdmin, string token, string refreshToken)
        {
            var RefreshToken = new Refreshtoken
            {
                IdUser = idAdmin,
                Token = token,
                RefreshtokenAtributte = refreshToken,
                Fechacreacion = DateTime.UtcNow,
                Fechaexpiracion = DateTime.UtcNow.AddDays(7)
            };
            await _context.Refreshtokens.AddAsync(RefreshToken);
            await _context.SaveChangesAsync();
            return new AuthenticationResponse { Token = new TokenClass { Token = token, RefreshToken = refreshToken, AdminAuthentication = true }, Succes = true, Msg = "Ok" };
        }
    }
}