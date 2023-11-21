using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Swashbuckle.AspNetCore.Annotations;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    /// <summary>
    /// Controlador para gestionar operaciones CRUD de Categorías.
    /// </summary>

    [SwaggerTag("Metodos Crud para Categorias")]

    public class CategoriasController : CrudController<Categoria, int, FlyEaseDataBaseContextAuthentication>
    {

        /// <summary>
        /// Constructor del controlador de Categorias.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="hubContext">Contexto del hub.</param>

        public CategoriasController(FlyEaseDataBaseContextAuthentication context, IHubContext<WebSocketHub> hubContext) : base(context, hubContext)
        {
            _context = context;
        }
        
        /// <summary>
        /// Crea un nuevo registro de Categoría.
        /// </summary>
        /// <param name="entity">Datos de la Categoría a crear.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        /// 
        
        [HttpPost("Post")]
        [Authorize(Policy = "Admin Policy")]

        [SwaggerOperation("Registrar una nueva categoría.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> Post([FromBody] Categoria entity)
        {
            var func = await base.Post(entity);
            return func;

        }
       
        /// <summary>
        /// Actualiza un registro de Categoría en la base de datos por su ID.
        /// </summary>
        /// <param name="entity">Datos de la Categoría a actualizar.</param>
        /// <param name="Id">ID de la Categoría a actualizar.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        
        [HttpPut("Put/{Id}")]
        [Authorize(Policy = "Admin Policy")]

        [SwaggerOperation("Actualizar una categoría por su ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> Put([FromBody] Categoria entity, int Id)
        {
            var func = await base.Put(entity, Id);
            return func;

        }
        
        /// <summary>
        /// Elimina un registro de Categoría de la base de datos por su ID.
        /// </summary>
        /// <param name="Id">ID de la Categoría a eliminar.</param>
        /// <returns>Respuesta de la solicitud.</returns>
        
        [HttpDelete("Delete/{Id}")]
        [SwaggerOperation("Eliminar una categoría por su ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> Delete(int Id)
        {
            var func = await base.Delete(Id);
            return func;

        }
       
        /// <summary>
        /// Elimina todos los registros de Categorías de la base de datos.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>
        
        [HttpDelete("DeleteAll")]
        [SwaggerOperation("Eliminar todas las categorías.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(string))]
        public override async Task<IActionResult> DeleteAll()
        {
            var func = await base.DeleteAll();
            return func;

        }
       
        /// <summary>
        /// Obtiene la lista de Categorías desde la base de datos.
        /// </summary>
        /// <returns>Respuesta de la solicitud.</returns>
       
        [HttpGet("GetAll")]
        [Authorize(Policy = "Admin Policy")]

        [SwaggerOperation("Obtener todas las categorías.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(List<Categoria>))]
        public override async Task<IActionResult> Get()
        {
            var func = await base.Get();
            return func;
        }
       
        /// <summary>
        /// Obtiene un registro de Categoría desde la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID de la Categoría a obtener.</param>
        /// <returns>Respuesta de la solicitud.</returns>
       
        [HttpGet("GetById/{id}")]
        [Authorize(Policy = "Admin Policy")]

        [SwaggerOperation("Obtener una categoría por su ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa", typeof(Categoria))]
        public override async Task<IActionResult> GetById(int id)
        {
            var func = await base.GetById(id);
            return func;
        }
      
        /// <summary>
        /// Inserta un nuevo registro de Categoría en la base de datos.
        /// </summary>
        /// <param name="entity">Datos de la Categoría a insertar.</param>
        /// <returns>Resultado de la operación.</returns>
       
        protected override async Task<string> InsertProcedure(Categoria entity)
        {
            try
            {
                var parameters = new NpgsqlParameter[]
                {
            new NpgsqlParameter("v_nombre", entity.Nombre),
            new NpgsqlParameter("v_descripcion", entity.Descripcion),
            new NpgsqlParameter("v_estadocategoria", entity.Estadocategoria),
            new NpgsqlParameter("v_tarifa", entity.Tarifa),
            new NpgsqlParameter("v_comercial", entity.Comercial)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_insertar_categoria(@v_nombre, @v_descripcion, @v_estadocategoria, @v_tarifa, @v_comercial)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
       
        /// <summary>
        /// Elimina un registro de Categoría de la base de datos por su ID.
        /// </summary>
        /// <param name="id_categoria">ID de la Categoría a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>

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
        /// <summary>
        /// Actualiza un registro de Categoría en la base de datos por su ID.
        /// </summary>
        /// <param name="nuevaCategoria">Nueva información de la Categoría a actualizar.</param>
        /// <param name="id_categoria">ID de la Categoría a actualizar.</param>
        /// <returns>Resultado de la operación.</returns>
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
            new NpgsqlParameter("nueva_tarifa", nuevaCategoria.Tarifa),
            new NpgsqlParameter("nuevo_comercial", nuevaCategoria.Comercial)
                };

                await _context.Database.ExecuteSqlRawAsync("CALL p_actualizar_categoria(@id_categoria, @nuevo_nombre, @nueva_descripcion, @nuevo_estado_categoria, @nueva_tarifa, @nuevo_comercial)", parameters);
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
