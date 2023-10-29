using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public interface IControllerCrud<T, Tkey> : IControllerRead<Tkey> where T : class
    {
        Task<IActionResult> Post([FromBody] T entity);
        Task<IActionResult> Delete(Tkey id);
        Task<IActionResult> Put(Tkey id, T entity);
        Task<IActionResult> DeleteAll();
    }
}
