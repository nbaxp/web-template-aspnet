using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Admin.Controllers
{
    public class TanentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
