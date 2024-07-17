using Microsoft.AspNetCore.Mvc;
using SISDEN.Models;
using Newtonsoft.Json.Linq;
using SISDEN.DTOS;


namespace SISDEN.Controllers
{
    public class UbicacionController : Controller
    {
        private readonly SisdemContext _context;

        public UbicacionController(SisdemContext context)
        {
            _context = context;
            
        }

    }
}
