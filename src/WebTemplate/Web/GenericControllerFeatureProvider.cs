using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using WebTemplate.Application.Shared;

namespace WebTemplate.Web.Web;

public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var entityTypeInfos = Assembly.GetAssembly(typeof(BaseEntity))!.GetTypes().Where(o => !o.IsAbstract && o.IsAssignableTo(typeof(BaseEntity))).Select(o => o.GetTypeInfo()).ToList();
        foreach (var entityTypeInfo in entityTypeInfos)
        {
            var typeName = entityTypeInfo.Name + "Controller";
            if (!feature.Controllers.Any(t => t.Name == typeName))
            {
                var entityType = entityTypeInfo.AsType();
                //var apiControllerTypeInfo = typeof(GenericApiController<,,>).MakeGenericType(entityType, entityType, entityType).GetTypeInfo();
                //feature.Controllers.Add(apiControllerTypeInfo);
                var mvcControllerTypeInfo = typeof(GenericMvcController<,,>).MakeGenericType(entityType, entityType, entityType).GetTypeInfo();
                feature.Controllers.Add(mvcControllerTypeInfo);
            }
        }
    }
}
