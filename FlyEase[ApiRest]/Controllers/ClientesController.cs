using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.SignalR;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NuGet.Common;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]

    /// <summary>
    /// Controlador para gestionar operaciones CRUD de Clientes.
    /// </summary>
    
    [SwaggerTag("Metodos Crud para Clientes")]

    public class ClientesController : CrudController<Cliente, string, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de Clientes.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public ClientesController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        /// <summary>
        /// Crea un nuevo registro de Cliente.
        /// </summary>
        /// <param name="entity">Datos del Cliente a crear.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        /// 

        [HttpPost("Post")]
        [Authorize]
        [SwaggerOperation("Registrar un cliente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
        public override async Task<IActionResult> Post([FromBody] Cliente entity)
        {
            var func = await base.Post(entity);
            return func;

        }

        /// <summary>
        /// Actualiza un registro de Cliente en la base de datos por su ID.
        /// </summary>
        /// <param name="entity">Datos del Cliente a actualizar.</param>
        /// <param name="Id">ID del Cliente a actualizar.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpPut("Put/{Id}")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Actualizar un cliente especifico por medio de su ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
        public override async Task<IActionResult> Put([FromBody] Cliente entity, string Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }

        /// <summary>
        /// Elimina un registro de Cliente de la base de datos por su ID.
        /// </summary>
        /// <param name="Id">ID del Cliente a eliminar.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpDelete("Delete/{Id}")]
        [SwaggerOperation("Eliminar un cliente especifico por medio de su ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
        public override async Task<IActionResult> Delete(string Id)
        {
            var func = await base.Delete(Id);
            return func;

        }

        /// <summary>
        /// Elimina todos los registros de Clientes de la base de datos.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpDelete("DeleteAll")]
        [SwaggerOperation("Eliminar todos los clientes registrados (Usar Con Precaucion).")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(string))]
        public override async Task<IActionResult> DeleteAll()
        {
            var func = await base.DeleteAll();
            return func;

        }

        /// <summary>
        /// Obtiene la lista de Clientes desde la base de datos.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpGet("GetAll")]
        [Authorize(Policy = "Admin Policy")]
        [SwaggerOperation("Obtener todos los clientes registrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(List<Cliente>))]
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }

        /// <summary>
        /// Obtiene un registro de Cliente desde la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID del Cliente a obtener.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpGet("GetById/{id}")]
        [Authorize]
        [SwaggerOperation("Obtener un cliente por medio de su ID (No.Documento).")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(Cliente))]
        public override async Task<IActionResult> GetById(string id)
        {
            var func = await base.GetById(id);
            return func;
        }
       
        /// <summary>
        /// Inserta un nuevo registro de Cliente en la base de datos.
        /// </summary>
        /// <param name="entity">Datos del Cliente a insertar.</param>
        /// <returns>Resultado de la operación.</returns>

        protected override async Task<string> InsertProcedure(Cliente entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_numerodocumento", entity.Numerodocumento),
            new NpgsqlParameter("v_tipodocumento", entity.Tipodocumento),
            new NpgsqlParameter("v_nombres", entity.Nombres),
            new NpgsqlParameter("v_apellidos", entity.Apellidos),
            new NpgsqlParameter("v_celular", entity.Celular),
            new NpgsqlParameter("v_correo", entity.Correo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_cliente(@v_numerodocumento, @v_tipodocumento, @v_nombres, @v_apellidos, @v_celular, @v_correo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Actualiza un registro de Cliente en la base de datos por su ID.
        /// </summary>
        /// <param name="entity">Nueva información del Cliente a actualizar.</param>
        /// <param name="OldId">ID del Cliente a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>

        protected override async Task<string> UpdateProcedure(Cliente entity, string OldId)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("numero_documento_cliente", OldId),
            new NpgsqlParameter("nuevo_documento", entity.Numerodocumento),
            new NpgsqlParameter("nuevo_tipo_documento", entity.Tipodocumento),
            new NpgsqlParameter("nuevo_nombres", entity.Nombres),
            new NpgsqlParameter("nuevo_apellidos", entity.Apellidos),
            new NpgsqlParameter("nuevo_celular", entity.Celular),
            new NpgsqlParameter("nuevo_correo", entity.Correo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_cliente(@numero_documento_cliente, @nuevo_documento, @nuevo_tipo_documento, @nuevo_nombres, @nuevo_apellidos, @nuevo_celular, @nuevo_correo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Elimina un registro de Cliente de la base de datos por su ID.
        /// </summary>
        /// <param name="Old_Id">ID del Cliente a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>

        protected override async Task<string> DeleteProcedure(string Old_Id)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("numero_documento_cliente", Old_Id),
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_cliente(@numero_documento_cliente) ", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Obtiene los boletos asociados a un Cliente específico.
        /// </summary>
        /// <param name="id_cliente">ID del Cliente.</param>
        /// <returns>Respuesta de la solicitud.</returns>

        [HttpGet]
        [Route("GetById/{id_cliente}/Boletos")]
        [Authorize]
        [SwaggerOperation("Obtener todos los boletos de un cliente en especifico.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operacion realizada con exito", typeof(List<Boleto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Cliente no encontrado o datos erroneos", typeof(string))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Sin autorizacion, por favor solicite el token", typeof(string))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor", typeof(string))]

        public async Task<IActionResult> GetAsiento(string id_cliente)
        {
            try
            {
                var entity = await _context.Set<Cliente>()
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Categoria)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aeropuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aeropuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aeropuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aeropuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(a => a.Boletos)
                .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Estado)
                    .FirstOrDefaultAsync(a => a.Numerodocumento == id_cliente);

                if (entity != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, response = entity.Boletos });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Cliente no encontrado", Succes = false, response = new List<Asiento>() });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = new List<Asiento>() });
            }
        }
    }
}