using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    public class CiudadesController : CrudController<Ciudad, int, FlyEaseDataBaseContext>
    {
        protected override async Task<string> InsertProcedure(Ciudad entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_ciudad", entity.Nombre),
            new NpgsqlParameter("nombre_region", entity.Region.Nombre),
                    new NpgsqlParameter("nombre_pais", entity.Region.Pais.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_ciudad(@nombre_ciudad, @nombre_region, @nombre_pais)", parameters);
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
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_ciudad", id_ciudad),
            new NpgsqlParameter("nuevo_nombre", nuevaCiudad.Nombre),
                    new NpgsqlParameter("nuevo_id_region", nuevaCiudad.Idregion)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_ciudad(@id_ciudad, @nuevo_nombre, @nuevo_id_region)", parameters);
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
