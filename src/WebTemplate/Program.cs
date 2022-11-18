using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using WebTemplate.Application.Email;
using WebTemplate.Application.interfaces;
using WebTemplate.Application.Interfaces;
using WebTemplate.Application.Services.Users;
using WebTemplate.Authentication;
using WebTemplate.Infrastructure.Data;
using WebTemplate.Infrastructure.Email;
using WebTemplate.Infrastructure.Security;
using WebTemplate.Infrastructure.Tenancy;
using WebTemplate.Localization;
using WebTemplate.Services.OAuth;
using WebTemplate.Services.Tokens;
using WebTemplate.Settings;
using WebTemplate.Web;
using WebTemplate.Web.Resources;
using WebTemplate.Web.Web;

const string DEFAULT_POLICY_NAME = "AllowAllHeaders";

// Logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    //
    var builder = WebApplication.CreateBuilder(args);
    //
    ConfigureServices(builder);
    //
    builder.Services.AddScoped<TestTransformer>();
    builder.Services.AddScoped<ITenantService, TenantService>();
    builder.Services.AddTransient<ITokenService, TokenService>();
    builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
    builder.Services.AddScoped<OAuthService>();
    builder.Services.Configure<OAuthOptions>(builder.Configuration.GetSection(OAuthOptions.Position));
    builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.Position));
    builder.Services.AddScoped<IEmailService, EmailService>();

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Position));
    builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection(IdentityOptions.Position));
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IRoleService, RoleService>();
    //
    var app = builder.Build();
    //
    Configure(app);
    //
    using var scope = app.Services.CreateScope();
    using var db = scope.ServiceProvider.GetRequiredService<DbContext>();
    if (db.Database.EnsureCreated())
    {
        await scope.ServiceProvider.GetRequiredService<AppDbContextSeed>().Seed();
    }
    //
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

void ConfigureServices(WebApplicationBuilder builder)
{
    // Logger
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/log.txt", rollOnFileSizeLimit: true, fileSizeLimitBytes: 1024 * 1024 * 1, rollingInterval: RollingInterval.Infinite);
    });
    // AppSettings
    builder.Services.Configure<AppSettings>(builder.Configuration);
    // Text Encoder
    builder.Services.AddWebEncoders(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
    // JSON
    builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
    // HTTP
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();
    // FormOptions
    builder.Services.Configure<FormOptions>(options =>
    {
        options.ValueCountLimit = int.MaxValue;
        options.MultipartBodyLengthLimit = long.MaxValue;
    });
    // Cache
    builder.Services.AddMemoryCache();
    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(DEFAULT_POLICY_NAME, builder =>
        {
            builder.SetIsOriginAllowed(isOriginAllowed => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    });
    // OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                },
                Array.Empty<string>()
            }
        });
    });
    // DbContext
    builder.Services.AddScoped<DbContext, AppDbContext>();
    builder.Services.AddScoped<AppDbContextSeed>();
    builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    var dbKey = builder.Configuration.GetValue("Database", "sqlite");
    var connectionString = builder.Configuration.GetConnectionString($"db.{dbKey}");
    builder.Services.AddPooledDbContextFactory<AppDbContext>(
        options =>
        {
            if (dbKey == "mysql")
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            else if (dbKey == "cockroachdb")
            {
                options.UseNpgsql(connectionString);
            }
            else
            {
                options.UseSqlite(connectionString);
            }
        });
    // JWT
    var jwtOptions = new JwtOptions();
    builder.Configuration.GetSection(JwtOptions.Position).Bind(jwtOptions);
    var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
    // https://stackoverflow.com/questions/47138849/how-to-correctly-get-dependent-scoped-services-from-isecuritytokenvalidator
    builder.Services.AddSingleton<CustomJwtSecurityTokenHandler>();
    builder.Services.AddSingleton<JwtSecurityTokenHandler, CustomJwtSecurityTokenHandler>();
    builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, CustomJwtBearerPostConfigureOptions>();
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = issuerSigningKey,
        NameClaimType = "Name",
        RoleClaimType = "Role",
        ClockSkew = TimeSpan.Zero,//default 300
    };
    builder.Services.AddSingleton(tokenValidationParameters);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
        options.Events = new()
        {
            OnMessageReceived = context =>
            {
                // 非 json 请求 使用 cookie 中的 token
                if (!context.Request.HasJsonContentType() && context.Request.Cookies.TryGetValue("jwt", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                if (!context.Request.HasJsonContentType())
                {
                    var linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
                    var url = linkGenerator.GetPathByAction("Login", "Account", new { returnUrl = context.Request.GetDisplayUrl() }, pathBase: context.HttpContext.Request.PathBase);
                    context.Response.Redirect(url!);
                    context.HandleResponse();
                }
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                if (context.Principal?.Claims.First(o => o.Type == nameof(ClaimTypes.IsPersistent)).Value == true.ToString())
                {
                    var systemClock = context.HttpContext.RequestServices.GetRequiredService<ISystemClock>();
                    var now = systemClock.UtcNow.Date;
                    if (now.Subtract(context.SecurityToken.ValidFrom) > context.SecurityToken.ValidTo.Subtract(now))
                    {
                        var path = context.Request.GetDisplayUrl();
                        var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                        var token = tokenService.CreateTokenResult(context.Principal?.Identity?.Name!, true);
                        context.Response.Cookies.Delete("jwt");
                        context.Response.Cookies.Append("jwt", token.access_token, new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTimeOffset.Now.Add(jwtOptions.RefreshTokenExpires)
                        });
                    }
                }
                return Task.CompletedTask;
            }
        };
    });
    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
    builder.Services.AddSingleton(new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature));
    // Localization
    builder.Services.AddSingleton<IStringLocalizer>(o => o.GetRequiredService<IStringLocalizer<Resource>>());
    builder.Services.AddPortableObjectLocalization(options => options.ResourcesPath = "Resources"); // 使用 po
    var supportedCultures = new[] { "zh-Hans-CN", "en-US", };
    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        options.DefaultRequestCulture = new RequestCulture(supportedCultures.First(), supportedCultures.First());
        options.AddSupportedCultures(supportedCultures);
        options.AddSupportedUICultures(supportedCultures);
        options.RequestCultureProviders.Insert(2, new RouteDataRequestCultureProvider());
    });
    // Mvc
    builder.Services.AddRouting(options =>
    {
        // 小写 + 连字符格式
        options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
    });
    builder.Services.AddMvc(options =>
    {
        options.ModelMetadataDetailsProviders.Insert(0, new CustomIDisplayMetadataProvider());
        // 小写 + 连字符格式
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        if (builder.Environment.IsDevelopment())
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            options.JsonSerializerOptions.WriteIndented = true;
        }
    })
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var localizer = factory.Create(typeof(Resource));
            return localizer;
        };
    })
    .ConfigureApplicationPartManager(apm => apm.FeatureProviders.Add(new GenericControllerFeatureProvider()));
    // SignalR
    var redisConnectionString = builder.Configuration.GetConnectionString("redis.signalr");
    builder.Services.AddSignalR(options => options.EnableDetailedErrors = true)
        .AddStackExchangeRedis(redisConnectionString, options => options.Configuration.ChannelPrefix = "signalr");
}

void Configure(WebApplication app)
{
    app.UseSerilogRequestLogging();
    app.UseCors(DEFAULT_POLICY_NAME);
    app.UseWebSockets();
    app.UseStaticFiles();
    app.UseRouting();
    // app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always });
    app.UseAuthentication();
    app.UseAuthorization();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseRequestLocalization();

    app.UseEndpoints(endpoints =>
    {
        var requestLocalizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
        var defaults = new { culture = requestLocalizationOptions.DefaultRequestCulture.Culture.Name };

        app.MapControllerRoute(
            name: "area",
            pattern: "{area:exists:slugify}/{controller:slugify=Home}/{action:slugify=Index}/{id?}", defaults: defaults);

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}", defaults: defaults);

        //endpoints.MapDynamicControllerRoute<TestTransformer>("{culture:slugify=zh-Hans}/{controller:slugify=Home}/{action:slugify=Index}");

        endpoints.MapSwagger();
        endpoints.MapHub<TestHub>("/hub");
    });
}
