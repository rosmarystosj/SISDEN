using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;
using System.Threading.Tasks;

namespace SISDEN.Services
{
    public interface IRegistrarDenuncia
    {
        Task<int> RegistrarDenunciaAsync(string sessionId);
    }

    public class RegistroDenunciaService : IRegistrarDenuncia
    {
        private readonly SisdemContext _context;
        private readonly ISesion _sesion;

        public RegistroDenunciaService(SisdemContext context, ISesion sesion)   
        {
            _context = context;
            _sesion = sesion;   
        }

        public async Task<int> RegistrarDenunciaAsync(string sesionId)

        { 

            var denunciaExistente = await _context.Denuncia.FirstOrDefaultAsync(d => d.Densesion == sesionId);

            if (denunciaExistente != null)
            {
                return denunciaExistente.Iddenuncia; 
            }
            var denuncia = new Denuncium
            {
                Densesion = sesionId,
                Denfechacreacion = DateTime.Now,
                DenIdestado = 1
            };
            _context.Denuncia.Add(denuncia);
            await _context.SaveChangesAsync();

            return denuncia.Iddenuncia;
        }
    }
}
