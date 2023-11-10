using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class EstadosController : ReadController<Estado, int, FlyEaseDataBaseContextAuthentication>
    {
        public EstadosController(FlyEaseDataBaseContextAuthentication context) : base(context)
        {
            _context = context;
        }
    }
}