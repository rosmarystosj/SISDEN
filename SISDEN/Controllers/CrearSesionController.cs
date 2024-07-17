using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SISDEN.Models;
using SISDEN.Services;
using SISDEN.DTOS;

namespace SISDEN.Controllers
{
    public class CrearSesionController : Controller
    {
        private readonly SisdemContext _context;
        private readonly ISesion _sesion;

        public CrearSesionController(ISesion sesion, SisdemContext context)
        {
             _context = context;
             _sesion = sesion; 
        }

        [HttpPost("api/CrearSesion")]

        public async Task<IActionResult> IniciarChat([FromBody] string userid)
        {
            var sesionid = await _sesion.ObtenerSesionID(userid);

            return Ok(new { sesionid });
        }

    }


}
