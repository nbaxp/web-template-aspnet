using Microsoft.Extensions.Options;
using WebTemplate.Application.Entities;
using WebTemplate.Application.interfaces;
using WebTemplate.Application.Interfaces;

namespace WebTemplate.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IdentityOptions _identityOptions;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IRepository<User> userRepository, IPasswordHasher passwordHasher, IOptions<IdentityOptions> options)
    {
        this._userRepository = userRepository;
        this._identityOptions = options.Value;
        this._passwordHasher = passwordHasher;
    }

    public ValidateUserResult ValidateUser(string userName, string password)
    {
        var result = new ValidateUserResult();
        var user = _userRepository.Set().FirstOrDefault(o => o.UserName == userName);
        if (user != null)
        {
            result.User = user;
            if (SupportsUserLockout(user))
            {
                if (user.LockoutEnd.HasValue)
                {
                    if (user.LockoutEnd.Value >= DateTimeOffset.UtcNow)
                    {
                        result.Status = ValidateUserStatus.LockedOut;
                    }
                    else
                    {
                        user.AccessFailedCount = 0;
                        user.LockoutEnd = null;
                        UpdateUser(user);
                    }
                }
            }
            if (user.PasswordHash == _passwordHasher.HashPassword(password, user.SecurityStamp!))
            {
                result.Status = ValidateUserStatus.Successful;
            }
            else
            {
                result.Status = ValidateUserStatus.WrongPassword;
                if (SupportsUserLockout(user))
                {
                    if (user.AccessFailedCount + 1 < _identityOptions.MaxFailedAccessAttempts)
                    {
                        user.AccessFailedCount += 1;
                    }
                    else
                    {
                        user.LockoutEnd = DateTimeOffset.UtcNow.Add(_identityOptions.DefaultLockoutTimeSpan);
                        user.AccessFailedCount = 0;
                        result.Status = ValidateUserStatus.LockedOut;
                    }
                    UpdateUser(user);
                }
            }
        }
        else
        {
            result.Status = ValidateUserStatus.NotExist;
        }
        return result;
    }

    private bool SupportsUserLockout(User user)
    {
        return _identityOptions.SupportsUserLockout && user.LockoutEnabled;
    }

    private void UpdateUser(User user)
    {
        this._userRepository.SaveChanges();
    }
}
