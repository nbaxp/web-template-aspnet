using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Controllers;

[Area("Area")]
public class AreaTestController : Controller
{
    public IActionResult Index()
    {
        return Content(Url.Action()!);
    }
}
