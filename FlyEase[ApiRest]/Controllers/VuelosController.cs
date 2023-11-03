using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class VuelosController : CrudController<Vuelo, int, FlyEaseDataBaseContextPrueba>
    {
        public VuelosController(FlyEaseDataBaseContextPrueba context) : base(context)
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
            new NpgsqlParameter("v_idavion", entity.Avion.Idavion)
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
            new NpgsqlParameter("nuevo_id_avion", nuevoVuelo.Avion.Idavion)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_vuelo(@id_vuelo, @nuevo_precio_vuelo, @nueva_tarifatemporada, @nuevo_descuento, @nueva_distancia_recorrida, @nueva_fecha_hora_llegada, @nuevo_cupo, @nuevo_id_despegue, @nuevo_id_destino, @nuevo_id_estado, @nuevo_id_avion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}