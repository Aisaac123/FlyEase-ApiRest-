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
    /// Controlador para gestionar operaciones CRUD de Aviones.
    /// </summary>
    public class AvionesController : CrudController<Avion, string, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de Aviones.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public AvionesController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }
        
        /// <summary>
        /// Crea un nuevo registro de Avión.
        /// </summary>
        /// <param name="entity">Datos del Avión a crear.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        
        [HttpPost("Post")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Post([FromBody] Avion entity)
        {
            var func = await base.Post(entity);
            return func;

        }
       
        /// <summary>
        /// Actualiza un registro de Avión en la base de datos por su ID.
        /// </summary>
        /// <param name="entity">Datos del Avión a actualizar.</param>
        /// <param name="Id">ID del Avión a actualizar.</param>
        /// <returns>Respuesta de la solicitud.</returns>
       
        [HttpPut("Put/{Id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Put([FromBody] Avion entity, string Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }
      
        /// <summary>
        /// Elimina un registro de Avión de la base de datos por su ID.
        /// </summary>
        /// <param name="Id">ID del Avión a eliminar.</param>
        /// <returns>Respuesta de la solicitud.</returns>
      
        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Delete(string Id)
        {
            var func = await base.Delete(Id);
            return func;

        }
      
        /// <summary>
        /// Elimina todos los registros de Aviones de la base de datos.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>
       
        [HttpDelete("DeleteAll")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> DeleteAll()
        {
            var func = await base.DeleteAll();
            return func;

        }

        /// <summary>
        /// Obtiene la lista de Aviones desde la base de datos.
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
        /// Obtiene un registro de Avión desde la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID del Avión a obtener.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Aeropuerto), StatusCodes.Status200OK)]
        public override async Task<IActionResult> GetById(string id)
        {
            var func = await base.GetById(id);
            return func;
        }
        
        /// <summary>
        /// Inserta un nuevo registro de Avión en la base de datos.
        /// </summary>
        /// <param name="entity">Datos del Avión a insertar.</param>
        /// <returns>Resultado de la operación.</returns>
       
        protected override async Task<string> InsertProcedure(Avion entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_idavion", entity.Idavion),
            new NpgsqlParameter("nombre_avion", entity.Nombre),
            new NpgsqlParameter("modelo_avion", entity.Modelo),
            new NpgsqlParameter("fabricante_avion", entity.Fabricante),
            new NpgsqlParameter("v_velocidadpromedio", entity.Velocidadpromedio),
            new NpgsqlParameter("v_cantidadpasajeros", entity.Cantidadpasajeros),
            new NpgsqlParameter("v_cantidadcarga", entity.Cantidadcarga),
            new NpgsqlParameter("id_aereolinea", entity.Aereolinea.Idaereolinea)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_avion(@v_idavion, @nombre_avion, @modelo_avion, @fabricante_avion, @v_velocidadpromedio, @v_cantidadpasajeros, @v_cantidadcarga, @id_aereolinea)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        /// <summary>
        /// Elimina un registro de Avión de la base de datos por su ID.
        /// </summary>
        /// <param name="id_avion">ID del Avión a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        
        protected override async Task<string> DeleteProcedure(string id_avion)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_avion", id_avion)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_avion(@id_avion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
       
        /// <summary>
        /// Actualiza un registro de Avión en la base de datos por su ID.
        /// </summary>
        /// <param name="nuevoAvion">Nueva información del Avión a actualizar.</param>
        /// <param name="id_avion">ID del Avión a actualizar.</param>
       
        protected override async Task<string> UpdateProcedure(Avion nuevoAvion, string id_avion)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_avion", id_avion),
            new NpgsqlParameter("new_idavion", nuevoAvion.Idavion),
            new NpgsqlParameter("nuevo_nombre", nuevoAvion.Nombre),
            new NpgsqlParameter("nuevo_modelo", nuevoAvion.Modelo),
            new NpgsqlParameter("nuevo_fabricante", nuevoAvion.Fabricante),
            new NpgsqlParameter("nueva_velocidad_promedio", nuevoAvion.Velocidadpromedio),
            new NpgsqlParameter("nueva_cantidad_pasajeros", nuevoAvion.Cantidadpasajeros),
            new NpgsqlParameter("nueva_cantidad_carga", nuevoAvion.Cantidadcarga),
            new NpgsqlParameter("nuevo_id_aereolinea", nuevoAvion.Aereolinea.Idaereolinea)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_avion(@id_avion, @new_idavion, @nuevo_nombre, @nuevo_modelo, @nuevo_fabricante, @nueva_velocidad_promedio, @nueva_cantidad_pasajeros, @nueva_cantidad_carga, @nuevo_id_aereolinea)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
       
        /// <summary>
        /// Obtiene la lista de Aviones desde la base de datos.
        /// </summary>
        /// <returns>Lista de Aviones.</returns>
       
        protected override async Task<List<Avion>> SetContextList()
        {
            var list = await _context.Set<Avion>()
          .Include(a => a.Aereolinea)
          .ToListAsync();
            return list;
        }
       
        /// <summary>
        /// Obtiene un registro de Avión desde la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID del Avión a obtener.</param>
        /// <returns>Avión obtenido.</returns>
       
        protected override async Task<Avion> SetContextEntity(string id)
        {
            var entity = await _context.Set<Avion>()
         .Include(a => a.Aereolinea)
         .FirstOrDefaultAsync(a => a.Idavion == id);

            return entity;
        }
       
        /// <summary>
        /// Obtiene la lista de Asientos asociados a un Avión específico.
        /// </summary>
        /// <param name="id_avion">ID del Avión para obtener sus Asientos.</param>
        /// <returns>Respuesta de la solicitud.</returns>
       
        [HttpGet]
        [Route("GetAsientos/{id_avion}")]
        public async Task<IActionResult> GetAsiento(string id_avion)
        {
            try
            {
                var entity = await _context.Set<Avion>().Include(a => a.Asientos).ThenInclude(a => a.Categoria).Include(a => a.Aereolinea).FirstOrDefaultAsync(a => a.Idavion == id_avion);

                if (entity != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, response = entity.Asientos });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Avión no encontrado", Succes = false, response = new List<Asiento>() });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = new List<Asiento>() });
            }
        }
    }
}