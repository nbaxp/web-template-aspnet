using Microsoft.AspNetCore.Mvc;
using Flurl;

namespace WebTemplate.Controllers;

public class ConnectController : Controller
{
    public IActionResult Authorize(string client_id,string state)
    {
        var client = "";
        var code = "";
        var redirectUrl = "";
        return Redirect(redirectUrl.SetQueryParam("code", code).SetQueryParam("state",state));
    }

    [HttpPost]
    public IActionResult Token(string client_id,string client_secret, string code)
    {
        var client = "";
        var access_token = "";
        return Content($"access_token={access_token}");
    }

    public IActionResult UserInfo(string access_token)
    {
        var user = "";
        return Json(new { 
            id="id",
            email="email"
        });
    }
}
