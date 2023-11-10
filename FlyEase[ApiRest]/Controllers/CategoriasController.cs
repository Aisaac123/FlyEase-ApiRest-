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

    public class CategoriasController : CrudController<Categoria, int, FlyEaseDataBaseContextAuthentication>
    {
        public CategoriasController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }
        protected override async Task<string> InsertProcedure(Categoria entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_nombre", entity.Nombre),
            new NpgsqlParameter("v_descripcion", entity.Descripcion),
            new NpgsqlParameter("v_estadocategoria", bool.Parse(entity.Estadocategoria.ToString())),
            new NpgsqlParameter("v_tarifa", entity.Tarifa)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_categoria(@v_nombre, @v_descripcion, @v_estadocategoria, @v_tarifa)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> DeleteProcedure(int id_categoria)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_categoria", id_categoria)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_eliminar_categoria(@id_categoria)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected override async Task<string> UpdateProcedure(Categoria nuevaCategoria, int id_categoria)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("id_categoria", id_categoria),
            new NpgsqlParameter("nuevo_nombre", nuevaCategoria.Nombre),
            new NpgsqlParameter("nueva_descripcion", nuevaCategoria.Descripcion),
            new NpgsqlParameter("nuevo_estado_categoria", nuevaCategoria.Estadocategoria),
            new NpgsqlParameter("nueva_tarifa", nuevaCategoria.Tarifa)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_categoria(@id_categoria, @nuevo_nombre, @nueva_descripcion, @nuevo_estado_categoria, @nueva_tarifa)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
