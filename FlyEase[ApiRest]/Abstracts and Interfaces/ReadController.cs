using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    [EnableCors("Reglas")]
    [Route("FlyEaseApi/[controller]")]
    [ApiController]
    public abstract class ReadController<TEntity,IdType, TContext> : Controller, IControllerRead<IdType>
        where TEntity : class
        where TContext : DbContext
    {


        protected TContext _context;
        public ReadController(TContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAll")]
        //[Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Get()
        {
            List<TEntity> lista = new();
            try
            {
                lista = await SetContextList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false, response = lista });
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
      //  [Authorize]
        public async Task<IActionResult> GetById(IdType id)
        {
            try
            {
                var entity = await SetContextEntity(id);
                if (entity == null)
                {
                    return BadRequest("No se ha encontrado");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Succes = true, response = entity });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false });
            }
        }
        protected async virtual Task<List<TEntity>> SetContextList()
        {
            var list = await _context.Set<TEntity>().ToListAsync();
            return list;
        }
        protected async virtual Task<TEntity> SetContextEntity(IdType id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity;
        }
    }
}