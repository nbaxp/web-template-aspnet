using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace WebTemplate.Web.Web;

[Route("[controller]/[action]")]
[ApiController]
public class FeaturesController : Controller
{
    private readonly ApplicationPartManager _partManager;

    public FeaturesController(ApplicationPartManager partManager)
    {
        _partManager = partManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var controllerFeature = new ControllerFeature();
        _partManager.PopulateFeature(controllerFeature);

        var tagHelperFeature = new TagHelperFeature();
        _partManager.PopulateFeature(tagHelperFeature);

        var viewComponentFeature = new ViewComponentFeature();
        _partManager.PopulateFeature(viewComponentFeature);

        return Json(new
        {
            Controllers = controllerFeature.Controllers.Select(o => new
            {
                o.FullName,
                CustomAttributes = o.CustomAttributes.Select(o => o.AttributeType.Name),
                DeclaredMethods = o.DeclaredMethods.Select(o => new
                {
                    o.Name,
                    o.IsPublic,
                    ReturnType = o.ReturnType.FullName,
                    CustomAttributes = o.CustomAttributes.Select(o => o.AttributeType.Name),
                }),
                GenericTypeArguments = o.GenericTypeArguments.Select(o => new
                {
                    o.FullName,
                    PropertyInfos = o.GetProperties().Select(o => new
                    {
                        o.Name,
                        o.PropertyType.FullName,
                        CustomAttributes = o.CustomAttributes.Select(o => o.AttributeType.Name),
                    }),
                    CustomAttributes = o.CustomAttributes.Select(o => o.AttributeType.Name),
                }),
            }).ToList(),
            TagHelpers = tagHelperFeature.TagHelpers.Select(o => o.Name).ToList(),
            ViewComponents = viewComponentFeature.ViewComponents.Select(o => o.Name).ToList()
        });
    }
}
