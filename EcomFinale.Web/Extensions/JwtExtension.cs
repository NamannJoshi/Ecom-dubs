using System.Text;
using EcomFinale.DataAccess.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EcomFinale.Web.Extensions;

public static class JwtExtension
{
    public static IServiceCollection AddJwtExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration
        .GetSection("JwtOptions")
        .Get<JwtOptions>();

        if (jwtOptions is null)
        {
            throw new Exception("JwtOptions configuration is missing.");
        }

        services.Configure<JwtOptions>(
            configuration.GetSection("JwtOptions"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}