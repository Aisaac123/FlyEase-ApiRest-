using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;

namespace FlyEase_ApiRest_.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Npgsql;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace TuNamespace
    {
        [EnableCors("Reglas")]
        [Route("FlyEaseApi/[controller]")]
        [ApiController]

        /// <summary>
        /// Controlador para gestionar operaciones CRUD de Aerolineas.
        /// </summary>
        [SwaggerTag("Metodos Crud para Aerolineas")]

        public class AerolineasController : CrudController<Aerolinea, int, FlyEaseDataBaseContextAuthentication>
        {

            /// <summary>
            /// Constructor del controlador de Aerolineas.
            /// </summary>
            /// <param name="context">Contexto de base de datos.</param>
            /// <param name="hubContext">Contexto del hub.</param>

            public AerolineasController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
            {
                _context = context;
            }

            // Documentación de métodos heredados de la clase abstracta.

            /// <summary>
            /// Crea un nuevo elemento de Aerolinea.
            /// </summary>

            [HttpPost("Post")]
            [SwaggerOperation("Registrar una nueva Aerolinea.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> Post([FromBody] Aerolinea entity)
            {
                var func = await base.Post(entity);
                return func;
            }

            /// <summary>
            /// Actualiza una Aerolinea por ID.
            /// </summary>

            [HttpPut("Put/{Id}")]
            [SwaggerOperation("Actualizar un registro de Aerolinea existente.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> Put([FromBody] Aerolinea entity, int Id)
            {
                var func = await base.Put(entity, Id);
                return func;
            }

            /// <summary>
            /// Elimina una Aerolinea por ID.
            /// </summary>

            [HttpDelete("Delete/{Id}")]
            [SwaggerOperation("Eliminar un registro de Aerolinea por ID.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> Delete(int Id)
            {
                var func = await base.Delete(Id);
                return func;
            }

            /// <summary>
            /// Elimina todas las Aerolinea.
            /// </summary>

            [HttpDelete("DeleteAll")]
            [SwaggerOperation("Eliminar todos los registros de Aerolineas.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
            public override async Task<IActionResult> DeleteAll()
            {
                var func = await base.DeleteAll();
                return func;
            }

            /// <summary>
            /// Obtiene todos los elementos de las Aerolinea.
            /// </summary>

            [HttpGet("GetAll")]
            [SwaggerOperation("Obtener todos los registros de Aerolineas.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(List<Aerolinea>))]
            public override async Task<IActionResult> Get()
            {
                var func = await base.Get();
                return func;
            }

            /// <summary>
            /// Obtiene una Aerolinea por ID.
            /// </summary>
            /// <param name="id">ID de la Aerolinea a obtener.</param>
            /// <returns>La Aerolinea solicitada.</returns>

            [HttpGet("GetById/{id}")]
            [SwaggerOperation("Obtener un registro de Aerolinea por ID.")]
            [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(Aerolinea))]
            public override async Task<IActionResult> GetById(int id)
            {
                var func = await base.GetById(id);
                return func;
            }

            // Métodos protegidos

            /// <summary>
            /// Inserta una nueva aerolínea en la base de datos.
            /// </summary>
            /// <param name="entity">Aerolínea a insertar.</param>
            /// <returns>Un mensaje de confirmación o un mensaje de error.</returns>
            protected override async Task<string> InsertProcedure(Aerolinea entity)
            {
                try
                {
                    var parameters = new NpgsqlParameter[]
                    {
                    new NpgsqlParameter("nombre_aereolinea", entity.Nombre),
                    new NpgsqlParameter("v_codigo_iata", entity.Codigoiata),
                    new NpgsqlParameter("v_codigo_icao", entity.Codigoicao)
                    };

                    await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_aereolinea(@nombre_aereolinea, @v_codigo_iata, @v_codigo_icao)", parameters);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            /// <summary>
            /// Elimina una aerolínea de la base de datos por su ID.
            /// </summary>
            /// <param name="id_Aerolinea">ID de la aerolínea a eliminar.</param>
            /// <returns>Un mensaje de confirmación o un mensaje de error.</returns>
            protected override async Task<string> DeleteProcedure(int id_Aerolinea)
            {
                try
                {
                    var parameters = new NpgsqlParameter[]
                    {
                    new NpgsqlParameter("id_aereolinea", id_Aerolinea)
                    };

                    await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_aereolinea(@id_aereolinea)", parameters);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            /// <summary>
            /// Actualiza una aerolínea en la base de datos por su ID.
            /// </summary>
            /// <param name="nuevaAerolinea">Nueva información de la aerolínea a actualizar.</param>
            /// <param name="id_Aerolinea">ID de la aerolínea a actualizar.</param>
            /// <returns>Un mensaje de confirmación o un mensaje de error.</returns>
            protected override async Task<string> UpdateProcedure(Aerolinea nuevaAerolinea, int id_Aerolinea)
            {
                try
                {
                    var parameters = new NpgsqlParameter[]
                    {
                    new NpgsqlParameter("id_aereolinea", id_Aerolinea),
                    new NpgsqlParameter("nuevo_nombre", nuevaAerolinea.Nombre),
                    new NpgsqlParameter("nuevo_codigo_iata", nuevaAerolinea.Codigoiata),
                    new NpgsqlParameter("nuevo_codigo_icao", nuevaAerolinea.Codigoicao)
                    };

                    await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_aereolinea(@id_aereolinea, @nuevo_nombre, @nuevo_codigo_iata, @nuevo_codigo_icao)", parameters);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}