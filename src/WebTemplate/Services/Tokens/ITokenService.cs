namespace WebTemplate.Services.Tokens;

public interface ITokenService
{
    TokenResult CreateTokenResult(string username,bool rememberMe);
    string? GetRefreshToken();
    void SetRefreshCookie(string refreshToken, string refreshCookiePath);
    void DeleteRefreshToken();
}
