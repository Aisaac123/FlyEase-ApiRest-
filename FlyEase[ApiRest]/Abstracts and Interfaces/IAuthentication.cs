using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Commons;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public interface IAuthentication
    {
        Task<AuthenticationResponse> GetToken(Administrador admin = null);
        Task<AuthenticationResponse> GetRefreshToken(int idusuario, RefreshTokenRequest refreshtoken = null);

    }
}
