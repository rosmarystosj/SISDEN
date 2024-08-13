using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;
using SISDEN.Services;

namespace SISDEN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly SisdemContext _context;
        private readonly INotificacionService _notificationService;

        public NotificacionController(SisdemContext context, INotificacionService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpPut("api/setreadnotificationv2/{id}")]
        public async Task<IActionResult> MarcarNotificacionesLeidas(int id)
        {
            var notificaciones = await _context.Notificacions
                .Where(n => n.Id == id && n.Leido == 0)
                .ToListAsync();

            if (!notificaciones.Any())
            {
                return NotFound("No se encontraron notificaciones no leídas.");
            }

            notificaciones.ForEach(n => n.Leido = 1);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        [HttpPut("api/setreadnotification/{id}")]
        public async Task<IActionResult> ActualizarNotificacion(int id)
        {
            var notificacion = await _context.Notificacions.FindAsync(id);
            if (notificacion == null)
            {
                return NotFound();
            }

            notificacion.Leido = 1;
            await _context.SaveChangesAsync();

            return Ok(notificacion);
        }

        [HttpGet("api/notificationUnread/{idUsuario}")]
        public async Task<IActionResult> ObtenerNotificacionesNoLeidas(int idUsuario)
        {
            var notificaciones = await _context.Notificacions
                .Where(n => n.Idusuario == idUsuario && n.Leido == 0)
                .ToListAsync();

            if (!notificaciones.Any())
            {
                return NotFound("No hay notificaciones no leídas.");
            }

            return Ok(notificaciones);
        }

        [HttpGet("api/notificationEntidadUnread/{EntidadId}")]
        public async Task<IActionResult> ObtenerNotificacionesEntidadNoLeidas(int EntidadId)
        {
            var notificaciones = await _context.Notificacions
                .Where(n => n.EntidadId == EntidadId && n.Leido == 0)
                .ToListAsync();

            if (!notificaciones.Any())
            {
                return NotFound("No hay notificaciones no leídas.");
            }

            return Ok(notificaciones);
        }
    }

}
