using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using ToDoListApi.Extensions;

namespace ToDoListApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IServiceCollection AddSwaggerGen(this IServiceCollection services, string version, string title)
    {
        return services.AddSwaggerGen(options =>
         {
             options.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
             options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 In = ParameterLocation.Header,
                 Description = "Please enter a valid token.",
                 Name = "Authorization",
                 Type = SecuritySchemeType.Http,
                 BearerFormat = "JWT",
                 Scheme = "Bearer"
             });
             options.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                     new OpenApiSecurityScheme
                     {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                     },
                     new List<string>()
                 }
             });
         });
    }

    public static void AddAuthentication(this IServiceCollection services, string issuer, string audience, string secretKey)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
    }

    public static void AddAuthorization(this IServiceCollection services, string policyName)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });
    }

    public static void UseRequestLocalization(this IApplicationBuilder app, string defaultCulture)
    {
        var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("uk-UA") };

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            ApplyCurrentCultureToResponseHeaders = true
        });
    }
}
