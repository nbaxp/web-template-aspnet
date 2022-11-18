using System.ComponentModel.DataAnnotations;
using Namotion.Reflection;
using NJsonSchema;
using NJsonSchema.Generation;

namespace WebTemplate.Web.Json;

public class CustomJsonSchemaGenerator : JsonSchemaGenerator
{
    public CustomJsonSchemaGenerator(JsonSchemaGeneratorSettings settings) : base(settings)
    {
    }

    public override void ApplyDataAnnotations(JsonSchema schema, JsonTypeDescription typeDescription)
    {
        base.ApplyDataAnnotations(schema, typeDescription);
        var contextualType = typeDescription.ContextualType;
        if (string.IsNullOrEmpty(schema.Format))
        {
            var attribute = contextualType.ContextAttributes.FirstAssignableToTypeNameOrDefault("System.ComponentModel.DataAnnotations.DataTypeAttribute");
            if (attribute != null)
            {
                var dataTypeAttribute = attribute as DataTypeAttribute;
                if (dataTypeAttribute != null)
                {
                    if (dataTypeAttribute.DataType == DataType.Custom)
                    {
                        schema.Format = dataTypeAttribute?.CustomDataType;
                    }
                    else
                    {
                        schema.Format = dataTypeAttribute.DataType.ToString().ToLowerInvariant();
                    }
                }
            }
        }
        if (string.IsNullOrEmpty(schema.Format))
        {
            var attribute = contextualType.ContextAttributes.FirstAssignableToTypeNameOrDefault("System.ComponentModel.DataAnnotations.UIHintAttribute");
            if (attribute != null)
            {
                var uiHintAttribute = attribute as UIHintAttribute;
                if (uiHintAttribute != null)
                {
                    schema.Format = uiHintAttribute.UIHint;
                }
            }
        }
    }

    protected override void GenerateEnum(JsonSchema schema, JsonTypeDescription typeDescription)
    {
        base.GenerateEnum(schema, typeDescription);
        var type = typeDescription.ContextualType.Type;
    }
}
