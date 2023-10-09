using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Portal.Services.BookingService;
using Portal.Services.BookingService.Configuration;
using Portal.Services.FeedbackService;
using Portal.Services.InventoryServices;
using Portal.Services.MenuService;
using Portal.Services.OauthService;
using Portal.Services.PackageService;
using Portal.Services.UserService;
using Portal.Services.ZoneService;
using Portal.Swagger;

namespace Portal.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPortalService(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<BookingServiceConfiguration>(config.GetRequiredSection("BookingServiceConfiguration"));
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IOauthService, OauthService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IZoneService, ZoneService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IPackageService, PackageService>();
    }
    
    public static void AddPortalSwaggerGen(this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Portal API",
                    Version = "v1",
                    Description = "Anticafe Portal API"
                });

            options.EnableAnnotations();
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            
            options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            
            var systemConfig = config.GetRequiredSection("SystemConfiguration");
            var dateFormat = systemConfig.GetValue<string>("Date");
            var timeFormat = systemConfig.GetValue<string>("Time");
            
            options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = dateFormat});
            options.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Format = timeFormat});
            
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
               
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            foreach (var supProjectXmlFiles in dir.EnumerateFiles("*.xml"))
            {
                options.IncludeXmlComments(supProjectXmlFiles.FullName);
            }
        });
    }

    public static void AddPortalGraphql(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddGraphQLServer();
        //.AddQueryType<>();
    }
    
    public static void AddPortalCors(this IServiceCollection services, string policyName)
    {
        services.AddCors(o => o.AddPolicy(policyName,policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));
    }
    
    public static void AddPortalJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AuthorizationConfiguration>(config.GetSection("AuthorizationConfiguration"));

        var authOptions = config.GetRequiredSection("AuthorizationConfiguration").Get<AuthorizationConfiguration>()!;
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey =true
                };
            });
    }
}