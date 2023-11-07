using Microsoft.AspNetCore.Mvc;

namespace APIUsuarios.Models
{
    public class InicioDeSesion : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
