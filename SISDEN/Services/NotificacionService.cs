using Microsoft.AspNetCore.SignalR;
using SISDEN.DTOS;
using SISDEN.Hubs;
using SISDEN.Models;

namespace SISDEN.Services
{
    public interface INotificacionService
    {
        Task CreateNotificationAsync(NotificationDto notificationDto);
    }

    public class NotificacionService : INotificacionService
    {
        private readonly SisdemContext _context;
        private readonly IServicioEmail _emailService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificacionService(SisdemContext context, IServicioEmail emailService, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _emailService = emailService;
            _hubContext = hubContext;
        }

        public async Task CreateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = new Notificacion
            {
                Idusuario = notificationDto.Idusuario,
                EntidadId = notificationDto.EntidadId,
                Idestado = notificationDto.Idestado,
                Mensaje = notificationDto.Mensaje,
                Fechaenvio = DateTime.Now,
                Leido = 0
            };

            _context.Notificacions.Add(notification);
            await _context.SaveChangesAsync();

            var user = await _context.Usuarios.FindAsync(notificationDto.Idusuario);
            if (user != null)
            {
                // Enviar correo electrónico
                await _emailService.SendEmailAsync(user.Usuemail, "Actualización de Estado de Denuncia", notificationDto.Mensaje);

                // Enviar notificación en tiempo real
                await _hubContext.Clients.User(notificationDto.Idusuario.ToString()).SendAsync("ReceiveNotification", notificationDto.Mensaje);
            }
        }
    }

}
