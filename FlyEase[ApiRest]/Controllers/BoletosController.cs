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
    /// Controlador para gestionar operaciones CRUD de Boletos.
    /// </summary>
    public class BoletosController : CrudController<Boleto, int, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de Boletos.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public BoletosController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }
        
        /// <summary>
        /// Crea un nuevo registro de Boleto.
        /// </summary>
        /// <param name="entity">Datos del Boleto a crear.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        
        [HttpPost("Post")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Post([FromBody] Boleto entity)
        {
            var func = await base.Post(entity);
            return func;

        }
        
        /// <summary>
        /// Actualiza un registro de Boleto en la base de datos por su ID.
        /// </summary>
        /// <param name="entity">Datos del Boleto a actualizar.</param>
        /// <param name="Id">ID del Boleto a actualizar.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        
        [HttpPut("Put/{Id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public override async Task<IActionResult> Put([FromBody] Boleto entity, int Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }
        
        /// <summary>
        /// Elimina un registro de Boleto de la base de datos por su ID.
        /// </summary>
        /// <param name="Id">ID del Boleto a eliminar.</param>
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
        /// Elimina todos los registros de Boletos de la base de datos.
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
        /// Obtiene la lista de Boletos desde la base de datos.
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
        /// Obtiene un registro de Boleto desde la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID del Boleto a obtener.</param>
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
        /// Inserta un nuevo registro de Boleto en la base de datos.
        /// </summary>
        /// <param name="entity">Datos del Boleto a insertar.</param>
        /// <returns>Resultado de la operación.</returns>
     
        protected override async Task<string> InsertProcedure(Boleto entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_descuento", entity.Descuento),
            new NpgsqlParameter("v_numero_documento", entity.Cliente.Numerodocumento),
            new NpgsqlParameter("v_idasiento", entity.Asiento.Idasiento),
            new NpgsqlParameter("v_idvuelo", entity.Vuelo.Idvuelo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_boleto(@v_descuento, @v_numero_documento, @v_idasiento, @v_idvuelo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Elimina un registro de Boleto de la base de datos por su ID.
        /// </summary>
        /// <param name="id_boleto">ID del Boleto a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>

        protected override async Task<string> DeleteProcedure(int id_boleto)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_boleto", id_boleto)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_boleto(@id_boleto)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
     
        /// <summary>
        /// Actualiza un registro de Boleto en la base de datos por su ID.
        /// </summary>
        /// <param name="nuevoBoleto">Nueva información del Boleto a actualizar.</param>
        /// <param name="id_boleto">ID del Boleto a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
      
        protected override async Task<string> UpdateProcedure(Boleto nuevoBoleto, int id_boleto)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_boleto", id_boleto),
            new NpgsqlParameter("nuevo_precio", nuevoBoleto.Precio),
            new NpgsqlParameter("nuevo_descuento", nuevoBoleto.Descuento),
            new NpgsqlParameter("nuevo_precio_total", nuevoBoleto.Preciototal),
            new NpgsqlParameter("nuevo_numero_documento_cliente", nuevoBoleto.Cliente.Numerodocumento),
            new NpgsqlParameter("nuevo_id_asiento", nuevoBoleto.Asiento.Idasiento),
            new NpgsqlParameter("nuevo_id_vuelo", nuevoBoleto.Vuelo.Idvuelo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_boleto(@id_boleto, @nuevo_precio, @nuevo_descuento, @nuevo_precio_total, @nuevo_numero_documento_cliente, @nuevo_id_asiento, @nuevo_id_vuelo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
      
        /// <summary>
        /// Obtiene la lista de Boletos desde la base de datos.
        /// </summary>
        /// <returns>Lista de Boletos.</returns>
      
        protected override async Task<List<Boleto>> SetContextList()
        {
            var list = await _context.Set<Boleto>()
                .Include(arg => arg.Cliente)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Categoria)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Estado)
                .ToListAsync();

            // Eliminar repeticiones:
            //list = list.GroupBy(boleto => boleto.Asiento.Idasiento).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Idvuelo).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Despegue.Idaereopuerto).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Destino.Idaereopuerto).Select(group => group.First()).ToList();

            return list;
        }
    
        /// <summary>
        /// Obtiene un registro de Boleto desde la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID del Boleto a obtener.</param>
        /// <returns>Boleto obtenido.</returns>
     
        protected override async Task<Boleto> SetContextEntity(int id)
        {
            var entity = await _context.Set<Boleto>()
                .Include(arg => arg.Cliente)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Categoria)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Estado)
         .FirstOrDefaultAsync(a => a.Idboleto == id);

            return entity;
        }
    }
}