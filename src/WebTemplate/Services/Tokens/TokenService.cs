using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebTemplate.Settings;

namespace WebTemplate.Services.Tokens;

public class TokenService : ITokenService
{
    private readonly string TOKEN_COOKIE_NAME = "refresh_token";
    private readonly JwtOptions _jwtOptions;
    private readonly JwtBearerOptions _options;
    private readonly SigningCredentials _credentials;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(
            IOptions<JwtOptions> jwtOptions,
            IOptionsSnapshot<JwtBearerOptions> options,
            SigningCredentials credentials,
            IHttpContextAccessor httpContextAccessor)
    {
        _jwtOptions = jwtOptions.Value;
        _options = options.Get(JwtBearerDefaults.AuthenticationScheme);
        _credentials = credentials;
        this._httpContextAccessor = httpContextAccessor;
    }

    public TokenResult CreateTokenResult(string username, bool rememberMe)
    {
        var claims = new Claim[] {
            new Claim(nameof(ClaimTypes.IsPersistent),rememberMe.ToString()),
            new Claim(_options.TokenValidationParameters.NameClaimType, username),
            new Claim("NickName","管理員")
        };
        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var now = DateTime.UtcNow;
        var accessToken = CreateToken(claimsIdentity, now, _jwtOptions.AccessTokenExpires);
        var refreshToken = CreateToken(claimsIdentity, now, _jwtOptions.RefreshTokenExpires);

        return new TokenResult
        {
            access_token = accessToken,
            refresh_token = refreshToken,
            expires_in = (int)_jwtOptions.AccessTokenExpires.TotalSeconds,
            token_type = JwtBearerDefaults.AuthenticationScheme,
        };
    }

    public string? GetRefreshToken()
    {
        return _httpContextAccessor.HttpContext?.Request.Cookies[TOKEN_COOKIE_NAME];
    }

    public void SetRefreshCookie(string refreshToken, string refreshCookiePath)
    {
        this.DeleteRefreshToken();
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(TOKEN_COOKIE_NAME, refreshToken, GetCookieOptions(refreshCookiePath));
    }

    public void DeleteRefreshToken()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(TOKEN_COOKIE_NAME);
    }

    private string CreateToken(ClaimsIdentity subject, DateTime now, TimeSpan timeout)
    {
        var handler = _options.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().First();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = _options.TokenValidationParameters.ValidIssuer,
            Audience = _options.TokenValidationParameters.ValidAudience,
            SigningCredentials = _credentials,
            Subject = subject,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.Add(timeout),
        };
        var securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
        var token = handler.WriteToken(securityToken);
        return token;
    }

    private CookieOptions GetCookieOptions(string cookiePath)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Path = cookiePath,
            Expires = DateTimeOffset.Now.Add(_jwtOptions.RefreshTokenExpires)
        };
    }
}
