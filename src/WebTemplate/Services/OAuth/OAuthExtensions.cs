using System.Dynamic;
using System.Text.Json;
using Flurl;

namespace WebTemplate.Services.OAuth;

public static class OAuthExtensions
{
    public static Dictionary<string, string> QueryStringToDictionary(this string queryString)
    {
        if (string.IsNullOrWhiteSpace(queryString))
        {
            return new Dictionary<string, string>();
        }
        return queryString
            .Split('&')
            .ToDictionary(o => o.Split('=')[0].Trim(), o => o.Split('=')[1].Trim());
    }

    public static object DictionaryToObject(this Dictionary<string, string> dict)
    {
        var eo = new ExpandoObject();
        var eoDict = (ICollection<KeyValuePair<string, object>>)eo!;

        foreach (var item in dict)
        {
            eoDict.Add(new KeyValuePair<string, object>(item.Key, item.Value));
        }

        dynamic eoDynamic = eo;
        return eoDynamic;
        //return eoDynamic.Property;
    }

    public static Dictionary<string, string> JsonTextToDictionary(this string value)
    {
        return JsonSerializer.Deserialize<Dictionary<string, object>>(value)!.ToDictionary(o => o.Key, o => o.Value?.ToString() ?? "");
    }

    public static Url SetQueryParamIf(this Url url, bool expression, string name, string? value)
    {
        return expression ? url.SetQueryParam(name, value) : url;
    }
}
