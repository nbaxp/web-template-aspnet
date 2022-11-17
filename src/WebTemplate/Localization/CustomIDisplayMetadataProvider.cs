using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace WebTemplate.Localization;

public class CustomIDisplayMetadataProvider : IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var attributes = context.Attributes;
        var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
        if (displayAttribute != null && string.IsNullOrEmpty(displayAttribute.Name))
        {
            displayAttribute.Name = context.Key.Name;
        }

        foreach (var item in attributes)
        {
            if (item is ValidationAttribute attribute)
            {
                
                if (attribute is DataTypeAttribute data)
                {
                    attribute.ErrorMessage = $"DataTypeAttribute_{data.GetDataTypeName()}";
                }
                else
                {
                    if (attribute.ErrorMessage == null)
                    {
                        attribute.ErrorMessage = attribute.GetType().Name;
                        if (item is StringLengthAttribute stringLengthAttribute)
                        {
                            if (stringLengthAttribute.MinimumLength != 0)
                            {
                                attribute.ErrorMessage += "IncludingMinimum";
                            }
                        }
                    }
                }
            }
        }
    }
}
