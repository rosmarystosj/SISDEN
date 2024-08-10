using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;

namespace SISDEN.Controllers
{
    public class OpcionesPreguntasController : Controller
    {
        private readonly SisdemContext _context;

        public OpcionesPreguntasController(SisdemContext context)
        {
            _context = context;
        }

        [HttpGet("api/ObtenerOpcionesPreguntas/{id}")]
        public async Task<ActionResult<VistaOpcionesPregunta>> GetPregunta(int id)
        {
            var opcionespregunta = await _context.VistaOpcionesPreguntas.FirstOrDefaultAsync(op => op.Idopcionpreg == id);
            if (opcionespregunta == null)
            {
                return BadRequest("Pregunta no encontrada");
            }
            return opcionespregunta;
        }
    }
}
