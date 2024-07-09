using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;
using SISDEN.Services;
using SISDEN.DTOS;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISDEN.Controllers
{
    
    public class ComentarioController : ControllerBase
    {
        private readonly SisdemContext _context;
        private readonly IServicioEmail _servicioEmail;

        public ComentarioController(SisdemContext context, IServicioEmail servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }

        [HttpPost("api/Comentario")]
        public async Task<ActionResult<ComentarioDTO>> PostComentario(ComentarioDTO comentarioDto)
        {
            var comentario = new Comentario
            {
                
                Comdescripcion = comentarioDto.Comdescripcion,
                ComIdusuario = comentarioDto.ComIdusuario,
                ComIddenuncia = comentarioDto.ComIddenuncia,
                ComIdrol = comentarioDto.ComIdrol,
            };

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            var Email = await _context.Usuarios.Where(u=> u.Idusuario== comentario.ComIdusuario).Select(u=> u.Usuemail).FirstOrDefaultAsync();

            var mensaje = $"<p>Se ha creado un nuevo comentario.</p>" + $"<p>Contenido del comentario:</p>" + $"<p>'{comentario.Comdescripcion}'</p>";

            await _servicioEmail.SendEmailAsync(Email, "Nuevo comentario creado", mensaje);

            comentarioDto.Idcomentario = comentario.Idcomentario;

            return CreatedAtAction(nameof(GetComentario), new { id = comentario.Idcomentario }, comentarioDto);
        }


        [HttpGet("api/Comentario/id")]
        public async Task<ActionResult<ComentarioDTO>> GetComentario(int id)
        {
            var comentario = await _context.Comentarios
                .Select(c => new ComentarioDTO
                {
                    
                    Comdescripcion = c.Comdescripcion,
                    ComIdusuario = c.ComIdusuario,
                    ComIddenuncia = c.ComIddenuncia,
                    ComIdrol = c.ComIdrol,
                }).FirstOrDefaultAsync(c => c.Idcomentario == id);

            if (comentario == null)
            {
                return NotFound();
            }

            return comentario;
        }

        [HttpGet("api/Comertariopordenuncia/Denunciaid")]
        public async Task<ActionResult<IEnumerable<VistaComentario>>> GetComentarioPorDenuncia(int Denunciaid)
        {
            var comentarios = await _context.VistaComentarios
        .Where(c => c.Iddenuncia == Denunciaid)
        .Select(c => new VistaComentario
        {
            Comdescripcion = c.Comdescripcion,
            UsuarioNombre = c.UsuarioNombre,
            Iddenuncia = c.Iddenuncia,
            Rolnombre = c.Rolnombre,
            DenunciaTitulo = c.DenunciaTitulo
        }).ToListAsync();

            if (comentarios == null || comentarios.Count == 0)
            {
                return NotFound();
            }

            return comentarios;
        }

        [HttpPut("api/Comentario/id")]
        public async Task<IActionResult> PutComentario(int id, ComentarioDTO comentarioDto)
        {
            if (id != comentarioDto.Idcomentario)
            {
                return BadRequest();
            }

            var comentario = await _context.Comentarios.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            comentario.Comdescripcion = comentarioDto.Comdescripcion;
            comentario.ComIdusuario = comentarioDto.ComIdusuario;
            comentario.ComIddenuncia = comentarioDto.ComIddenuncia;
            comentario.ComIdrol = comentarioDto.ComIdrol;

            _context.Entry(comentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(id))
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

        
        [HttpDelete("api/Comentario/id")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(e => e.Idcomentario == id);
        }
    }
}
