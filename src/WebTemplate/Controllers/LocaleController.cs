using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebTemplate.Web.Resources;

namespace WebTemplate.Controllers;

public class LocaleController : Controller
{
    private readonly RequestLocalizationOptions _requestLocalizationOptions;
    private readonly IStringLocalizer<Resource> _localizer;

    public LocaleController(IOptions<RequestLocalizationOptions> requestLocalizationOptions, IStringLocalizer<Resource> localizer)
    {
        this._requestLocalizationOptions = requestLocalizationOptions.Value;
        this._localizer = localizer;
    }

    public IActionResult Index(string language, string target)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
        return Redirect(target);
    }

    public IActionResult List()
    {
        var options = this._requestLocalizationOptions.SupportedUICultures!
            .Select(o => new { Value = o.Name, Label = o.NativeName })
            .ToList();
        return Json(new
        {
            current = CultureInfo.CurrentCulture.Name,
            options = options,
        }); ;
    }

    public IActionResult Resources(string? id)
    {
        var culture = id == null ? CultureInfo.CurrentCulture.Name : id;
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        return Json(_localizer.GetAllStrings().ToDictionary(o => o.Name, o => o.Value));
    }
}
