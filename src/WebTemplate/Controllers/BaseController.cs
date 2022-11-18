using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Controllers;

public class BaseController : Controller
{
    protected IActionResult Result(IActionResult actionResult)
    {
        if (actionResult is OkObjectResult okObjectResult)
        {
            return View(okObjectResult.Value);
        }
        return actionResult;
    }
}
