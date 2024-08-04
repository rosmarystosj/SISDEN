using Microsoft.AspNetCore.SignalR;
using SISDEN.Models;
using Microsoft.EntityFrameworkCore;
using SISDEN.Hubs;


namespace SISDEN.Services
{
    public class NotificationService
    {
        private readonly SisdemContext _context;
        private readonly EmailService _emailService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(SisdemContext context, EmailService emailService, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _emailService = emailService;
            _hubContext = hubContext;
        }

        public async Task CreateNotificationAsync(int userId, int estadoId, string message)
        {
            var notification = new Notificacion
            {
                Idusuario = userId,
                Idestado = estadoId,
                Mensaje = message,
                Fechaenvio = DateTime.Now,
                Leido = false
            };

            _context.Notificacions.Add(notification);
            await _context.SaveChangesAsync();

            var user = await _context.Usuarios.FindAsync(userId);
            if (user != null)
            {
                // Enviar correo electrónico
                await _emailService.SendEmailAsync(user.Usuemail, "Actualización de Estado de Denuncia", message);

                // Enviar notificación en tiempo real
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
            }
        }
    }

}
