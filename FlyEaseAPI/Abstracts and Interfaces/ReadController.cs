using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces;

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
    ///     Obtiene todos los elementos.
    /// </summary>
    /// <returns>Una lista de elementos.</returns>
    [HttpGet("GetAll")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autorizado, por favor solicitar el token", typeof(string))]
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
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = ex.Message, Success = false, response = lista });
        }
    }

    /// <summary>
    ///     Obtiene un elemento por ID.
    /// </summary>
    /// <param name="id">ID del elemento a obtener.</param>
    /// <returns>El elemento solicitado.</returns>
    [HttpGet("GetById/{id}")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Solicitud incorrecta, No se ha podido encontrar el elemento u objeto", typeof(string))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autorizado, por favor solicitar el token", typeof(string))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]
    public virtual async Task<IActionResult> GetById(IdType id)
    {
        try
        {
            var entity = await SetContextEntity(id);
            if (entity == null)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { mensaje = "No se ha encontrado", Success = true, response = entity });
            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Success = true, response = entity });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
        }
    }

    protected virtual async Task<List<TEntity>> SetContextList()
    {
        var list = await _context.Set<TEntity>().ToListAsync();
        return list;
    }

    protected virtual async Task<TEntity> SetContextEntity(IdType id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        return entity;
    }
}