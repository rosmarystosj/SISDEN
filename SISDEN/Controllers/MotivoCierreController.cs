using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;

namespace SISDEN.Controllers
{
    public class MotivoCierreController : Controller
    {
        private readonly SisdemContext _context;

        public MotivoCierreController(SisdemContext context)
        {
            _context = context;
        }

        [HttpGet("api/ObtenerMotivoCierre")]
        public async Task<ActionResult<IEnumerable<Motivocierre>>> GetMotivoCierre()
        {

            return await _context.Motivocierres.ToListAsync();  
        }
        
    }
}
