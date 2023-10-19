using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
namespace FlyEase_ApiRest_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdministradoresController : Controller
    {
        private readonly FlyEaseDataBaseContext _context;

        public AdministradoresController(FlyEaseDataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetAdministrador()
        {
            List < Administrador > lista = new();
            try
            {
                lista = _context.Administradores.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = lista });
            }

        }
    }
}
