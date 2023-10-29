using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    public class AvionesController : CrudController<Avion, string, FlyEaseDataBaseContext>
    {
        public AvionesController(FlyEaseDataBaseContext context) : base(context)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Avion entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_idavion", entity.Idavion),
            new NpgsqlParameter("nombre_avion", entity.Nombre),
            new NpgsqlParameter("modelo_avion", entity.Modelo),
            new NpgsqlParameter("fabricante_avion", entity.Fabricante),
            new NpgsqlParameter("v_velocidadpromedio", entity.Velocidadpromedio),
            new NpgsqlParameter("v_cantidadpasajeros", entity.Cantidadpasajeros),
            new NpgsqlParameter("v_cantidadcarga", entity.Cantidadcarga),
            new NpgsqlParameter("id_aereolinea", entity.Aereolinea.Idaereolinea)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_avion(@v_idavion, @nombre_avion, @modelo_avion, @fabricante_avion, @v_velocidadpromedio, @v_cantidadpasajeros, @v_cantidadcarga, @id_aereolinea)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(string id_avion)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_avion", id_avion)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_avion(@id_avion)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Avion nuevoAvion, string id_avion)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_avion", id_avion),
            new NpgsqlParameter("nuevo_nombre", nuevoAvion.Nombre),
            new NpgsqlParameter("nuevo_modelo", nuevoAvion.Modelo),
            new NpgsqlParameter("nuevo_fabricante", nuevoAvion.Fabricante),
            new NpgsqlParameter("nueva_velocidad_promedio", nuevoAvion.Velocidadpromedio),
            new NpgsqlParameter("nueva_cantidad_pasajeros", nuevoAvion.Cantidadpasajeros),
            new NpgsqlParameter("nueva_cantidad_carga", nuevoAvion.Cantidadcarga),
            new NpgsqlParameter("nuevo_id_aereolinea", nuevoAvion.Idaereolinea)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_avion(@id_avion, @nuevo_nombre, @nuevo_modelo, @nuevo_fabricante, @nueva_velocidad_promedio, @nueva_cantidad_pasajeros, @nueva_cantidad_carga, @nuevo_id_aereolinea)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<List<Avion>> SetContextList()
        {
            var list = await _context.Set<Avion>()
          .Include(a => a.Aereolinea)
          .ToListAsync();
            return list;
        }

        protected override async Task<Avion> SetContextEntity(string id)
        {
            var entity = await _context.Set<Avion>()
         .Include(a => a.Aereolinea)
         .FirstOrDefaultAsync(a => a.Idavion == id);

            return entity;
        }
    }
}