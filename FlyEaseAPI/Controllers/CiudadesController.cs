using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Contexto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Swashbuckle.AspNetCore.Annotations;

namespace FlyEase_ApiRest_.Controllers;

[EnableCors("Reglas")]
/// <summary>
/// Controlador para gestionar operaciones CRUD de Ciudades.
/// </summary
[SwaggerTag("Metodos Crud para Ciudades")]
public class CiudadesController : CrudController<Ciudad, int, FlyEaseDataBaseContextAuthentication>
{
    /// <summary>
    ///     Constructor del controlador de Ciudades.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="hubContext">Contexto del hub.</param>
    public CiudadesController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) :
        base(context, hubContext)
    {
        _context = context;
    }

    /// <summary>
    ///     Crea un nuevo registro de Ciudad.
    /// </summary>
    /// <param name="entity">Datos de la Ciudad a crear.</param>
    /// <returns>Respuesta de la solicitud.</returns>
    [HttpPost("Post")]
    [Authorize(Policy = "Admin Policy")]
    [SwaggerOperation("Registrar una ciudad.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
    public override async Task<IActionResult> Post([FromBody] Ciudad entity)
    {
        var func = await base.Post(entity);
        return func;
    }

    /// <summary>
    ///     Actualiza un registro de Ciudad en la base de datos por su ID.
    /// </summary>
    /// <param name="entity">Datos de la Ciudad a actualizar.</param>
    /// <param name="Id">ID de la Ciudad a actualizar.</param>
    /// <returns>Respuesta de la solicitud.</returns>
    [HttpPut("Put/{Id}")]
    [Authorize(Policy = "Admin Policy")]
    [SwaggerOperation("Actualizar una ciudad especifica por medio de su ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
    public override async Task<IActionResult> Put([FromBody] Ciudad entity, int Id)
    {
        var func = await base.Put(entity, Id);
        return func;
    }

    /// <summary>
    ///     Elimina un registro de Ciudad de la base de datos por su ID.
    /// </summary>
    /// <param name="Id">ID de la Ciudad a eliminar.</param>
    /// <returns>Respuesta de la solicitud.</returns>
    [HttpDelete("Delete/{Id}")]
    [SwaggerOperation("Eliminar una ciudad especifica por medio de su ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
    public override async Task<IActionResult> Delete(int Id)
    {
        var func = await base.Delete(Id);
        return func;
    }

    /// <summary>
    ///     Elimina todos los registros de Ciudades de la base de datos.
    /// </summary>
    /// <returns>Respuesta de la solicitud.</returns>
    [HttpDelete("DeleteAll")]
    [SwaggerOperation("Eliminar todas las ciudades registradas (Usar Con Precaucion).")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
    public override async Task<IActionResult> DeleteAll()
    {
        var func = await base.DeleteAll();
        return func;
    }

    /// <summary>
    ///     Obtiene la lista de Ciudades desde la base de datos.
    /// </summary>
    /// <returns>Respuesta de la solicitud.</returns>
    [HttpGet("GetAll")]
    [Authorize]
    [SwaggerOperation("Obtener todas las ciudades registradas.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(List<Ciudad>))]
    public override async Task<IActionResult> Get()
    {
        var func = await base.Get();
        return func;
    }

    /// <summary>
    ///     Obtiene un registro de Ciudad desde la base de datos por su ID.
    /// </summary>
    /// <param name="id">ID de la Ciudad a obtener.</param>
    /// <returns>Respuesta de la solicitud.</returns>
    [HttpGet("GetById/{id}")]
    [Authorize(Policy = "Admin Policy")]
    [SwaggerOperation("Obtener una ciudad por medio de su ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(Ciudad))]
    public override async Task<IActionResult> GetById(int id)
    {
        var func = await base.GetById(id);
        return func;
    }

    /// <summary>
    ///     Inserta un nuevo registro de Ciudad en la base de datos.
    /// </summary>
    /// <param name="entity">Datos de la Ciudad a insertar.</param>
    /// <returns>Resultado de la operación.</returns>
    protected override async Task<string> InsertProcedure(Ciudad entity)
    {
        try
        {
            NpgsqlParameter v_imagen;

            if (entity.Imagen != null)
                v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                {
                    Value = entity.Imagen // Valor de la imagen si no es nulo
                };
            else
                v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                {
                    Value = DBNull.Value // Valor nulo
                };

            var parameters = new[]
            {
                new("nombre_ciudad", entity.Nombre),
                new("nombre_region", entity.Region.Nombre),
                new("nombre_pais", entity.Region.Pais.Nombre),
                v_imagen
            };
            await _context.Database.ExecuteSqlRawAsync(
                "CALL p_insertar_ciudad(@nombre_ciudad, @nombre_region, @nombre_pais, @_imagen)", parameters);
            return "Ok";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    ///     Elimina un registro de Ciudad de la base de datos por su ID.
    /// </summary>
    /// <param name="id_ciudad">ID de la Ciudad a eliminar.</param>
    /// <returns>Resultado de la operación.</returns>
    protected override async Task<string> DeleteProcedure(int id_ciudad)
    {
        try
        {
            var parameters = new NpgsqlParameter[]
            {
                new("id_ciudad", id_ciudad)
            };

            await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_ciudad(@id_ciudad)", parameters);
            return "Ok";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    ///     Actualiza un registro de Ciudad en la base de datos por su ID.
    /// </summary>
    /// <param name="nuevaCiudad">Nueva información de la Ciudad a actualizar.</param>
    /// <param name="id_ciudad">ID de la Ciudad a actualizar.</param>
    /// <returns>Resultado de la operación.</returns>
    protected override async Task<string> UpdateProcedure(Ciudad nuevaCiudad, int id_ciudad)
    {
        try
        {
            NpgsqlParameter v_imagen;

            if (nuevaCiudad.Imagen != null)
                v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                {
                    Value = nuevaCiudad.Imagen // Valor de la imagen si no es nulo
                };
            else
                v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                {
                    Value = DBNull.Value // Valor nulo
                };
            var parameters = new[]
            {
                new("id_ciudad", id_ciudad),
                new("nuevo_nombre", nuevaCiudad.Nombre),
                new("nuevo_id_region", nuevaCiudad.Region.Idregion),
                v_imagen
            };
            if (v_imagen.Value == DBNull.Value)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL p_actualizar_ciudad(@id_ciudad, @nuevo_nombre, @nuevo_id_region)", parameters);
                return "Ok";
            }

            await _context.Database.ExecuteSqlRawAsync(
                "CALL p_actualizar_ciudad(@id_ciudad, @nuevo_nombre, @nuevo_id_region, @_imagen)", parameters);
            return "Ok";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    ///     Establece la lista de Ciudades en el contexto de la base de datos.
    /// </summary>
    /// <returns>Lista de Ciudades.</returns>
    protected override async Task<List<Ciudad>> SetContextList()
    {
        var list = await _context.Set<Ciudad>()
            .Include(a => a.Region)
            .ThenInclude(c => c.Pais)
            .ToListAsync();
        return list;
    }

    /// <summary>
    ///     Establece una Ciudad específica en el contexto de la base de datos por su ID.
    /// </summary>
    /// <param name="id">ID de la Ciudad.</param>
    /// <returns>Ciudad encontrada en la base de datos.</returns>
    protected override async Task<Ciudad> SetContextEntity(int id)
    {
        var entity = await _context.Set<Ciudad>()
            .Include(a => a.Region)
            .ThenInclude(c => c.Pais)
            .FirstOrDefaultAsync(a => a.Idciudad == id);

        return entity;
    }
}