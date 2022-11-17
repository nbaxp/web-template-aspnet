using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;
using WebTemplate.Web.Resources;

namespace WebTemplate.Web;

public abstract class CustomRazorPage<TModel> : RazorPage<TModel>
{
    private IStringLocalizer<Resource>? _stringLocalizer;

    public LocalizedString T(string name, params string[] args)
    {
        if (_stringLocalizer == null)
        {
            _stringLocalizer = this.Context.RequestServices.GetRequiredService<IStringLocalizer<Resource>>()!;
        }
        if (args == null || args.Length == 0)
        {
            return _stringLocalizer[name];
        }

        return new LocalizedString(name, string.Format(_stringLocalizer[name], args));
    }
}
