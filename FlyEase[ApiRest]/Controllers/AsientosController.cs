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

namespace FlyEase_ApiRest_.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones CRUD de Asientos.
    /// </summary>
    [SwaggerTag("Metodos Crud para Asientos")]

    public class AsientosController : CrudController<Asiento, int, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de Asientos.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public AsientosController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }
        
        /// <summary>
        /// Crea un nuevo registro de Asiento.
        /// </summary>
        /// <param name="entity">Datos del Asiento a crear.</param>
        /// <returns>Respuesta de la solicitud.</returns>
     
        [HttpPost("Post")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Registrar un nuevo Asiento.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> Post([FromBody] Asiento entity)
        {
            var func = await base.Post(entity);
            return func;

        }
     
        /// <summary>
        /// Actualiza un registro de Asiento existente.
        /// </summary>
        /// <param name="entity">Datos del Asiento a actualizar.</param>
        /// <param name="Id">ID del Asiento a actualizar.</param>
        /// <returns>Respuesta de la solicitud.</returns>
     
        [HttpPut("Put/{Id}")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Actualizar un registro de Asiento existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> Put([FromBody] Asiento entity, int Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }
     
        /// <summary>
        /// Elimina un registro de Asiento por ID.
        /// </summary>
        /// <param name="Id">ID del Asiento a eliminar.</param>
        /// <returns>Respuesta de la solicitud.</returns>
     
        [HttpDelete("Delete/{Id}")]
        [SwaggerOperation("Eliminar un registro de Asiento por ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> Delete(int Id)
        {
            var func = await base.Delete(Id);
            return func;

        }
     
        /// <summary>
        /// Elimina todos los registros de Asientos.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>
     
        [HttpDelete("DeleteAll")]
        [SwaggerOperation("Eliminar todos los registros de Asientos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> DeleteAll()
        {
            var func = await base.DeleteAll();
            return func;

        }
    
        /// <summary>
        /// Obtiene todos los registros de Asientos.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>
    
        [HttpGet("GetAll")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Obtener todos los registros de Asientos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(List<Asiento>))]
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }
     
        /// <summary>
        /// Obtiene un registro de Asiento por ID.
        /// </summary>
        /// <param name="id">ID del Asiento a obtener.</param>
        /// <returns>Respuesta de la solicitud.</returns>
     
        [HttpGet("GetById/{id}")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Obtener un registro de Asiento por ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(Asiento))]
        public override async Task<IActionResult> GetById(int id)
        {
            var func = await base.GetById(id);
            return func;
        }
     
        /// <summary>
        /// Inserta un nuevo registro de Asiento en la base de datos.
        /// </summary>
        /// <param name="entity">Datos del Asiento a insertar.</param>
        /// <returns>Resultado de la operación.</returns>
     
        protected override async Task<string> InsertProcedure(Asiento entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_posicion", entity.Posicion),
            new NpgsqlParameter("v_disponibilidad", entity.Disponibilidad),
            new NpgsqlParameter("v_idcategoria", entity.Categoria.Idcategoria),
            new NpgsqlParameter("v_idavion", entity.Avion.Idavion)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_asiento(@v_posicion, @v_disponibilidad, @v_idcategoria, @v_idavion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
     
        /// <summary>
        /// Elimina un registro de Asiento de la base de datos por su ID.
        /// </summary>
        /// <param name="id_asiento">ID del Asiento a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
    
        protected override async Task<string> DeleteProcedure(int id_asiento)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_asiento", id_asiento)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_asiento(@id_asiento)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
     
        /// <summary>
        /// Actualiza un registro de Asiento en la base de datos por su ID.
        /// </summary>
        /// <param name="nuevoAsiento">Nueva información del Asiento a actualizar.</param>
        /// <param name="id_asiento">ID del Asiento a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
     
        protected override async Task<string> UpdateProcedure(Asiento nuevoAsiento, int id_asiento)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_asiento", id_asiento),
            new NpgsqlParameter("nueva_posicion", nuevoAsiento.Posicion),
            new NpgsqlParameter("nueva_disponibilidad", nuevoAsiento.Disponibilidad),
            new NpgsqlParameter("nuevo_id_categoria", nuevoAsiento.Categoria.Idcategoria),
            new NpgsqlParameter("nuevo_id_avion", nuevoAsiento.Avion.Idavion)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_asiento(@id_asiento, @nueva_posicion, @nueva_disponibilidad, @nuevo_id_categoria, @nuevo_id_avion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
     
        /// <summary>
        /// Obtiene la lista de Asientos desde la base de datos.
        /// </summary>
        /// <returns>Lista de Asientos.</returns>
     
        protected override async Task<List<Asiento>> SetContextList()
        {
            var list = await _context.Set<Asiento>()
          .Include(a => a.Avion)
          .ThenInclude(a => a.Aereolinea)
          .Include(a => a.Categoria)
         .ToListAsync();
            return list;
        }
     
        /// <summary>
        /// Obtiene un registro de Asiento desde la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID del Asiento a obtener.</param>
        /// <returns>Asiento obtenido.</returns>
     
        protected override async Task<Asiento> SetContextEntity(int id)
        {
            var entity = await _context.Set<Asiento>()
          .Include(a => a.Avion)
          .ThenInclude(a => a.Aereolinea)
          .Include(a => a.Categoria)
         .FirstOrDefaultAsync(a => a.Idasiento == id);

            return entity;
        }
    }
}