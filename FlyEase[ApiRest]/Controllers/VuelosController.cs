using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class VuelosController : CrudController<Vuelo, int, FlyEaseDataBaseContextAuthentication>
    {
        public VuelosController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Vuelo entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_preciovuelo", double.Parse(entity.Preciovuelo.ToString())),
            new NpgsqlParameter("v_tarifatemporada", double.Parse(entity.Tarifatemporada.ToString())),
            new NpgsqlParameter("v_descuento", double.Parse(entity.Descuento.ToString())),
            new NpgsqlParameter("v_fechayhoradespegue", entity.Fechayhoradesalida),
            new NpgsqlParameter("v_iddespegue", entity.Aereopuerto_Despegue.Idaereopuerto),
            new NpgsqlParameter("v_iddestino", entity.Aereopuerto_Destino.Idaereopuerto),
            new NpgsqlParameter("v_idavion", entity.Avion.Idavion),
                };
                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_vuelo(@v_preciovuelo, @v_tarifatemporada, @v_descuento, @v_fechayhoradespegue, @v_iddespegue, @v_iddestino, @v_idavion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

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

        protected override async Task<string> UpdateProcedure(Vuelo nuevoVuelo, int id_vuelo)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_vuelo", id_vuelo),
            new NpgsqlParameter("nuevo_precio_vuelo", nuevoVuelo.Preciovuelo),
            new NpgsqlParameter("nueva_tarifatemporada", nuevoVuelo.Tarifatemporada),
            new NpgsqlParameter("nuevo_descuento", nuevoVuelo.Descuento),
            new NpgsqlParameter("nueva_distancia_recorrida", nuevoVuelo.Distanciarecorrida),
            new NpgsqlParameter("nueva_fecha_hora_llegada", nuevoVuelo.Fechayhorallegada),
            new NpgsqlParameter("nuevo_cupo", nuevoVuelo.Cupo),
            new NpgsqlParameter("nuevo_id_despegue", nuevoVuelo.Aereopuerto_Despegue.Idaereopuerto),
            new NpgsqlParameter("nuevo_id_destino", nuevoVuelo.Aereopuerto_Destino.Idaereopuerto),
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

        protected override async Task<List<Vuelo>> SetContextList()
        {
            var list = await _context.Set<Vuelo>()
                    .Include(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Estado)
                    .Include(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                .ToListAsync();

            // Eliminar repeticiones:
            //list = list.GroupBy(boleto => boleto.Asiento.Idasiento).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Idvuelo).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Despegue.Idaereopuerto).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Destino.Idaereopuerto).Select(group => group.First()).ToList();

            return list;
        }

        protected override async Task<Vuelo> SetContextEntity(int id)
        {
            var entity = await _context.Set<Vuelo>()
                .Include(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                    .Include(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                    .Include(arg => arg.Estado)
         .FirstOrDefaultAsync(a => a.Idvuelo == id);
            return entity;
        }

        [HttpGet]
        [Route("GetAllAvailable")]
        [Authorize]
        public virtual async Task<IActionResult> GetAvailable()
        {
            List<Vuelo> lista = new();
            try
            {
                lista = await _context.Set<Vuelo>()
                  .Include(arg => arg.Aereopuerto_Despegue)
                  .ThenInclude(arg => arg.Ciudad)
                  .ThenInclude(arg => arg.Region)
                  .ThenInclude(arg => arg.Pais)
                  .Include(arg => arg.Aereopuerto_Despegue)
                  .ThenInclude(arg => arg.Coordenadas)
                  .Include(arg => arg.Aereopuerto_Destino)
                  .ThenInclude(arg => arg.Ciudad)
                  .ThenInclude(arg => arg.Region)
                  .ThenInclude(arg => arg.Pais)
                  .Include(arg => arg.Aereopuerto_Destino)
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

        [HttpGet]
        [Route("Vuelo/{idVuelo}/Avion/AsientosDisponibles")]
        public virtual async Task<IActionResult> GetAsientosVuelo(int idVuelo)
        {
            List<Asiento> AsientosDisponibles = new();
            List<Asiento> AsientosOcupados = new();

            try
            {
                Vuelo vuelo = await _context.Set<Vuelo>()
                      .Include(arg => arg.Avion)
                      .ThenInclude(arg => arg.Asientos)
                  .FirstOrDefaultAsync(item => item.Idvuelo == idVuelo);

                var BoletosList = await _context.Set<Boleto>()
            .Include(arg => arg.Asiento)
            .Include(arg => arg.Vuelo)
            .ToListAsync();

                foreach (var boleto in BoletosList)
                {
                    if (boleto.Vuelo.Idvuelo == idVuelo)
                    {
                        AsientosOcupados.Add(boleto.Asiento);
                    }
                }
                 AsientosDisponibles = vuelo.Avion.Asientos.Except(AsientosOcupados).ToList();


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, AsientosOcupados, AsientosDisponibles });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = AsientosDisponibles });
            }
        }
    }
}