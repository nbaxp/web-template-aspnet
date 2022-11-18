using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace WebTemplate.Services.OAuth;

public class OAuthService
{
    private const string CLIENT_ID_NAME = "client_id";
    private const string CLIENT_SECRET_NAME = "client_secret";
    private const string REDIRECT_URI = "redirect_uri";
    private const string ACCESS_TOKEN_NAME = "access_token";
    private readonly ILogger<OAuthService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OAuthOptions Options { get; private set; }

    public OAuthService(ILogger<OAuthService> logger, IHttpContextAccessor httpContextAccessor, IOptions<OAuthOptions> options)
    {
        this._logger = logger;
        this._httpContextAccessor = httpContextAccessor;
        this.Options = options.Value;
    }

    public string GetAuthorizationUrl(string provider)
    {
        var options = this.Options.Providers.First(o => o.Name == provider);
        var url = options.AuthorizationEndpoint
            .SetQueryParam(options.ClientIdName ?? CLIENT_ID_NAME, options.ClientId)
            .SetQueryParamIf(options.AuthorizationEndpoint.Contains(REDIRECT_URI), REDIRECT_URI, UrlContent(options.CallbackPath));
        return url;
    }

    public async Task<Dictionary<string, string>> GetUserInfo(string provider, string code)
    {
        var options = this.Options.Providers.First(o => o.Name == provider);
        var tokenResult = await this.GetToken(options, code);
        var userInfoResult = await this.GetUserInfoInternal(options, tokenResult[ACCESS_TOKEN_NAME]);
        return userInfoResult;
    }

    public async Task<string?> GetOpenId(string provider, string code)
    {
        var options = this.Options.Providers.First(o => o.Name == provider);
        var tokenResult = await this.GetToken(options, code);
        var userInfoResult = await this.GetUserInfoInternal(options, tokenResult[ACCESS_TOKEN_NAME]);
        if (userInfoResult.TryGetValue(options.UserIdName!, out var userId))
        {
            return userId;
        }
        return null;
    }

    public async Task<Dictionary<string, string>> GetToken(OAuthProviderOptions options, string code)
    {
        var dictionary = this.getRequestOptions(options.TokenEndpoint);
        var url = dictionary["url"]
            .SetQueryParam(options.ClientIdName ?? CLIENT_ID_NAME, options.ClientId)
            .SetQueryParam(options.ClientSecretName ?? CLIENT_SECRET_NAME, options.ClientSecret)
            .SetQueryParam("code", code)
            .SetQueryParamIf(options.TokenEndpoint.Contains(REDIRECT_URI), REDIRECT_URI, UrlContent(options.CallbackPath));
        var result = await this.RequestAsync(url, dictionary["method"], dictionary["application"], dictionary["payload"], dictionary["accept"]);
        return result;
    }

    public async Task<Dictionary<string, string>> GetUserInfoInternal(OAuthProviderOptions options, string access_token)
    {
        var dictionary = this.getRequestOptions(options.UserInformationEndpoint);
        var url = Url.Parse(dictionary["url"]);
        var result = await this.RequestAsync(url, dictionary["method"], dictionary["application"], dictionary["payload"], dictionary["accept"], access_token, dictionary["token"]);
        return result;
    }

    private Dictionary<string, string> getRequestOptions(string endpoint)
    {
        var groupSeparator = ';';
        var keyValueSeparator = '|';
        var dictionary = endpoint.Contains(groupSeparator)
            ? endpoint.Split(groupSeparator).ToDictionary(o => o.Split(keyValueSeparator)[0].Trim(), o => o.Split(keyValueSeparator)[1].Trim())
            : new Dictionary<string, string> { { "url", endpoint } };
        if (!dictionary.ContainsKey("method"))
        {
            dictionary.Add("method", "get");
        }
        if (!dictionary.ContainsKey("application"))
        {
            dictionary.Add("application", "urlencode");
        }
        if (!dictionary.ContainsKey("payload"))
        {
            dictionary.Add("payload", "body");
        }
        if (!dictionary.ContainsKey("accept"))
        {
            dictionary.Add("accept", "urlencode");
        }
        if (!dictionary.ContainsKey("token"))
        {
            dictionary.Add("token", "header");
        }
        return dictionary;
    }

    private async Task<Dictionary<string, string>> RequestAsync(Url url,
        string method,
        string application,
        string payload,
        string accept,
        string? access_token = null,
        string? tokenPosition = null)
    {
        var isPost = method == "post";
        var isJsonRequest = application == "json";
        var isQueryPayload = payload == "query";
        var isJsonResponse = accept == "json";
        var data = url.Query.QueryStringToDictionary().DictionaryToObject();
        var request = (isPost
            ? (isQueryPayload ? url : url.RemoveQuery())
            : url).WithHeader("User-Agent", "curl");
        var result = string.Empty;
        // add access_token
        if (access_token != null)
        {
            request = tokenPosition == "header" ? request.WithOAuthBearerToken(access_token) : request.SetQueryParam(nameof(access_token), access_token);
        }
        if (isPost)
        {
            try
            {
                var response = isJsonRequest
                    ? await request.PostJsonAsync(data)
                    : (isQueryPayload ? await request.PostAsync() : await request.PostUrlEncodedAsync(data));
                result = await response.GetStringAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                throw;
            }
        }
        else
        {
            result = await request.GetStringAsync();
        }
        return isJsonResponse ? result.JsonTextToDictionary() : result.QueryStringToDictionary();
    }

    private string UrlContent(string targetPath)
    {
        if (string.IsNullOrEmpty(targetPath))
        {
            return targetPath;
        }

        if (!targetPath.StartsWith("/", StringComparison.Ordinal))
        {
            return targetPath;
        }
        var request = _httpContextAccessor.HttpContext!.Request;
        var url = $"{request.Scheme}{Uri.SchemeDelimiter}{request.Host}{request.PathBase}{targetPath}";
        return url;
    }
}
