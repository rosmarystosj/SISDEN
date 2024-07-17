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
    public class EstadoController : ControllerBase
    {
        private readonly SisdemContext _context;
        private readonly IServicioEmail _emailService;

        public EstadoController(SisdemContext context, IServicioEmail emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet("api/Estado")]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstado()
        {
            return await _context.Estados
                .Select(e => new Estado
                {
                    Idestado = e.Idestado,
                    Estdescripcion = e.Estdescripcion,
                    
                }).ToListAsync();
        }


        [HttpGet("api/Estado/id")]
        public async Task<ActionResult<EstadoDTO>> GetEstadoid(int id)
        {
            var estado = await _context.Estados
                .Select(e => new EstadoDTO
                {
                    Idestado = e.Idestado,
                    Estdescripcion = e.Estdescripcion
                }).FirstOrDefaultAsync(e => e.Idestado == id);

            if (estado == null)
            {
                return NotFound();
            }

            return estado;
        }

       
        [HttpPost("api/Estado")]
        public async Task<ActionResult<EstadoDTO>> PostEstado([FromBody] EstadoDTO estadoDto)
        {
            var estado = new Estado
            {
                Idestado = estadoDto.Idestado,
                Estdescripcion = estadoDto.Estdescripcion
            };

            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();

            estadoDto.Idestado = estado.Idestado;

            return CreatedAtAction(nameof(GetEstado), new { id = estado.Idestado }, estadoDto);
        }

        
        [HttpPut("api/Estado/id")]
        public async Task<IActionResult> PutEstado(int id, EstadoDTO estadoDto)
        {
            if (id != estadoDto.Idestado)
            {
                return BadRequest();
            }

            var estado = await _context.Estados.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            estado.Estdescripcion = estadoDto.Estdescripcion;

            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
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

        [HttpDelete("api/Estado/id")]
        public async Task<IActionResult> DeleteEstado(int id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoExists(int id)
        {
            return _context.Estados.Any(e => e.Idestado == id);
        }
    }
}
