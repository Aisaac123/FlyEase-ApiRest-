﻿using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

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
                NpgsqlParameter v_imagen;

                if (entity.Imagen != null)
                {
                    v_imagen = new NpgsqlParameter("v_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = entity.Imagen // Valor de la imagen si no es nulo
                    };
                }
                else
                {
                    v_imagen = new NpgsqlParameter("v_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = DBNull.Value // Valor nulo
                    };
                }

                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_preciovuelo", double.Parse(entity.Preciovuelo.ToString())),
            new NpgsqlParameter("v_tarifatemporada", double.Parse(entity.Tarifatemporada.ToString())),
            new NpgsqlParameter("v_descuento", double.Parse(entity.Descuento.ToString())),
            new NpgsqlParameter("v_fechayhoradespegue", entity.Fechayhoradesalida),
            new NpgsqlParameter("v_iddespegue", entity.Aereopuerto_Despegue.Idaereopuerto),
            new NpgsqlParameter("v_iddestino", entity.Aereopuerto_Destino.Idaereopuerto),
            new NpgsqlParameter("v_idavion", entity.Avion.Idavion),
            v_imagen
                };
                if (v_imagen.Value == DBNull.Value)
                {
                    await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_vuelo(@v_preciovuelo, @v_tarifatemporada, @v_descuento, @v_fechayhoradespegue, @v_iddespegue, @v_iddestino, @v_idavion)", parameters);
                    return "Ok";
                }
                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_vuelo(@v_preciovuelo, @v_tarifatemporada, @v_descuento, @v_fechayhoradespegue, @v_iddespegue, @v_iddestino, @v_idavion, @v_imagen)", parameters);
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
                NpgsqlParameter v_imagen;

                if (nuevoVuelo.Imagen != null)
                {
                    v_imagen = new NpgsqlParameter("v_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = nuevoVuelo.Imagen // Valor de la imagen si no es nulo
                    };
                }
                else
                {
                    v_imagen = new NpgsqlParameter("v_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = DBNull.Value // Valor nulo
                    };
                }
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
            v_imagen
                };
                if (v_imagen.Value == DBNull.Value)
                {
                    await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_vuelo(@id_vuelo, @nuevo_precio_vuelo, @nueva_tarifatemporada, @nuevo_descuento, @nueva_distancia_recorrida, @nueva_fecha_hora_llegada, @nuevo_cupo, @nuevo_id_despegue, @nuevo_id_destino, @nuevo_id_estado, @nuevo_id_avion)", parameters);
                    return "Ok";
                }

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_vuelo(@id_vuelo, @nuevo_precio_vuelo, @nueva_tarifatemporada, @nuevo_descuento, @nueva_distancia_recorrida, @nueva_fecha_hora_llegada, @nuevo_cupo, @nuevo_id_despegue, @nuevo_id_destino, @nuevo_id_estado, @nuevo_id_avion, @nueva_imagen)", parameters);
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
    }
}