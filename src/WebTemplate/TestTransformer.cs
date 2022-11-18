using Microsoft.AspNetCore.Mvc.Routing;

public class TestTransformer : DynamicRouteValueTransformer
{
    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        if (httpContext.Request.Path.Value.Substring(httpContext.Request.PathBase.Value.Length) == "/")
        {

        }
        return ValueTask.FromResult(values);
    }
}
