using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    public class BoletosController : CrudController<Boleto, int, FlyEaseDataBaseContext>
    {
        protected override async Task<string> InsertProcedure(Boleto entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_descuento", entity.Descuento),
            new NpgsqlParameter("v_numero_documento", entity.Cliente.Numerodocumento),
            new NpgsqlParameter("v_idasiento", entity.Asiento.Idasiento),
            new NpgsqlParameter("v_idvuelo", entity.Vuelo.Idvuelo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_boleto(@v_descuento, @v_numero_documento, @v_idasiento, @v_idvuelo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(int id_boleto)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_boleto", id_boleto)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_boleto(@id_boleto)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Boleto nuevoBoleto, int id_boleto)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_boleto", id_boleto),
            new NpgsqlParameter("nuevo_precio", nuevoBoleto.Precio),
            new NpgsqlParameter("nuevo_descuento", nuevoBoleto.Descuento),
            new NpgsqlParameter("nuevo_precio_total", nuevoBoleto.Preciototal),
            new NpgsqlParameter("nuevo_numero_documento_cliente", nuevoBoleto.Cliente.Numerodocumento),
            new NpgsqlParameter("nuevo_id_asiento", nuevoBoleto.Asiento.Idasiento),
            new NpgsqlParameter("nuevo_id_vuelo", nuevoBoleto.Vuelo.Idvuelo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_boleto(@id_boleto, @nuevo_precio, @nuevo_descuento, @nuevo_precio_total, @nuevo_numero_documento_cliente, @nuevo_id_asiento, @nuevo_id_vuelo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<List<Boleto>> SetContextList()
        {
            var list = await _context.Set<Boleto>()
          .Include(a => a.Asiento)
          .ThenInclude(a => a.Avion)
          .ThenInclude(a => a.Aereolinea)
          .Include(a => a.Vuelo)
          .ThenInclude(a => a.Aereopuerto_Despegue)
          .ThenInclude(a => a.Ciudad)
          .ThenInclude(a => a.Region)
          .ThenInclude(a => a.Pais)
         .ToListAsync();
            return list;
        }

        protected override async Task<Boleto> SetContextEntity(int id)
        {
            var entity = await _context.Set<Boleto>()
         .Include(a => a.Asiento)
          .ThenInclude(a => a.Avion)
          .ThenInclude(a => a.Aereolinea)
          .Include(a => a.Vuelo)
          .ThenInclude(a => a.Aereopuerto_Despegue)
          .ThenInclude(a => a.Ciudad)
          .ThenInclude(a => a.Region)
          .ThenInclude(a => a.Pais)
         .FirstOrDefaultAsync(a => a.Idasiento == id);

            return entity;
        }
    }
}