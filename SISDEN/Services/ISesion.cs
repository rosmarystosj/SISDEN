using Microsoft.EntityFrameworkCore;
using SISDEN.Models;
using System.Runtime;

namespace SISDEN.Services
{
    public interface ISesion 
    {
        Task<string> ObtenerSesionID(string Userid);
        Task ExtenderSesionAsync(string sessionId);
        Task<string> ObtenerUserIdAsync(string sessionId);
    }

    public class  ObtenerSesionIdService : ISesion
    {
        private readonly SisdemContext _context;

        public ObtenerSesionIdService(SisdemContext context)
        {
            _context = context; 
        }

        public  async Task<string> ObtenerSesionID(string Userid)
        {

            var sesion = new Sesion
            {
                Sesionid = Guid.NewGuid().ToString(),
                Userid = Userid,
                Createat = DateTime.Now,
                Expiresat = DateTime.UtcNow.AddMinutes(30)

            };
            _context.Sesions.Add(sesion);   
            await _context.SaveChangesAsync(); 
            
            return sesion.Sesionid;

        }
        public async Task<string> ObtenerUserIdAsync(string sessionId)
        {
           
            var session = await _context.Sesions.FirstOrDefaultAsync(s => s.Sesionid == sessionId && s.Expiresat > DateTime.UtcNow);

            return session?.Userid;
        }

        public async Task ExtenderSesionAsync(string sessionId)
        {
            var session = await _context.Sesions.FirstOrDefaultAsync(s => s.Sesionid == sessionId);

            if (session != null)
            {
                session.Expiresat = DateTime.UtcNow.AddMinutes(30);
                await _context.SaveChangesAsync();
            }
        }
    } 
}
