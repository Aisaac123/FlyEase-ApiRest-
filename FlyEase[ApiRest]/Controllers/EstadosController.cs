using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class EstadosController : ReadController<Estado, int, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de estados.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public EstadosController(FlyEaseDataBaseContextAuthentication context) : base(context)
        {
            _context = context;
        }

        // Documentación de métodos heredados de la clase abstracta

        /// <summary>
        /// Obtiene todos los elementos heredados de la clase base.
        /// </summary>

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(List<Administrador>), StatusCodes.Status200OK)] // Actualizado a Administrador
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }

        /// <summary>
        /// Obtiene un elemento por ID heredado de la clase base.
        /// </summary>
        /// <param name="id">ID del elemento a obtener.</param>
        /// <returns>El elemento solicitado.</returns>

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Administrador), StatusCodes.Status200OK)] // Actualizado a Administrador
        public override async Task<IActionResult> GetById(int id)
        {
            var func = await base.GetById(id);
            return func;

        }
    }
}