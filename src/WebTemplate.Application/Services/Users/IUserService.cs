namespace WebTemplate.Application.Services.Users;

public interface IUserService
{
    ValidateUserResult ValidateUser(string userName, string password);
}
