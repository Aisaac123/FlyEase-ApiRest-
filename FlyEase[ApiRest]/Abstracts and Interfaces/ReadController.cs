using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public abstract class ReadController<TEntity,IdType, TContext> : ControllerBase, IControllerRead<IdType>
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
        public async Task<IActionResult> Get()
        {
            List<TEntity> lista = new();
            try
            {
                lista = await _context.Set<TEntity>().ToListAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(IdType id)
        {
            try
            {
                var id_s = id.ToString();
                var entity = await _context.Set<TEntity>().FindAsync(id_s);
                if (entity == null)
                {
                    return BadRequest("No se ha encontrado");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = entity });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}