using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlyEase_ApiRest_.Controllers
{
    [EnableCors("Reglas")]
    [Route("FlyEaseApi/[controller]")]
    [ApiController]
    public class PublicTokenController : Controller
    {
        private readonly IAuthentication _aut;
        public PublicTokenController(IAuthentication aut)
        {
            _aut = aut;
        }
        [HttpGet]
        [Route("GenerateCommonToken")]
        public async Task<IActionResult> GenerateCommonToken()
        {
            try
            {
                var Aut = await _aut.Authentication();
                if (!Aut.Succes)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { response = Aut });

                }
                return StatusCode(StatusCodes.Status200OK, new { response = Aut });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
