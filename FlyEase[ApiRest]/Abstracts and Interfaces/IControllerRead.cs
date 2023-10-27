using Microsoft.AspNetCore.Mvc;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public interface IControllerRead<IdType>
    {
        Task<IActionResult> Get();
        Task<IActionResult> GetById(IdType id);

    }
}
