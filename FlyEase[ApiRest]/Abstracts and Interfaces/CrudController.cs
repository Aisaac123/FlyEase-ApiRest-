using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public abstract class CrudController<TEntity, IdType, TContext> : ReadController<TEntity, IdType, TContext> where TEntity : class where TContext : DbContext, new()
    {
        protected CrudController(TContext context) : base(context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Post")]
        public virtual async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            try
            {
                var mensaje = await InsertProcedure(entity);
                if (mensaje == "Ok")
                {
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operacion Realizada con Exito", Success = true, response = entity });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = mensaje, Success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
            }
        }

        [HttpPut]
        [Route("Put/{Id}")]
        public virtual async Task<IActionResult> Put([FromBody] TEntity entity, IdType Id)
        {
            try
            {
                var mensaje = await UpdateProcedure(entity, Id);
                if (mensaje.ToString() == "Ok")
                {
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operacion Realizada con Exito", Success = true, response = entity });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = mensaje, Success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Success = false });
            }
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public virtual async Task<IActionResult> Delete(IdType Id)
        {
            try
            {


                var mensaje = await DeleteProcedure(Id);
                if (mensaje.ToString() == "Ok")
                {
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operacion Realizada con Exito", Succes = true });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = mensaje, Succes = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false });
            }
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public virtual async Task<IActionResult> DeleteAll()
        {
            try
            {
                var entities = await _context.Set<TEntity>().ToListAsync();
                _context.Set<TEntity>().RemoveRange(entities);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", Succes = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Succes = false });
            }
        }

        protected abstract Task<string> InsertProcedure(TEntity entity);

        protected abstract Task<string> UpdateProcedure(TEntity entity, IdType OldId);

        protected abstract Task<string> DeleteProcedure(IdType Old_Id);
    }
}