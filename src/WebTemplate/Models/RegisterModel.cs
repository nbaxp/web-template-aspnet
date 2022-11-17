using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebTemplate.Models;

[Display]
public class RegisterModel
{
    [Display(Description = "UserNameDescription")]
    [StringLength(20, MinimumLength = 4)]
    [Required]
    [Remote("IsUserNameAvailable", "Account")]
    public string? UserName { get; set; }

    [Display]
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display]
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Display]
    [Required]
    public string? VerificationCode { get; set; }
}
