using FlyEase_ApiRest_.Contexto;
using FlyEase_ApiRest_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlyEase_ApiRest_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : Controller
    {
        private readonly FlyEaseDataBaseContext _context;

        public ClientesController(FlyEaseDataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetClientes()
        {
            List<Cliente> lista = new();
            try
            {
                lista = _context.Clientes.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, response = lista });
            }
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult InsertarProducto([FromBody] Cliente cliente)
        {
            // Llamar al procedimiento almacenado utilizando Entity Framework
            _context.Database.ExecuteSqlRaw("CALL sp_InsertarProducto({0}, {1}, {2}, {3}, {4}, {5})", cliente.Numerodocumento, cliente.Tipodocumento,
                cliente.Nombres, cliente.Apellidos, cliente.Celular, cliente.Correo);

            return Ok("Producto insertado exitosamente");
        }
    }
}
