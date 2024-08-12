using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;

namespace SISDEN.Controllers
{
    public class PreguntasController : Controller
    {
        private readonly SisdemContext _context;

        public PreguntasController(SisdemContext context)
        {
            _context = context;
        }

        [HttpGet("api/ObtenerPreguntas/{id}")]
        public async Task<ActionResult<VistaPregunta>> GetPregunta(int id)
        {
            var pregunta = await _context.VistaPreguntas.FirstOrDefaultAsync(p => p.Idpregunta == id);
            if (pregunta == null)
            {
                return BadRequest("Pregunta no encontrada");
            }
            return pregunta;
        }
        [HttpGet("api/ObtenerPreguntas")]
        public async Task<ActionResult<IEnumerable<VistaPregunta>>> GetOpciones()
        {

            return await _context.VistaPreguntas.ToListAsync();
        }
    }
}
