namespace WebTemplate.Application.interfaces;

public interface IPasswordHasher
{
    string CreateSalt();

    string HashPassword(string password, string salt);
}
