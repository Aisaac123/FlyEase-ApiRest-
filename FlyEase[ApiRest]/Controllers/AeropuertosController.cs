using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace FlyEase_ApiRest_.Controllers
{
    using FlyEase_ApiRest_.Models.Contexto;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using NpgsqlTypes;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    namespace TuNamespaceDeControladores
    {
        [EnableCors("Reglas")]
        [Route("FlyEaseApi/[controller]")]
        [ApiController]

        /// <summary>
        /// Controlador para gestionar operaciones CRUD de Aeropuertos.
        /// </summary>
        [SwaggerTag("Metodos Crud para Aeropuertos")]

        public class AeropuertosController : CrudController<Aeropuerto, int, FlyEaseDataBaseContextAuthentication>
        {

            /// <summary>
            /// Constructor del controlador de Aeropuerto.
            /// </summary>
            /// <param name="context">Contexto de base de datos.</param>
            /// <param name="hubContext">Contexto del hub.</param>

            public AeropuertosController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
            {
                _context = context;
            }

            /// <summary>
            /// Crea un nuevo registro de Aeropuerto.
            /// </summary>
            /// <param name="entity">Datos del Aeropuerto a crear.</param>
            /// <returns>Respuesta de la solicitud.</returns>

            [HttpPost("Post")]
            [Authorize(Policy = "Admin Policy")]
            [SwaggerOperation("Registrar un nuevo Aeropuerto.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> Post([FromBody] Aeropuerto entity)
            {
                var func = await base.Post(entity);
                return func;

            }

            /// <summary>
            /// Actualiza un registro de Aeropuerto existente.
            /// </summary>
            /// <param name="entity">Datos del Aeropuerto a actualizar.</param>
            /// <param name="Id">ID del Aeropuerto a actualizar.</param>
            /// <returns>Respuesta de la solicitud.</returns>

            [HttpPut("Put/{Id}")]
            [Authorize(Policy = "Admin Policy")]
            [SwaggerOperation("Actualizar un registro de Aeropuerto existente.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> Put([FromBody] Aeropuerto entity, int Id)
            {
                var func = await base.Put(entity, Id);
                return func;

            }

            /// <summary>
            /// Elimina un registro de Aeropuerto por ID.
            /// </summary>
            /// <param name="Id">ID del Aeropuerto a eliminar.</param>
            /// <returns>Respuesta de la solicitud.</returns>

            [HttpDelete("Delete/{Id}")]
            [SwaggerOperation("Eliminar un registro de Aeropuerto por ID.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> Delete(int Id)
            {
                var func = await base.Delete(Id);
                return func;

            }

            /// <summary>
            /// Elimina todos los registros de Aeropuerto.
            /// </summary>
            /// <returns>Respuesta de la solicitud.</returns>

            [HttpDelete("DeleteAll")]
            [SwaggerOperation("Eliminar todos los registros de Aeropuertos.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> DeleteAll()
            {
                var func = await base.DeleteAll();
                return func;

            }

            /// <summary>
            /// Obtiene todos los registros de Aeropuerto.
            /// </summary>
            /// <returns>Respuesta de la solicitud.</returns>

            [HttpGet("GetAll")]
            [Authorize(Policy = "Admin Policy")]
            [SwaggerOperation("Obtener todos los registros de Aeropuertos.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(List<Aeropuerto>))]
            public override async Task<IActionResult> Get()
            {
                var func = await base.Get();
                return func;
            }

            /// <summary>
            /// Obtiene un registro de Aeropuerto por ID.
            /// </summary>
            /// <param name="id">ID del Aeropuerto a obtener.</param>
            /// <returns>Respuesta de la solicitud.</returns>

            [HttpGet("GetById/{id}")]
            [Authorize(Policy = "Admin Policy")]
            [SwaggerOperation("Obtener un registro de Aeropuerto por ID.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(Aeropuerto))]
            public override async Task<IActionResult> GetById(int id)
            {
                var func = await base.GetById(id);
                return func;
            }

            /// <summary>
            /// Inserta un nuevo registro de Aeropuerto en la base de datos.
            /// </summary>
            /// <param name="entity">Datos del Aeropuerto a insertar.</param>
            /// <returns>Resultado de la operación.</returns>

            protected override async Task<string> InsertProcedure(Aeropuerto entity)
            {
                try
                {
                    NpgsqlParameter v_imagen;

                    if (entity.Ciudad.Imagen != null)
                    {
                        v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                        {
                            Value = entity.Ciudad.Imagen
                        };
                    }
                    else
                    {
                        v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                        {
                            Value = DBNull.Value // Valor nulo
                        };
                    }
                    var parameters = new NpgsqlParameter[]
                    {
                    new NpgsqlParameter("nombre_pais", entity.Ciudad.Region.Pais.Nombre),
                    new NpgsqlParameter("nombre_region",  entity.Ciudad.Region.Nombre),
                    new NpgsqlParameter("nombre_ciudad",  entity.Ciudad.Nombre),
                    new NpgsqlParameter("v_latitud", entity.Coordenadas.Latitud),
                    new NpgsqlParameter("v_longitud",  entity.Coordenadas.Longitud),
                    new NpgsqlParameter("nombre_aereopuerto", entity.Nombre),
                    v_imagen
                    };

                    await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_aereopuerto(@nombre_pais, @nombre_region, @nombre_ciudad, @v_latitud, @v_longitud, @nombre_aereopuerto, @_imagen)", parameters);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            /// <summary>
            /// Elimina un registro de Aeropuerto de la base de datos por su ID.
            /// </summary>
            /// <param name="id_Aeropuerto">ID del Aeropuerto a eliminar.</param>
            /// <returns>Resultado de la operación.</returns>

            protected override async Task<string> DeleteProcedure(int id_Aeropuerto)
            {
                try
                {
                    var parameters = new NpgsqlParameter[]
                    {
                    new NpgsqlParameter("id_aereopuerto", id_Aeropuerto)
                    };

                    await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_aereopuerto(@id_aereopuerto)", parameters);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            /// <summary>
            /// Actualiza un registro de Aeropuerto en la base de datos por su ID.
            /// </summary>
            /// <param name="nuevoAeropuerto">Nueva información del Aeropuerto a actualizar.</param>
            /// <param name="id_Aeropuerto">ID del Aeropuerto a actualizar.</param>
            /// <returns>Resultado de la operación.</returns>

            protected override async Task<string> UpdateProcedure(Aeropuerto nuevoAeropuerto, int id_Aeropuerto)
            {
                try
                {
                    NpgsqlParameter v_imagen;

                    if (nuevoAeropuerto.Ciudad.Imagen != null)
                    {
                        v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                        {
                            Value = nuevoAeropuerto.Ciudad.Imagen
                        };
                    }
                    else
                    {
                        v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                        {
                            Value = DBNull.Value // Valor nulo
                        };
                    }
                    var parameters = new NpgsqlParameter[]
                    {
                    new NpgsqlParameter("id_aereopuerto", id_Aeropuerto),
                    new NpgsqlParameter("nuevo_nombre", nuevoAeropuerto.Nombre),
                    new NpgsqlParameter("nueva_ciudad", nuevoAeropuerto.Ciudad.Nombre),
                    new NpgsqlParameter("nueva_region", nuevoAeropuerto.Ciudad.Region.Nombre),
                    new NpgsqlParameter("nuevo_pais", nuevoAeropuerto.Ciudad.Region.Pais.Nombre),
                    new NpgsqlParameter("nueva_latitud", nuevoAeropuerto.Coordenadas.Latitud),
                    new NpgsqlParameter("nueva_longitud", nuevoAeropuerto.Coordenadas.Longitud),
                    v_imagen
                    };

                    await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_aereopuerto(@id_aereopuerto, @nuevo_nombre, @nueva_ciudad, @nueva_region, @nuevo_pais, @nueva_latitud, @nueva_longitud, @_imagen)", parameters);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            /// <summary>
            /// Obtiene la lista de Aeropuerto desde la base de datos.
            /// </summary>
            /// <returns>Lista de Aeropuerto.</returns>

            protected override async Task<List<Aeropuerto>> SetContextList()
            {
                var list = await _context.Set<Aeropuerto>()
                  .Include(a => a.Ciudad)
                      .ThenInclude(c => c.Region)
                          .ThenInclude(r => r.Pais)
                  .Include(a => a.Coordenadas)
                  .ToListAsync();
                return list;
            }

            /// <summary>
            /// Obtiene un registro de Aeropuerto desde la base de datos por su ID.
            /// </summary>
            /// <param name="id">ID del Aeropuerto a obtener.</param>
            /// <returns>Aeropuerto obtenido.</returns>

            protected override async Task<Aeropuerto> SetContextEntity(int id)
            {
                var entity = await _context.Set<Aeropuerto>()
                 .Include(a => a.Ciudad)
                     .ThenInclude(c => c.Region)
                         .ThenInclude(r => r.Pais)
                 .Include(a => a.Coordenadas)
                 .FirstOrDefaultAsync(a => a.Idaereopuerto == id);

                return entity;
            }
        }
    }
}