using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    [EnableCors("Reglas")]
    [Route("FlyEaseApi/[controller]")]
    [ApiController]
    public abstract class CrudController<TEntity, IdType, TContext> : ReadController<TEntity, IdType, TContext>
        where TEntity : class
        where TContext : DbContext, new()
    {
        private readonly IHubContext<WebSocketHub> _hubContext;

        protected CrudController(TContext context, IHubContext<WebSocketHub> hubContext) : base(context)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Crea un nuevo elemento.
        /// </summary>
        
        [HttpPost("Post")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Se ha generado un conflicto en la base de datos, por favor rectifica los datos que deseas ingresar", typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "No autorizado, por favor solicitar el token")]
        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            try
            {
                var mensaje = await InsertProcedure(entity);
                if (mensaje == "Ok")
                {
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("UpdateRequest", "Se ha insertado en la base de datos");

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operación realizada con éxito", Success = true, response = entity });
                }
                return StatusCode(StatusCodes.Status409Conflict, new { mensaje = mensaje, Success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
            }
        }

        /// <summary>
        /// Actualiza un elemento por ID.
        /// </summary>
        
        [HttpPut("Put/{Id}")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Se ha generado un conflicto en la base de datos, por favor rectifica los datos que deseas ingresar", typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autorizado, por favor solicitar el token", typeof(string))]
        public virtual async Task<IActionResult> Put([FromBody] TEntity entity, IdType Id)
        {
            try
            {
                var mensaje = await UpdateProcedure(entity, Id);
                if (mensaje.ToString() == "Ok")
                {
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("UpdateRequest", "Se ha actualizado en la base de datos");

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operación realizada con éxito", Success = true, response = entity });
                }
                return StatusCode(StatusCodes.Status409Conflict, new { mensaje = mensaje, Success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
            }
        }

        /// <summary>
        /// Elimina un elemento por ID.
        /// </summary>
        
        [HttpDelete("Delete/{Id}")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Se ha generado un conflicto en la base de datos, por favor rectifica los datos que deseas ingresar", typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autorizado, por favor solicitar el token", typeof(string))]
        public virtual async Task<IActionResult> Delete(IdType Id)
        {
            try
            {
                var mensaje = await DeleteProcedure(Id);
                if (mensaje.ToString() == "Ok")
                {
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("UpdateRequest", "Se ha eliminado en la base de datos");

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operación realizada con éxito", Success = true });
                }
                return StatusCode(StatusCodes.Status409Conflict, new { mensaje = mensaje, Success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
            }
        }

        /// <summary>
        /// Elimina todos los elementos.
        /// </summary>
        
        [HttpDelete("DeleteAll")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autorizado, por favor solicitar el token", typeof(string))]
        public virtual async Task<IActionResult> DeleteAll()
        {
            try
            {
                var entities = await _context.Set<TEntity>().ToListAsync();
                _context.Set<TEntity>().RemoveRange(entities);
                _context.SaveChanges();

                await _hubContext.Clients.All.SendAsync("UpdateRequest", "Se ha eliminado en la base de datos");

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
            }
        }

        protected abstract Task<string> InsertProcedure(TEntity entity);

        protected abstract Task<string> UpdateProcedure(TEntity entity, IdType OldId);

        protected abstract Task<string> DeleteProcedure(IdType Old_Id);
    }
}