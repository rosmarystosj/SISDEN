using Microsoft.AspNetCore.Mvc;

namespace SISDEN.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
