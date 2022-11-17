using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebTemplate.Services.Tokens;

public class TokenResult
{
    public string access_token { get; set; } = null!;
    public string refresh_token { get; set; } = null!;
    public int expires_in { get; set; }
    public string token_type { get; set; } = JwtBearerDefaults.AuthenticationScheme;
}
