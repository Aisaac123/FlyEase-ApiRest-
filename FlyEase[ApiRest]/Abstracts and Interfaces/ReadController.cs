using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    [EnableCors("Reglas")]
    [Route("FlyEaseApi/[controller]")]
    [ApiController]
    public abstract class ReadController<TEntity, IdType, TContext> : ControllerBase, IControllerRead<IdType>
        where TEntity : class
        where TContext : DbContext
    {
        protected TContext _context;

        public ReadController(TContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los elementos.
        /// </summary>
        /// <returns>Una lista de elementos.</returns>
        [HttpGet("GetAll")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "No autorizado, por favor solicitar el token")]

        public virtual async Task<IActionResult> Get()
        {
            List<TEntity> lista = new();
            try
            {
                lista = await SetContextList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Success = true, response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false, response = lista });
            }
        }

        /// <summary>
        /// Obtiene un elemento por ID.
        /// </summary>
        /// <param name="id">ID del elemento a obtener.</param>
        /// <returns>El elemento solicitado.</returns>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "No autorizado, por favor solicitar el token")]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public virtual async Task<IActionResult> GetById(IdType id)
        {
            try
            {
                var entity = await SetContextEntity(id);
                if (entity == null)
                {
                    return BadRequest("No se ha encontrado");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Success = true, response = entity });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
            }
        }

        protected async virtual Task<List<TEntity>> SetContextList()
        {
            var list = await _context.Set<TEntity>().ToListAsync();
            return list;
        }

        protected async virtual Task<TEntity> SetContextEntity(IdType id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity;
        }
    }
}