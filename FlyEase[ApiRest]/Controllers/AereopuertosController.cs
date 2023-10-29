using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    public class AereopuertosController : CrudController<Aereopuerto, int, FlyEaseDataBaseContext>
    {
        public AereopuertosController(FlyEaseDataBaseContext context) : base(context)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Aereopuerto entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_pais", entity.Ciudad.Region.Pais.Nombre),
            new NpgsqlParameter("nombre_region",  entity.Ciudad.Region.Nombre),
            new NpgsqlParameter("nombre_ciudad",  entity.Ciudad.Nombre),
            new NpgsqlParameter("v_latitud", entity.Coordenadas.Latitud),
            new NpgsqlParameter("v_longitud",  entity.Coordenadas.Longitud),
            new NpgsqlParameter("nombre_aereopuerto", entity.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_aereopuerto(@nombre_pais, @nombre_region, @nombre_ciudad, @v_latitud, @v_longitud, @nombre_aereopuerto)", parameters);
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
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_aereopuerto", id_aereopuerto),
            new NpgsqlParameter("nuevo_nombre", nuevoAereopuerto.Nombre),
            new NpgsqlParameter("nuevo_id_ciudad", nuevoAereopuerto.Idciudad),
            new NpgsqlParameter("nuevo_id_coordenada", nuevoAereopuerto.Idcoordenada)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_aereopuerto(@id_aereopuerto, @nuevo_nombre, @nuevo_id_ciudad, @nuevo_id_coordenada)", parameters);
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