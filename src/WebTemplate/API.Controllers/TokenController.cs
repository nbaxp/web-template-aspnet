using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using WebTemplate.Application.Services.Users;
using WebTemplate.Services.Tokens;
using WebTemplate.Web.Models;
using WebTemplate.Web.Resources;

namespace WebTemplate.API.Controllers;

[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IStringLocalizer<Resource> _localizer;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IUserService _userService;

    public TokenController(
            ITokenService tokenService,
            IStringLocalizer<Resource> localizer,
            JwtSecurityTokenHandler jwtSecurityTokenHandler,
            TokenValidationParameters tokenValidationParameters,
            IUserService userService)
    {
        _tokenService = tokenService;
        _localizer = localizer;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        _tokenValidationParameters = tokenValidationParameters;
        _userService = userService;
    }

    [ResponseCache(NoStore = true)]
    [HttpPost]
    public IActionResult Token([FromBody] LoginModel model)
    {
        var test = System.Globalization.CultureInfo.CurrentCulture.Name;
        if (ModelState.IsValid)
        {
            var result = _userService.ValidateUser(model.UserName, model.Password);
            if (result.Status == ValidateUserStatus.Successful)
            {
                var token = _tokenService.CreateTokenResult(model.UserName, model.RememberMe);
                _tokenService.SetRefreshCookie(token.refresh_token, Url.Action(nameof(RefreshToken))!);
                return this.Ok(token);
            }
            ModelState.AddModelError("", _localizer[result.Status.ToString()]);
        }
        return Problem();
    }

    [ResponseCache(NoStore = true)]
    [HttpPost("refresh")]
    public IActionResult RefreshToken()
    {
        var refresh_token = _tokenService.GetRefreshToken();
        var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(refresh_token, _tokenValidationParameters, out _);
        var userName = claimsPrincipal?.Identity?.Name!;
        var isPersistent = claimsPrincipal!.Claims.First(o => o.Type == nameof(ClaimTypes.IsPersistent)).Value == true.ToString();
        var result = _tokenService.CreateTokenResult(userName, isPersistent);
        _tokenService.SetRefreshCookie(result.refresh_token, Url.Action(nameof(RefreshToken))!);
        return this.Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _tokenService.DeleteRefreshToken();
        return Ok();
    }
}
