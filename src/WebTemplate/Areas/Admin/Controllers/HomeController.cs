using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
