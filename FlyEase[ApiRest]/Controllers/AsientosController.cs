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

    public class AsientosController : CrudController<Asiento, int, FlyEaseDataBaseContextPrueba>
    {
        public AsientosController(FlyEaseDataBaseContextPrueba context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }

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

        protected override async Task<List<Asiento>> SetContextList()
        {
            var list = await _context.Set<Asiento>()
          .Include(a => a.Avion)
          .ThenInclude(a => a.Aereolinea)
          .Include(a => a.Categoria)
         .ToListAsync();
            return list;
        }

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