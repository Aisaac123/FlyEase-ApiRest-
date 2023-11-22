using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Contexto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]

    /// <summary>
    /// Controlador para operaciones CRUD relacionadas con países.
    /// </summary>
    [SwaggerTag("Metodos Crud para Paises")]

    public class PaisesController : CrudController<Pais, int, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de Paises.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>
       
        public PaisesController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        /// <summary>
        /// Crea un nuevo país.
        /// </summary>
        /// <param name="entity">Información del país a crear.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpPost("Post")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Registrar un nuevo país")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se ha creado el país con éxito", typeof(string))]
        public override async Task<IActionResult> Post([FromBody] Pais entity)
        {
            var func = await base.Post(entity);
            return func;

        }

        /// <summary>
        /// Actualiza la información de un país existente.
        /// </summary>
        /// <param name="entity">Información actualizada del país.</param>
        /// <param name="Id">ID del país a actualizar.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpPut("Put/{Id}")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Actualizar información de un país existente")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se ha actualizado el país con éxito", typeof(string))]
        public override async Task<IActionResult> Put([FromBody] Pais entity, int Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }


        /// <summary>
        /// Elimina un país por su ID.
        /// </summary>
        /// <param name="Id">ID del país a eliminar.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpDelete("Delete/{Id}")]
        [SwaggerOperation("Eliminar un país por su ID")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se ha eliminado el país con éxito", typeof(string))]
        public override async Task<IActionResult> Delete(int Id)
        {
            var func = await base.Delete(Id);
            return func;

        }

        /// <summary>
        /// Elimina todos los países.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpDelete("DeleteAll")]
        [SwaggerOperation("Eliminar todos los países")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se han eliminado todos los países con éxito", typeof(string))]
        public override async Task<IActionResult> DeleteAll()
        {
            var func = await base.DeleteAll();
            return func;

        }

        /// <summary>
        /// Obtiene todos los países.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpGet("GetAll")]
        [Authorize]
        [SwaggerOperation("Obtener todos los países")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se han obtenido todos los países con éxito", typeof(List<Pais>))]
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }

        /// <summary>
        /// Obtiene un país por su ID.
        /// </summary>
        /// <param name="id">ID del país a obtener.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpGet("GetById/{id}")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Obtener un país por su ID")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se ha obtenido el país con éxito", typeof(Pais))]
        public override async Task<IActionResult> GetById(int id)
        {
            var func = await base.GetById(id);
            return func;
        }

        /// <summary>
        /// Método interno para insertar un país usando un procedimiento almacenado.
        /// </summary>
        /// <param name="entity">Información del país a insertar.</param>
        /// <returns>Respuesta de la operación.</returns>

        protected override async Task<string> InsertProcedure(Pais entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_pais", entity.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_pais(@nombre_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Método interno para eliminar un país usando un procedimiento almacenado.
        /// </summary>
        /// <param name="id_pais">ID del país a eliminar.</param>
        /// <returns>Respuesta de la operación.</returns>

        protected override async Task<string> DeleteProcedure(int id_pais)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_pais", id_pais)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_pais(@id_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Método interno para actualizar un país usando un procedimiento almacenado.
        /// </summary>
        /// <param name="nuevoPais">Información actualizada del país.</param>
        /// <param name="id_pais">ID del país a actualizar.</param>
        /// <returns>Respuesta de la operación.</returns>

        protected override async Task<string> UpdateProcedure(Pais nuevoPais, int id_pais)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_pais", id_pais),
            new NpgsqlParameter("nuevo_nombre", nuevoPais.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_pais(@id_pais, @nuevo_nombre)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}