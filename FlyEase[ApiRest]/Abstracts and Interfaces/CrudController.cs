using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlyEase_ApiRest_.Abstracts_and_Interfaces
{
    public abstract class CrudController<TEntity, IdType, TContext> : ReadController<TEntity, IdType, TContext> where TEntity : class where TContext : DbContext
    {
        public CrudController(TContext context) : base(context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            try
            {
                var mensaje = await InsertProcedure(entity);
                if (mensaje == "Ok")
                {
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operacion Realizada con Exito", response = entity });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = mensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] TEntity entity, IdType OldId)
        {
            try
            {
                var mensaje = await UpdateProcedure(entity, OldId);
                if (mensaje.ToString() == "Ok")
                {
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operacion Realizada con Exito", response = entity });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = mensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Delete/{Old_Id}")]
        public async Task<IActionResult> Delete(IdType Old_Id)
        {
            var id = Old_Id.ToString();
            try
            {
                var mensaje = await DeleteProcedure(Old_Id);
                if (mensaje.ToString() == "Ok")
                {
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Operacion Realizada con Exito" });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = mensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        protected abstract Task<string> InsertProcedure(TEntity entity);

        protected abstract Task<string> UpdateProcedure(TEntity entity, IdType OldId);

        protected abstract Task<string> DeleteProcedure(IdType Old_Id);
    }
}