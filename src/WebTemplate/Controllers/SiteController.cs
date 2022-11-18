using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebTemplate.Controllers;

[Route("[controller]/[action]")]
[Route("{culture}/[controller]/[action]")]
[ApiController]
public class SiteController : Controller
{
    private readonly IOptions<RequestLocalizationOptions> _options;

    public SiteController(IOptions<RequestLocalizationOptions> options)
    {
        this._options = options;
    }

    [HttpGet]
    public IActionResult Summary()
    {
        var cultrurName = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;

        return Ok(new
        {
            title = "Html Title",
            name = "网站名称",
            logo = "logo.svg",
            copyright = $"© {DateTime.Now.Year} 版权示例"
        });
    }

    [HttpGet]
    public IActionResult locale()
    {
        var cultrurName = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
        var cultureItems = _options.Value.SupportedUICultures!
            .Select(c => new { c.Name, c.NativeName, Selected = CultureInfo.CurrentUICulture.Name == c.Name })
            .ToList();
        return Ok(new
        {
            @default = HttpContext.RequestServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value.DefaultRequestCulture.Culture.Name,
            current = CultureInfo.CurrentUICulture.Name,
            items = cultureItems
        });
    }
}
