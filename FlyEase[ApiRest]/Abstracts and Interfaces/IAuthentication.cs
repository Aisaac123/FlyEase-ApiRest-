using FlyEase_ApiRest_.Models;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public interface IAuthentication
    {
        Task<AuthenticationResponse> Authentication(Administrador admin = null);
    }
}
