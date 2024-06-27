using Microsoft.AspNetCore.Mvc;
using SISDEN.Models;
using System.Linq;


namespace SISDEN.Controllers
{
    public class VistaArticuloController : Controller

    {
        private readonly SisdemContext _context;

        public VistaArticuloController(SisdemContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
       

            [HttpGet("{id}")]
            public ActionResult<VistaArticulo> GetVistaArticulo(int id)
            {
                var vistaarticulo = _context.VistaArticulos.FirstOrDefault(va => va.Idarticulo == id);
                if ( vistaarticulo == null)
                {
                    return NotFound();
                }

            //return Ok(new {nombreArticulo = vistaarticulo.Artnombre});
            return vistaarticulo;

         
        }
        
    }

}
