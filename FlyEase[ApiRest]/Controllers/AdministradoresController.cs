using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlyEase_ApiRest_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ReadController<Administrador, int, FlyEaseDataBaseContext>
    {
        private readonly FlyEaseDataBaseContext _context;

        public AdministradoresController(FlyEaseDataBaseContext context): base(context)
        {
            _context = context;
        }



        [HttpGet]
        [Route("GetByDocument/{AdminDocument}")]
        public async Task<IActionResult> GetByDocument(string AdminDocument)
        {
            try
            {
                var Admin = await _context.Administradores
         .FirstOrDefaultAsync(a => a.Numerodocumento == AdminDocument);
                if (Admin == null)
                {
                    return BadRequest("No se ha encontrado");
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = Admin });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}