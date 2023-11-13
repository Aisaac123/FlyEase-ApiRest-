using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    public class AvionesController : CrudController<Avion, string, FlyEaseDataBaseContextAuthentication>
    {
        public AvionesController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
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
            new NpgsqlParameter("new_idavion", nuevoAvion.Idavion),
            new NpgsqlParameter("nuevo_nombre", nuevoAvion.Nombre),
            new NpgsqlParameter("nuevo_modelo", nuevoAvion.Modelo),
            new NpgsqlParameter("nuevo_fabricante", nuevoAvion.Fabricante),
            new NpgsqlParameter("nueva_velocidad_promedio", nuevoAvion.Velocidadpromedio),
            new NpgsqlParameter("nueva_cantidad_pasajeros", nuevoAvion.Cantidadpasajeros),
            new NpgsqlParameter("nueva_cantidad_carga", nuevoAvion.Cantidadcarga),
            new NpgsqlParameter("nuevo_id_aereolinea", nuevoAvion.Aereolinea.Idaereolinea)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_avion(@id_avion, @new_idavion, @nuevo_nombre, @nuevo_modelo, @nuevo_fabricante, @nueva_velocidad_promedio, @nueva_cantidad_pasajeros, @nueva_cantidad_carga, @nuevo_id_aereolinea)", parameters);
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

        [HttpGet]
        [Route("GetAsientos/{id_avion}")]
        public async Task<IActionResult> GetAsiento(string id_avion)
        {
            try
            {
                var entity = await _context.Set<Avion>().Include(a => a.Asientos).ThenInclude(a => a.Categoria).Include(a => a.Aereolinea).FirstOrDefaultAsync(a => a.Idavion == id_avion);

                if (entity != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, response = entity.Asientos });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Avión no encontrado", Succes = false, response = new List<Asiento>() });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = new List<Asiento>() });
            }
        }
    }
}