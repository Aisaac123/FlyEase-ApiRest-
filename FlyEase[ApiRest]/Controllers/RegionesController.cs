using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]

    /// <summary>
    /// Controlador para operaciones CRUD relacionadas con países.
    /// </summary>

    public class RegionesController : CrudController<Region, int, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de regiones.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public RegionesController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        /// <summary>
        /// Crea una nueva región.
        /// </summary>
        /// <param name="entity">Información de la región a crear.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpPost("Post")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Post([FromBody] Region entity)
        {
            var func = await base.Post(entity);
            return func;

        }
       
        /// <summary>
        /// Actualiza la información de una región existente.
        /// </summary>
        /// <param name="entity">Información actualizada de la región.</param>
        /// <param name="Id">ID de la región a actualizar.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpPut("Put/{Id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Put([FromBody] Region entity, int Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }
       
        /// <summary>
        /// Elimina una región por su ID.
        /// </summary>
        /// <param name="Id">ID de la región a eliminar.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Delete(int Id)
        {
            var func = await base.Delete(Id);
            return func;

        }

        /// <summary>
        /// Elimina todas las regiones.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpDelete("DeleteAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> DeleteAll()
        {
            var func = await base.DeleteAll();
            return func;

        }

        /// <summary>
        /// Obtiene todas las regiones.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(List<Aeropuerto>), StatusCodes.Status200OK)]
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }

        /// <summary>
        /// Obtiene una región por su ID.
        /// </summary>
        /// <param name="id">ID de la región a obtener.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Aeropuerto), StatusCodes.Status200OK)]
        public override async Task<IActionResult> GetById(int id)
        {
            var func = await base.GetById(id);
            return func;
        }

        /// <summary>
        /// Método interno para insertar una región usando un procedimiento almacenado.
        /// </summary>
        /// <param name="entity">Información de la región a insertar.</param>
        /// <returns>Respuesta de la operación.</returns>

        protected override async Task<string> InsertProcedure(Region entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_region", entity.Nombre),
                    new NpgsqlParameter("nombre_pais", entity.Pais.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_region(@nombre_region, @nombre_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Método interno para eliminar una región usando un procedimiento almacenado.
        /// </summary>
        /// <param name="id_region">ID de la región a eliminar.</param>
        /// <returns>Respuesta de la operación.</returns>

        protected override async Task<string> DeleteProcedure(int id_region)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("id_region", id_region)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_region(@id_region)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Método interno para actualizar una región usando un procedimiento almacenado.
        /// </summary>
        /// <param name="nuevaRegion">Información actualizada de la región.</param>
        /// <param name="id_region">ID de la región a actualizar.</param>
        /// <returns>Respuesta de la operación.</returns>

        protected override async Task<string> UpdateProcedure(Region nuevaRegion, int id_region)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_region", id_region),
            new NpgsqlParameter("nuevo_nombre", nuevaRegion.Nombre),
                    new NpgsqlParameter("nuevo_id_pais", nuevaRegion.Pais.Idpais)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_region(@id_region, @nuevo_nombre, @nuevo_id_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Establece la lista de regiones dentro del contexto.
        /// </summary>
        /// <returns>Lista de regiones.</returns>

        protected override async Task<List<Region>> SetContextList()
        {
            var list = await _context.Set<Region>()
          .Include(a => a.Pais)
          .ToListAsync();
            return list;
        }

        /// <summary>
        /// Establece una región específica dentro del contexto por su ID.
        /// </summary>
        /// <param name="id">ID de la región.</param>
        /// <returns>La región obtenida del contexto.</returns>

        protected override async Task<Region> SetContextEntity(int id)
        {
            var entity = await _context.Set<Region>()
         .Include(a => a.Pais)
         .FirstOrDefaultAsync(a => a.Idregion == id);

            return entity;
        }
    }
}