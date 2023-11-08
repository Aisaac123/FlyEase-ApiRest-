using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.SignalR;

using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class CiudadesController : CrudController<Ciudad, int, FlyEaseDataBaseContextPrueba>
    {
        public CiudadesController(FlyEaseDataBaseContextPrueba context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Ciudad entity)
        {
            try
            {
                NpgsqlParameter v_imagen;

                if (entity.Imagen != null)
                {
                    v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = entity.Imagen // Valor de la imagen si no es nulo
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
                    new NpgsqlParameter("nombre_ciudad", entity.Nombre),
                    new NpgsqlParameter("nombre_region", entity.Region.Nombre),
                    new NpgsqlParameter("nombre_pais", entity.Region.Pais.Nombre),
                    v_imagen

                };
                if (v_imagen.Value == DBNull.Value)
                {
                    await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_ciudad(@nombre_ciudad, @nombre_region, @nombre_pais)", parameters);
                    return "Ok";
                }
                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_ciudad(@nombre_ciudad, @nombre_region, @nombre_pais, @_imagen)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(int id_ciudad)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("id_ciudad", id_ciudad)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_ciudad(@id_ciudad)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Ciudad nuevaCiudad, int id_ciudad)
        {
            try
            {
                NpgsqlParameter v_imagen;

                if (nuevaCiudad.Imagen != null)
                {
                    v_imagen = new NpgsqlParameter("_imagen", NpgsqlDbType.Bytea)
                    {
                        Value = nuevaCiudad.Imagen // Valor de la imagen si no es nulo
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
                    new NpgsqlParameter("id_ciudad", id_ciudad),
                    new NpgsqlParameter("nuevo_nombre", nuevaCiudad.Nombre),
                    new NpgsqlParameter("nuevo_id_region", nuevaCiudad.Idregion),
                    v_imagen
                };
                if (v_imagen.Value == DBNull.Value)
                {
                    await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_ciudad(@id_ciudad, @nuevo_nombre, @nuevo_id_region)", parameters);
                    return "Ok";
                }
                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_ciudad(@id_ciudad, @nuevo_nombre, @nuevo_id_region, @_imagen)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<List<Ciudad>> SetContextList()
        {
            var list = await _context.Set<Ciudad>()
          .Include(a => a.Region)
            .ThenInclude(c => c.Pais)
          .ToListAsync();
            return list;
        }

        protected override async Task<Ciudad> SetContextEntity(int id)
        {
            var entity = await _context.Set<Ciudad>()
                   .Include(a => a.Region)
            .ThenInclude(c => c.Pais)
         .FirstOrDefaultAsync(a => a.Idciudad == id);

            return entity;
        }
    }
}