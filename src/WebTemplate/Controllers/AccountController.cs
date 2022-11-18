using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebTemplate.Application.Entities;
using WebTemplate.Application.Interfaces;
using WebTemplate.Application.Services.Users;
using WebTemplate.Extensions;
using WebTemplate.Models;
using WebTemplate.Services.OAuth;
using WebTemplate.Services.Tokens;
using WebTemplate.Settings;
using WebTemplate.Shared.Extensions;
using WebTemplate.Web.Models;
using WebTemplate.Web.Resources;

namespace WebTemplate.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IStringLocalizer<Resource> _localizer;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly JwtOptions _jwtOptions;
    private readonly OAuthService _oAuthService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserLogin> _userLoginRepository;

    public AccountController(ILogger<AccountController> logger,
        IStringLocalizer<Resource> localizer,
        IUserService userService,
        ITokenService tokenService,
        IOptions<JwtOptions> jwtOptions,
        OAuthService oAuthService,
        IHttpClientFactory httpClientFactory,
        IRepository<User> userRepository,
        IRepository<UserLogin> userLoginRepository)
    {
        this._logger = logger;
        this._localizer = localizer;
        this._userService = userService;
        this._tokenService = tokenService;
        this._jwtOptions = jwtOptions.Value;
        this._oAuthService = oAuthService;
        this._httpClientFactory = httpClientFactory;
        this._userRepository = userRepository;
        this._userLoginRepository = userLoginRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        var model = new LoginModel
        {
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };
        return View(model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = _userService.ValidateUser(model.UserName, model.Password);
            if (result.Status == ValidateUserStatus.Successful)
            {
                CookieLogin(model.UserName, model.RememberMe);
                return Ok();
            }
            ModelState.AddModelError("", _localizer[result.Status.ToString()]);
        }
        return BadRequest(ModelState.ToErrors());
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok();
    }

    [AllowAnonymous]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var url = this._oAuthService.GetAuthorizationUrl(provider);
        return Redirect(url);
    }

    [AllowAnonymous]
    public async Task<IActionResult> OAuthCallback(string id, string code)
    {
        var openId = await _oAuthService.GetOpenId(id, code).ConfigureAwait(false);
        if (User.Identity!.IsAuthenticated)// 已登录增加三方登录
        {
            if (!_userLoginRepository.Set().Any(o => o.LoginProvider == id && o.ProviderKey == openId))// 没有 openid 增加 openid 并关联当前用户
            {
                var user = this._userRepository.Set().First(o => o.UserName == User.Identity.Name);
                user.UserLogins.Add(new UserLogin { LoginProvider = id, ProviderKey = openId! });
                _userRepository.SaveChanges();
            }
        }
        else//未登录确认用户名并登录
        {
            var loginUser = this._userLoginRepository.Set().FirstOrDefault(o => o.LoginProvider == id && o.ProviderKey == openId);
            if (loginUser == null) // 没有 openid 确认登录名
            {
                var model = new ExternalLoginModel { Provider = id, OpenId = openId!, UserName = $"{id}_{openId}" };
                return View(model);
            }
            else
            {
                var user = this._userRepository.Set().Where(o => o.Id == loginUser.UserId).FirstOrDefault();
                CookieLogin(user!.UserName!, false);
            }
        }
        return LocalRedirect(Url.Action("Index", "Account")!);
        //return RedirectToAction("Index", "Account");
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult OAuthCallback([FromBody] ExternalLoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User { UserName = model.UserName, NormalizedUserName = model.UserName.Normalized() };
            user.UserLogins.Add(new UserLogin { LoginProvider = model.Provider, ProviderKey = model.OpenId });
            _userRepository.Add(user);
            _userRepository.SaveChanges();
            CookieLogin(user!.UserName!, false);
            return Ok();
        }
        return BadRequest(ModelState.ToErrors());
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterModel());
    }

    [AllowAnonymous]
    public IActionResult IsUserNameAvailable(string userName)
    {
        var normalizedUserName = userName.Normalized();
        return Json(!this._userRepository.Set().Any(o => o.NormalizedUserName == normalizedUserName));
    }

    //public IActionResult RegisterConfirmation()
    //{
    //}

    private void CookieLogin(string userName, bool rememberMe)
    {
        var token = _tokenService.CreateTokenResult(userName, rememberMe);
        Response.Cookies.Delete("jwt");
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = rememberMe ? DateTimeOffset.Now.Add(_jwtOptions.RefreshTokenExpires) : null
        };
        Response.Cookies.Append("jwt", token.access_token, cookieOptions);
    }
}
