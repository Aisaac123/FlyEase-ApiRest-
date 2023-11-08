using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class ClientesController : CrudController<Cliente, string, FlyEaseDataBaseContextPrueba>
    {
        public ClientesController(FlyEaseDataBaseContextPrueba context) : base(context)
        {
            _context = context;
        }
        protected override async Task<string> InsertProcedure(Cliente entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_numerodocumento", entity.Numerodocumento),
            new NpgsqlParameter("v_tipodocumento", entity.Tipodocumento),
            new NpgsqlParameter("v_nombres", entity.Nombres),
            new NpgsqlParameter("v_apellidos", entity.Apellidos),
            new NpgsqlParameter("v_celular", entity.Celular),
            new NpgsqlParameter("v_correo", entity.Correo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_cliente(@v_numerodocumento, @v_tipodocumento, @v_nombres, @v_apellidos, @v_celular, @v_correo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Cliente entity, string OldId)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("numero_documento_cliente", OldId),
            new NpgsqlParameter("nuevo_documento", entity.Numerodocumento),
            new NpgsqlParameter("nuevo_tipo_documento", entity.Tipodocumento),
            new NpgsqlParameter("nuevo_nombres", entity.Nombres),
            new NpgsqlParameter("nuevo_apellidos", entity.Apellidos),
            new NpgsqlParameter("nuevo_celular", entity.Celular),
            new NpgsqlParameter("nuevo_correo", entity.Correo)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_cliente(@numero_documento_cliente, @nuevo_documento, @nuevo_tipo_documento, @nuevo_nombres, @nuevo_apellidos, @nuevo_celular, @nuevo_correo)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(string Old_Id)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("numero_documento_cliente", Old_Id),
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_cliente(@numero_documento_cliente) ", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        [Route("GetBoletosByCliente/{id_cliente}")]
        public async Task<IActionResult> GetAsiento(string id_cliente)
        {
            try
            {
                var entity = await _context.Set<Cliente>()
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Avion)
                    .ThenInclude(arg => arg.Aereolinea)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Asiento)
                    .ThenInclude(arg => arg.Categoria)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Despegue)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Ciudad)
                    .ThenInclude(arg => arg.Region)
                    .ThenInclude(arg => arg.Pais)
                .Include(a => a.Boletos)
                    .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Aereopuerto_Destino)
                    .ThenInclude(arg => arg.Coordenadas)
                .Include(a => a.Boletos)
                .ThenInclude(arg => arg.Vuelo)
                    .ThenInclude(arg => arg.Estado)
                    .FirstOrDefaultAsync(a => a.Numerodocumento == id_cliente);

                if (entity != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, response = entity.Boletos });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Cliente no encontrado", Succes = false, response = new List<Asiento>() });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = new List<Asiento>() });
            }
        }
    }
}