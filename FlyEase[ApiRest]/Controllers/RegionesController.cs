using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class RegionesController : CrudController<Region, int, FlyEaseDataBaseContextAuthentication>
    {
        public RegionesController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Region entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_region", entity.Nombre),
                    new NpgsqlParameter("nombre_pais", entity.Pais.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_region(@nombre_region, @nombre_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(int id_region)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("id_region", id_region)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_region(@id_region)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Region nuevaRegion, int id_region)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_region", id_region),
            new NpgsqlParameter("nuevo_nombre", nuevaRegion.Nombre),
                    new NpgsqlParameter("nuevo_id_pais", nuevaRegion.Pais.Idpais)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_region(@id_region, @nuevo_nombre, @nuevo_id_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<List<Region>> SetContextList()
        {
            var list = await _context.Set<Region>()
          .Include(a => a.Pais)
          .ToListAsync();
            return list;
        }

        protected override async Task<Region> SetContextEntity(int id)
        {
            var entity = await _context.Set<Region>()
         .Include(a => a.Pais)
         .FirstOrDefaultAsync(a => a.Idregion == id);

            return entity;
        }
    }
}