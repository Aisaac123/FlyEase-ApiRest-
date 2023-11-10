using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Commons;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public interface IAuthentication
    {
        Task<AuthenticationResponse> TokenAuthorization(Administrador admin = null);
        Task<AuthenticationResponse> RefreshTokenAuthorization(int idusuario, RefreshTokenRequest refreshtoken = null);

    }
}
