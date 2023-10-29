using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;

namespace FlyEase_ApiRest_.Controllers
{
    public class EstadosController : ReadController<Estado, int, FlyEaseDataBaseContext>
    {
    }
}