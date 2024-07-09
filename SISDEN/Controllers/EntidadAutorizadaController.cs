using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISDEN.DTOS;
using SISDEN.Models;
using SISDEN.Services;
using Microsoft.EntityFrameworkCore;


namespace SISDEN.Controllers
{
   
    public class EntidadAutorizadaController : ControllerBase
    {
        private readonly SisdemContext _context;
        private readonly IServicioEmail _emailService;

        public EntidadAutorizadaController(SisdemContext context, IServicioEmail emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        [HttpGet("api/EntidadAutorizada")]
        public async Task<ActionResult<IEnumerable<EntidadAutorizadaDTO>>> GetEntidadesAutorizadas()
        {
            return await _context.Entidadautorizada
                .Select(e => new EntidadAutorizadaDTO
                {
                    Identidadaut = e.Identidadaut,
                    Entautorizadadescp = e.Entautorizadadescp,
                    EntautorizadaEstatus = e.EntautorizadaEstatus
                }).ToListAsync();
        }

        
        [HttpGet("api/EntidadAutorizada/id")]
        public async Task<ActionResult<EntidadAutorizadaDTO>> GetEntidadAutorizada(int id)
        {
            var entidadAutorizada = await _context.Entidadautorizada
                .Select(e => new EntidadAutorizadaDTO
                {
                    Identidadaut = e.Identidadaut,
                    Entautorizadadescp = e.Entautorizadadescp,
                    EntautorizadaEstatus = e.EntautorizadaEstatus
                }).FirstOrDefaultAsync(e => e.Identidadaut == id);

            if (entidadAutorizada == null)
            {
                return NotFound();
            }

            return entidadAutorizada;
        }
    }
}
