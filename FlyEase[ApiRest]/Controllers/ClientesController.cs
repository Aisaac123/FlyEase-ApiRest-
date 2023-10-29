using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlyEase_ApiRest_.Controllers
{
    public class ClientesController : CrudController<Cliente, string, FlyEaseDataBaseContext>
    {
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
    }
}