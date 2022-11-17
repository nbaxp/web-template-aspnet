using WebTemplate.Application.Entities;

namespace WebTemplate.Application.Services.Users;

public class ValidateUserResult
{
    public ValidateUserStatus Status { get; set; }
    public User? User { get; set; }
}
