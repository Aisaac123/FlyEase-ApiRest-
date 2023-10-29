using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    public class AereolineasController : CrudController<Aereolinea, int, FlyEaseDataBaseContext>
    {
        public AereolineasController(FlyEaseDataBaseContext context) : base(context)
        {
            _context = context;
        }

        protected override async Task<string> InsertProcedure(Aereolinea entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("nombre_aereolinea", entity.Nombre),
            new NpgsqlParameter("v_codigo_iata", entity.Codigoiata),
            new NpgsqlParameter("v_codigo_icao", entity.Codigoicao)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_aereolinea(@nombre_aereolinea, @v_codigo_iata, @v_codigo_icao)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        protected override async Task<string> DeleteProcedure(int id_aereolinea)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_aereolinea", id_aereolinea)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_aereolinea(@id_aereolinea)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        protected override async Task<string> UpdateProcedure(Aereolinea nuevaAereolinea, int id_aereolinea)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_aereolinea", id_aereolinea),
            new NpgsqlParameter("nuevo_nombre", nuevaAereolinea.Nombre),
            new NpgsqlParameter("nuevo_codigo_iata", nuevaAereolinea.Codigoiata),
            new NpgsqlParameter("nuevo_codigo_icao", nuevaAereolinea.Codigoicao)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_aereolinea(@id_aereolinea, @nuevo_nombre, @nuevo_codigo_iata, @nuevo_codigo_icao)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
