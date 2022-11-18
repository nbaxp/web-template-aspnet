using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Web.Models;

[Display(Name = "Login")]
public class LoginModel
{
    [StringLength(20, MinimumLength = 5)]
    [Description]
    [Required]
    [Display]
    public string UserName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display]
    public string Password { get; set; } = null!;

    [Display]
    public bool RememberMe { get; set; }

    [HiddenInput(DisplayValue = false)]
    public string? ReturnUrl { get; set; } = null!;
}
