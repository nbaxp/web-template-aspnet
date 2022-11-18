using Flurl;
using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Controllers;

public class ConnectController : Controller
{
    public IActionResult Authorize(string client_id, string state)
    {
        var code = "";
        var redirectUrl = "";
        return Redirect(redirectUrl.SetQueryParam("code", code).SetQueryParam("state", state));
    }

    [HttpPost]
    public IActionResult Token(string client_id, string client_secret, string code)
    {
        var access_token = "";
        return Content($"access_token={access_token}");
    }

    public IActionResult UserInfo(string access_token)
    {
        return Json(new
        {
            id = "id",
            email = "email"
        });
    }
}
