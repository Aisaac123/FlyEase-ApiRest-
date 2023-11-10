using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class AereopuertosController : CrudController<Aereopuerto, int, FlyEaseDataBaseContextAuthentication>
    {
        public AereopuertosController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Aereopuerto entity)
        {
            try
            {
                NpgsqlParameter v_imagen;

                if (entity.Ciudad.Imagen != null)
                {
                    v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = entity.Ciudad.Imagen
                    };
                }
                else
                {
                    v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = DBNull.Value // Valor nulo
                    };
                }
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_pais", entity.Ciudad.Region.Pais.Nombre),
            new NpgsqlParameter("nombre_region",  entity.Ciudad.Region.Nombre),
            new NpgsqlParameter("nombre_ciudad",  entity.Ciudad.Nombre),
            new NpgsqlParameter("v_latitud", entity.Coordenadas.Latitud),
            new NpgsqlParameter("v_longitud",  entity.Coordenadas.Longitud),
            new NpgsqlParameter("nombre_aereopuerto", entity.Nombre),
            v_imagen
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_aereopuerto(@nombre_pais, @nombre_region, @nombre_ciudad, @v_latitud, @v_longitud, @nombre_aereopuerto, @_imagen)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(int id_aereopuerto)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("id_aereopuerto", id_aereopuerto)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_aereopuerto(@id_aereopuerto)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Aereopuerto nuevoAereopuerto, int id_aereopuerto)
        {
            try
            {
                NpgsqlParameter v_imagen;

                if (nuevoAereopuerto.Ciudad.Imagen != null)
                {
                    v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = nuevoAereopuerto.Ciudad.Imagen 
                    };
                }
                else
                {
                    v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = DBNull.Value // Valor nulo
                    };
                }
                var parameters = new NpgsqlParameter[]
       {
            new NpgsqlParameter("id_aereopuerto", id_aereopuerto),
            new NpgsqlParameter("nuevo_nombre", nuevoAereopuerto.Nombre),
            new NpgsqlParameter("nueva_ciudad", nuevoAereopuerto.Ciudad.Nombre),
            new NpgsqlParameter("nueva_region", nuevoAereopuerto.Ciudad.Region.Nombre),
            new NpgsqlParameter("nuevo_pais", nuevoAereopuerto.Ciudad.Region.Pais.Nombre),
            new NpgsqlParameter("nueva_latitud", nuevoAereopuerto.Coordenadas.Latitud),
            new NpgsqlParameter("nueva_longitud", nuevoAereopuerto.Coordenadas.Longitud),
            v_imagen
       };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_aereopuerto(@id_aereopuerto, @nuevo_nombre, @nueva_ciudad, @nueva_region, @nuevo_pais, @nueva_latitud, @nueva_longitud, @_imagen)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<List<Aereopuerto>> SetContextList()
        {
            var list = await _context.Set<Aereopuerto>()
          .Include(a => a.Ciudad)
              .ThenInclude(c => c.Region)
                  .ThenInclude(r => r.Pais)
          .Include(a => a.Coordenadas)
          .ToListAsync();
            return list;
        }

        protected override async Task<Aereopuerto> SetContextEntity(int id)
        {
            var entity = await _context.Set<Aereopuerto>()
         .Include(a => a.Ciudad)
             .ThenInclude(c => c.Region)
                 .ThenInclude(r => r.Pais)
         .Include(a => a.Coordenadas)
         .FirstOrDefaultAsync(a => a.Idaereopuerto == id);

            return entity;
        }
    }
}