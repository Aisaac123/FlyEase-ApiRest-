using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class BoletosController : CrudController<Boleto, int, FlyEaseDataBaseContextPrueba>
    {
        public BoletosController(FlyEaseDataBaseContextPrueba context) : base(context)
        {
            _context = context;
        }

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
                .Include(arg => arg.Cliente)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Categoria) 
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Estado)
                .ToListAsync();

            // Eliminar repeticiones:
            //list = list.GroupBy(boleto => boleto.Asiento.Idasiento).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Idvuelo).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Despegue.Idaereopuerto).Select(group => group.First()).ToList();
            //list = list.GroupBy(boleto => boleto.Vuelo.Aereopuerto_Destino.Idaereopuerto).Select(group => group.First()).ToList();

            return list;
        }

        protected override async Task<Boleto> SetContextEntity(int id)
        {
            var entity = await _context.Set<Boleto>()
                .Include(arg => arg.Cliente)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                .Include(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Categoria) 
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Estado)
         .FirstOrDefaultAsync(a => a.Idboleto == id);

            return entity;
        }
    }
}