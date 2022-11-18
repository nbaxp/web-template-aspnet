using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebTemplate.Extensions;

public static class ModelStateDictionaryExtensions
{
    public static Dictionary<string, string> ToErrors(this ModelStateDictionary modelState)
    {
        return modelState
            .Where(o => o.Value!.Errors.Any())
            .ToDictionary(o => o.Key, o => o.Value!.Errors.First().ErrorMessage);
    }
}
