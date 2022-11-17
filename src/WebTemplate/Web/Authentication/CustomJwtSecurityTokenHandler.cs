using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebTemplate.Authentication;

public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
{
    private readonly IServiceProvider _serviceProvider;

    public CustomJwtSecurityTokenHandler(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        var claimsPrincipal = base.ValidateToken(token, validationParameters, out validatedToken);
        using var scope = this._serviceProvider.CreateScope();
        var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
        return new CustomClaimsPrincipal(roleService, claimsPrincipal);
    }
}
