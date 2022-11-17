using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Models;

public class ExternalLoginModel
{
    [HiddenInput(DisplayValue =false)]
    public string Provider { get; set; } = null!;
    [HiddenInput(DisplayValue = false)]
    public string OpenId { get; set; } = null!;
    [Display]
    public string UserName { get; set; } = null!;
}
