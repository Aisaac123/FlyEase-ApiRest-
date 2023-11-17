using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using GeoCoordinatePortable;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]

    /// <summary>
    /// Controlador para operaciones CRUD relacionadas con vuelos.
    /// </summary>

    [SwaggerTag("Metodos Crud para Vuelos")]

    public class VuelosController : CrudController<Vuelo, int, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de vuelos.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public VuelosController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        /// <summary>
        /// Método HTTP POST para agregar un vuelo.
        /// </summary>
        /// <param name="entity">Entidad de tipo Vuelo a agregar.</param>
        /// <returns>Resultado de la operación.</returns>

        [HttpPost("Post")]
        [SwaggerOperation("Registra un vuelo")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se ha creado y registrado con exito", typeof(string))]
        public override async Task<IActionResult> Post([FromBody] Vuelo entity)
        {
            var func = await base.Post(entity);
            return func;

        }

        /// <summary>
        /// Método HTTP PUT para actualizar un vuelo por su ID.
        /// </summary>
        /// <param name="entity">Entidad de tipo Vuelo con la información actualizada.</param>
        /// <param name="Id">ID del vuelo a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>

        [HttpPut("Put/{Id}")]
        [SwaggerOperation("Actualizar los datos de un vuelo")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se ha actualizado con éxito", typeof(string))]
        public override async Task<IActionResult> Put([FromBody] Vuelo entity, int Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }

        /// <summary>
        /// Método HTTP DELETE para eliminar un vuelo por su ID.
        /// </summary>
        /// <param name="Id">ID del vuelo a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>

        [HttpDelete("Delete/{Id}")]
        [SwaggerOperation("Eliminar un vuelo específico", OperationId = "EliminarVuelo")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se ha eliminado con exito", typeof(string))]

        public override async Task<IActionResult> Delete(int Id)
        {
            var func = await base.Delete(Id);
            return func;

        }

        /// <summary>
        /// Método HTTP DELETE para eliminar todos los vuelos.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>

        [HttpDelete("DeleteAll")]
        [SwaggerOperation("Eliminar todo los datos (Usar con precaucion)")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Se han eliminado todos los datos con exito", typeof(string))]
        public override async Task<IActionResult> DeleteAll()
        {
            var func = await base.DeleteAll();
            return func;

        }

        /// <summary>
        /// Método HTTP GET para obtener todos los vuelos.
        /// </summary>
        /// <returns>Lista de vuelos.</returns>

        [HttpGet("GetAll")]
        [SwaggerOperation("Obtener todos los vuelo especifico")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Consulta realizada con exito", typeof(List<Vuelo>))]
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }

        /// <summary>
        /// Método HTTP GET para obtener un vuelo por su ID.
        /// </summary>
        /// <param name="id">ID del vuelo a obtener.</param>
        /// <returns>El vuelo solicitado.</returns>

        [HttpGet("GetById/{id}")]
        [SwaggerOperation("Obtener un vuelo especifico")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Consulta unica Realizada con exito", typeof(Vuelo))]

        public override async Task<IActionResult> GetById(int id)
        {
            var func = await base.GetById(id);
            return func;
        }

        /// <summary>
        /// Procedimiento para insertar un nuevo vuelo en la base de datos.
        /// </summary>

        protected override async Task<string> InsertProcedure(Vuelo entity)
        {
            try
            {
                GeoCoordinate DespegueCord = new GeoCoordinate(entity.Aeropuerto_Despegue.Coordenadas.Latitud, entity.Aeropuerto_Despegue.Coordenadas.Longitud);

                // Coordenadas de Olaya Herrera, Medellín
                GeoCoordinate DestinoCord = new GeoCoordinate(entity.Aeropuerto_Destino.Coordenadas.Latitud, entity.Aeropuerto_Destino.Coordenadas.Longitud);
                double distance = DespegueCord.GetDistanceTo(DestinoCord) / 1000; 


                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_preciovuelo", double.Parse(entity.Preciovuelo.ToString())),
            new NpgsqlParameter("v_tarifatemporada", double.Parse(entity.Tarifatemporada.ToString())),
            new NpgsqlParameter("v_descuento", double.Parse(entity.Descuento.ToString())),
            new NpgsqlParameter("v_fechayhoradespegue", entity.Fechayhoradesalida),
            new NpgsqlParameter("v_iddespegue", entity.Aeropuerto_Despegue.Idaereopuerto),
            new NpgsqlParameter("v_iddestino", entity.Aeropuerto_Destino.Idaereopuerto),
            new NpgsqlParameter("v_idavion", entity.Avion.Idavion),
            new NpgsqlParameter("v_distancia", distance)
                };
                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_vuelo(@v_distancia, @v_preciovuelo, @v_tarifatemporada, @v_descuento, @v_fechayhoradespegue, @v_iddespegue, @v_iddestino, @v_idavion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Procedimiento para eliminar un vuelo de la base de datos.
        /// </summary>
       
        protected override async Task<string> DeleteProcedure(int id_vuelo)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_vuelo", id_vuelo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_vuelo(@id_vuelo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Procedimiento para actualizar un vuelo en la base de datos.
        /// </summary>

        protected override async Task<string> UpdateProcedure(Vuelo nuevoVuelo, int id_vuelo)
        {
            try
            {
                GeoCoordinate DespegueCord = new GeoCoordinate(nuevoVuelo.Aeropuerto_Despegue.Coordenadas.Latitud, nuevoVuelo.Aeropuerto_Despegue.Coordenadas.Longitud);

                // Coordenadas de Olaya Herrera, Medellín
                GeoCoordinate DestinoCord = new GeoCoordinate(nuevoVuelo.Aeropuerto_Destino.Coordenadas.Latitud, nuevoVuelo.Aeropuerto_Destino.Coordenadas.Longitud);
                double distance = DespegueCord.GetDistanceTo(DestinoCord) / 1000;

                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_vuelo", id_vuelo),
            new NpgsqlParameter("nuevo_precio_vuelo", nuevoVuelo.Preciovuelo),
            new NpgsqlParameter("nueva_tarifatemporada", nuevoVuelo.Tarifatemporada),
            new NpgsqlParameter("nuevo_descuento", nuevoVuelo.Descuento),
            new NpgsqlParameter("nueva_distancia_recorrida", distance),
            new NpgsqlParameter("nueva_fecha_hora_llegada", nuevoVuelo.Fechayhorallegada),
            new NpgsqlParameter("nuevo_cupo", nuevoVuelo.Cupo),
            new NpgsqlParameter("nuevo_id_despegue", nuevoVuelo.Aeropuerto_Despegue.Idaereopuerto),
            new NpgsqlParameter("nuevo_id_destino", nuevoVuelo.Aeropuerto_Destino.Idaereopuerto),
            new NpgsqlParameter("nuevo_id_estado", nuevoVuelo.Estado.Idestado),
            new NpgsqlParameter("nuevo_id_avion", nuevoVuelo.Avion.Idavion),
                };
                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_vuelo(@id_vuelo, @nuevo_precio_vuelo, @nueva_tarifatemporada, @nuevo_descuento, @nueva_distancia_recorrida, @nueva_fecha_hora_llegada, @nuevo_cupo, @nuevo_id_despegue, @nuevo_id_destino, @nuevo_id_estado, @nuevo_id_avion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Método para obtener la lista de vuelos desde el contexto.
        /// </summary>

        protected override async Task<List<Vuelo>> SetContextList()
        {
            var list = await _context.Set<Vuelo>()
                    .Include(arg => arg.Aeropuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aeropuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Aeropuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aeropuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Estado)
                    .Include(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                    .Include(arg => arg.Estado)
                .ToListAsync();

            // Eliminar repeticiones:
            //list = list.GroupBy(boleto => boleto.Asiento.Idasiento).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Idvuelo).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Despegue.Idaereopuerto).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Destino.Idaereopuerto).Select(group => group.First()).ToList();

            return list;
        }

        /// <summary>
        /// Método para obtener un vuelo específico desde el contexto por su ID.
        /// </summary>

        protected override async Task<Vuelo> SetContextEntity(int id)
        {
            var entity = await _context.Set<Vuelo>()
                .Include(arg => arg.Aeropuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aeropuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Aeropuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aeropuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                    .Include(arg => arg.Estado)
         .FirstOrDefaultAsync(a => a.Idvuelo == id);
            return entity;
        }

        /// <summary>
        /// Método para obtener todos los vuelos disponibles.
        /// </summary>

        [HttpGet]
        [Route("GetAllAvailable")]
        [Authorize]
        [SwaggerOperation("Obtener los vuelos que tengan cupo y esten disponibles")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Operacion Realizada con exito", typeof(List<Avion>))] 
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Error interno del servidor")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "No autorizado, por favor solicitar el token")]

        public virtual async Task<IActionResult> GetAvailable()
        {
            List<Vuelo> lista = new();
            try
            {
                lista = await _context.Set<Vuelo>()
                  .Include(arg => arg.Aeropuerto_Despegue)
                  .ThenInclude(arg => arg.Ciudad)
                  .ThenInclude(arg => arg.Region)
                  .ThenInclude(arg => arg.Pais)
                  .Include(arg => arg.Aeropuerto_Despegue)
                  .ThenInclude(arg => arg.Coordenadas)
                  .Include(arg => arg.Aeropuerto_Destino)
                  .ThenInclude(arg => arg.Ciudad)
                  .ThenInclude(arg => arg.Region)
                  .ThenInclude(arg => arg.Pais)
                  .Include(arg => arg.Aeropuerto_Destino)
                  .ThenInclude(arg => arg.Coordenadas)
                  .Include(arg => arg.Estado)
                  .Include(arg => arg.Avion)
                  .ThenInclude(arg => arg.Aereolinea)
              .ToListAsync();
                lista = lista.FindAll(item => item.Cupo && item.Estado.Idestado == 8);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = lista });
            }
        }

        /// <summary>
        /// Método para obtener los asientos disponibles para un vuelo específico.
        /// </summary>
        /// <param name="idVuelo">ID del vuelo.</param>

        [HttpGet]
        [Route("Vuelo/{idVuelo}/Avion/AsientosDisponibles")]
        [SwaggerOperation("Obtener asientos disponibles y ocupados de un vuelo especifico")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Operacion Realizada con exito", typeof(List<Asiento>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Error interno del servidor")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "No autorizado, por favor solicitar el token")]
        public virtual async Task<IActionResult> GetAsientosVuelo(int idVuelo)
        {
            List<Asiento> AsientosDisponibles = new();
            List<Asiento> AsientosOcupados = new();

            try
            {
                Vuelo vuelo = await _context.Set<Vuelo>()
                      .Include(arg => arg.Avion)
                      .ThenInclude(arg => arg.Asientos)
                      .ThenInclude(arg => arg.Categoria)
                  .FirstOrDefaultAsync(item => item.Idvuelo == idVuelo);

                var BoletosList = await _context.Set<Boleto>()
            .Include(arg => arg.Asiento)
            .ThenInclude(arg => arg.Categoria)
            .Include(arg => arg.Vuelo)
            .ToListAsync();

                foreach (var boleto in BoletosList)
                {
                    if (boleto.Vuelo.Idvuelo == idVuelo)
                    {
                        AsientosOcupados.Add(boleto.Asiento);
                    }
                }
                 AsientosDisponibles = vuelo.Avion.Asientos.Except(AsientosOcupados).ToList().FindAll(item => item.Categoria.Comercial);


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, AsientosOcupados, AsientosDisponibles });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = AsientosDisponibles });
            }
        }
    }
}