using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class PaisesController : CrudController<Pais, int, FlyEaseDataBaseContextPrueba>
    {
        public PaisesController(FlyEaseDataBaseContextPrueba context) : base(context)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Pais entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_pais", entity.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_pais(@nombre_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(int id_pais)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_pais", id_pais)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_pais(@id_pais)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Pais nuevoPais, int id_pais)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_pais", id_pais),
            new NpgsqlParameter("nuevo_nombre", nuevoPais.Nombre)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_pais(@id_pais, @nuevo_nombre)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}