using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;
using SISDEN.DTOS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SISDEN.Services;

namespace SISDEN.Controllers
{
    public class LeyviolacionController : ControllerBase
    {
        private readonly SisdemContext _context;
        private readonly IServicioEmail _emailService;

        public LeyviolacionController(SisdemContext context, IServicioEmail emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeyviolacionDTO>>> GetLeyviolaciones()
        {
            return await _context.Leyviolacions
                .Select(v => new LeyviolacionDTO
                {
                    Idviolacion = v.Idviolacion,
                    ViolIdpuntoart = v.ViolIdpuntoart,
                    ViolIdarticulo = v.ViolIdarticulo,
                    ViolIddenuncia = v.ViolIddenuncia
                }).ToListAsync();
        }

        [HttpGet("api/Leyviolacion/{id}")]
        public async Task<ActionResult<VistaViolacione>> GetLeyviolacion(int id)
        {
            var leyviolacion = await _context.VistaViolaciones
                .Select(v => new VistaViolacione
                {
                    Idviolacion = v.Idviolacion,
                    Artnombre = v.Artnombre,
                    Artdescripcion = v.Artdescripcion,
                    Puntoartnumero = v.Puntoartnumero,
                    Puntoartdescripcion = v.Puntoartdescripcion,
                    Iddenuncia = v.Iddenuncia,
                    Dentitulo = v.Dentitulo
                }).FirstOrDefaultAsync(v => v.Idviolacion == id);

            if (leyviolacion == null)
            {
                return NotFound();
            }

            return leyviolacion;
        }

        [HttpGet("api/ObtenerViolacionesPorDenuncia/{denunciaid}")]
        public async Task<ActionResult<IEnumerable<VistaViolacione>>> GetViolacionesPorDenuncia(int denunciaid)
        {
            var leyviolacion = await _context.VistaViolaciones
                .Where(v => v.Iddenuncia == denunciaid)
                  .ToListAsync();

            if (leyviolacion == null)
            {
                return BadRequest("Violacion no encontrada");
            }
            return Ok(leyviolacion);

        }

        [HttpPost("api/Leyviolacion")]
        public async Task<ActionResult<LeyviolacionDTO>> PostLeyviolacion([FromBody] LeyviolacionDTO leyviolacionDto)
        {
            var leyviolacion = new Leyviolacion
            {
                ViolIdpuntoart = leyviolacionDto.ViolIdpuntoart,
                ViolIdarticulo = leyviolacionDto.ViolIdarticulo,
                ViolIddenuncia = leyviolacionDto.ViolIddenuncia
            };

            _context.Leyviolacions.Add(leyviolacion);
            await _context.SaveChangesAsync();

            leyviolacionDto.Idviolacion = leyviolacion.Idviolacion;

            return CreatedAtAction(nameof(GetLeyviolacion), new { id = leyviolacion.Idviolacion }, leyviolacionDto);
        }

        [HttpPut("api/Leyviolacion/{id}")]
        public async Task<IActionResult> PutLeyviolacion([FromBody] int id, LeyviolacionDTO leyviolacionDto)
        {
            if (id != leyviolacionDto.Idviolacion)
            {
                return BadRequest();
            }

            var leyviolacion = await _context.Leyviolacions.FindAsync(id);

            if (leyviolacion == null)
            {
                return NotFound();
            }

            leyviolacion.ViolIdpuntoart = leyviolacionDto.ViolIdpuntoart;
            leyviolacion.ViolIdarticulo = leyviolacionDto.ViolIdarticulo;
            leyviolacion.ViolIddenuncia = leyviolacionDto.ViolIddenuncia;

            _context.Entry(leyviolacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeyviolacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("api/Leyviolacion/{id}")]
        public async Task<IActionResult> DeleteLeyviolacion(int id)
        {
            var leyviolacion = await _context.Leyviolacions.FindAsync(id);
            if (leyviolacion == null)
            {
                return NotFound();
            }

            _context.Leyviolacions.Remove(leyviolacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeyviolacionExists(int id)
        {
            return _context.Leyviolacions.Any(e => e.Idviolacion == id);
        }
    }
}
