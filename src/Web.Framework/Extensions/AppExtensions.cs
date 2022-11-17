using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Web.Framework.Localization;

namespace Web.Framework.Extensions;

public static class AppExtensions
{
    public const string DEFAULT_POLICY_NAME = "AllowAllHeaders";

    public static void ConfigureServices<TDbContext>(this IServiceCollection services, IConfiguration config)
        where TDbContext : DbContext
    {
        // Text Encoder
        services.AddWebEncoders(o => o.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
        // JSON
        services.Configure<JsonOptions>(o =>
        {
            o.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            o.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        });
        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy(DEFAULT_POLICY_NAME, builder =>
            {
                builder.SetIsOriginAllowed(o => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });
        // OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
            });
            o.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        services.AddScoped<DbContext, TDbContext>();
        var dbKey = config.GetValue("Database", "sqlite");
        var connectionString = config.GetConnectionString($"db.{dbKey}");
        services.AddPooledDbContextFactory<TDbContext>(
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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = key,
            NameClaimType = "Name",
            RoleClaimType = "Role",
            ClockSkew = TimeSpan.Zero,//default 300
        };
        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
        });
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
        services.AddSingleton(new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));
        // Localization
        services.AddSingleton<IValidationAttributeAdapterProvider, LocalizedValidationAttributeAdapterProvider>();
        services.AddLocalization();
        services.AddPortableObjectLocalization(o => o.ResourcesPath = "Resources"); // 使用 po
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] {
                new CultureInfo("zh-CN"),
                new CultureInfo("en-US"),
            };
            options.DefaultRequestCulture = new RequestCulture("zh-CN");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
        });
        // Mvc
        // services.AddMvc(o => o.ModelMetadataDetailsProviders.Insert(0, new CustomIDisplayMetadataProvider()));
        // SignalR
        services.AddSignalR().AddStackExchangeRedis(config.GetConnectionString("redis.signalr"), o => o.Configuration.ChannelPrefix = "signalr");
    }

    public static void Configure(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        app.UseCors(DEFAULT_POLICY_NAME);
        app.UseWebSockets();
        app.UseDefaultFiles();//UseDefaultFiles 必须在 UseStaticFiles 之前
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRequestLocalization(scope.ServiceProvider.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        app.UseEndpoints(endpoints =>//MapDynamicControllerRoute
        {
            // cultureRoute 必须在 default 之前，否则 url 的 culture 部分会变成参数而不是路径
            endpoints.MapControllerRoute(
                name: "cultureRoute",
                pattern: "{culture=zh-CN}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { culture = "zh-CN" });

            endpoints.MapFallbackToFile("/index.html");

            // endpoints.MapHub<TestHub>("/hub");
            endpoints.MapSwagger();
        });
    }
}
