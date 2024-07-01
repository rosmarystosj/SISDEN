
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.DTOS;
using SISDEN.Models;
using System.Threading.Tasks;

namespace SISDEN.Services
{
    public interface IRegistrarDenuncia
    {
        Task<int>  RegistrarDenunciaAsync(int id);

    }
    public class RegistroDenunciaService : IRegistrarDenuncia
    {
        private readonly SisdemContext _context;

        public RegistroDenunciaService(SisdemContext context)
        {
            _context = context;
        }
        public async Task<int> RegistrarDenunciaAsync(int id)
        {

            var denuncia = new Denuncium
            {
                Iddenuncia = id,
            };
            _context.Denuncia.Add(denuncia);
            await _context.SaveChangesAsync();

            return denuncia.Iddenuncia;
        }
           
    }
 }


