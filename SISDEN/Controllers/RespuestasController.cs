using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.DTOS;
using SISDEN.Models;
using SISDEN.Services;
using System.Runtime.InteropServices;

namespace SISDEN.Controllers
{
    public class RespuestasController : Controller
    {
        private readonly SisdemContext _context;
        private readonly IRegistrarDenuncia _registrarDenuncia;

        public RespuestasController(SisdemContext context, IRegistrarDenuncia registrarDenuncia )
        {
            _context = context;
            _registrarDenuncia = registrarDenuncia; 
        }

        [HttpGet("api/ObtenerRespuestas")]
        public async Task<ActionResult<IEnumerable<VistaRespuesta>>> GetRespuestas()
        {

            return await _context.VistaRespuestas.ToListAsync();
        }
        [HttpGet("api/ObtenerRespuestas/{id}")]
        public async Task<ActionResult<VistaRespuesta>> GetRespuestas(int id)
        {
            var respuestas = await _context.VistaRespuestas.FirstOrDefaultAsync(r => r.Idrespuesta == id);
            if (respuestas == null)
            {
                return BadRequest("Respuesta no encontrada");
            }
            return respuestas;
        }

        [HttpPost("api/GuardarRespuesta")]
        public async Task<ActionResult> PostRespuesta([FromBody] RespuestasDTO respuestasDTO, string sesionid)
        {
            if (respuestasDTO == null)
            {
                return BadRequest("Respuesta no valida");
            }
            try
            {
                var respuesta = new Respuestum
                {
                    Respdescripcion = respuestasDTO.Respdescripcion,
                    RespIdopcion = respuestasDTO.RespIdopcion,
                    RespIdpregunta = respuestasDTO.RespIdpregunta,
                    RespIdusuario = respuestasDTO.RespIdusuario,
                };

                _context.Respuesta.Add(respuesta);
                await _context.SaveChangesAsync();

                var violaciones = await _context.Opcionpregunta
                    .Where(o => o.Idopcionpreg == respuesta.RespIdopcion)
                    .Select(o => new 
                    {
                        o.OpcionpregIdarticulo,
                        o.OpcionpregIdpuntoart
                    })
                    .FirstOrDefaultAsync();

                if (violaciones == null)
                {
                    return NotFound();  
                }

                var denunciaid = await _registrarDenuncia.RegistrarDenunciaAsync(sesionid);
                    
                var leyviolacion = new Leyviolacion
                {
                    ViolIdarticulo = violaciones.OpcionpregIdarticulo,
                    ViolIddenuncia = denunciaid,
                    ViolIdpuntoart = violaciones.OpcionpregIdpuntoart

                };
                _context.Leyviolacions.Add(leyviolacion);
                await _context.SaveChangesAsync();

                return Ok(new { id = respuesta.Idrespuesta, respuesta });
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("api/EditarRespuesta/{id}")]
        public async Task<IActionResult> PutRespuesta([FromBody] int id, RespuestasDTO respuestasDTO)
        {
            if (respuestasDTO == null)
            {
                return BadRequest("Respuesta no valida");
            }
            try
            {
                var respuesta = await _context.Respuesta.FindAsync(id);

                if (respuesta == null)
                {
                    return NotFound("Respuesta no encontrada");

                }
                respuesta.Respdescripcion = respuestasDTO.Respdescripcion;
                respuesta.RespIdopcion = respuestasDTO.RespIdopcion;
                respuesta.RespIdpregunta = respuestasDTO.RespIdpregunta;
                respuesta.RespIdusuario = respuestasDTO.RespIdusuario;

               _context.Entry(respuesta).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(respuesta);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (RespuestaExists(id))
                {
                    return NotFound("Respuesta no encontrada");
                }
                else
                {
                    throw;
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
          
            }
        }
        [HttpDelete("api/EliminarRespuesta/{id}")]
        public async Task<IActionResult> DeleteRespuestas(int id)
        {
            var respuesta = await _context.Respuesta.FirstOrDefaultAsync(r => r.Idrespuesta == id);
            if (respuesta == null)
            {
                return NotFound();

            }
            _context.Respuesta.Remove(respuesta);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        private bool RespuestaExists(int id)
        {
            return _context.Respuesta.Any(r => r.Idrespuesta == id);
        }
    }
}
