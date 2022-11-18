using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using WebTemplate.Web.Resources;

namespace Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelperExtensions
{
    public static IHtmlContent DescriptionFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
    {
        var serviceProvider = htmlHelper.ViewContext.HttpContext.RequestServices;
        var metadata = serviceProvider.GetRequiredService<ModelExpressionProvider>().CreateModelExpression(htmlHelper.ViewData, expression).Metadata as DefaultModelMetadata;
        var factory = serviceProvider.GetRequiredService<IStringLocalizerFactory>();
        var localizer = factory.Create(typeof(Resource));
        var descriptionAttribute = metadata!.Attributes.Attributes.First(o => o.GetType() == typeof(DescriptionAttribute)) as DescriptionAttribute;
        if (descriptionAttribute == null)
        {
            return new HtmlString(string.Empty);
        }
        var description = string.IsNullOrEmpty(descriptionAttribute.Description) ? $"{metadata.Name}_Description" : descriptionAttribute.Description;
        return new HtmlString(localizer[description]);
    }
}
