using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Portal.Common.Dto;
using Portal.Common.Dto.Booking;
using Portal.Common.Dto.Dish;
using Portal.Common.Dto.Feedback;
using Portal.Common.Dto.Inventory;
using Portal.Common.Dto.Package;
using Portal.Common.Dto.User;
using Portal.Common.Dto.Zone;
using Portal.Graphql.Filters;
using Portal.Graphql.Mutations;
using Portal.Graphql.Queries;
using Portal.Services.BookingService;
using Portal.Services.BookingService.Configuration;
using Portal.Services.FeedbackService;
using Portal.Services.InventoryServices;
using Portal.Services.MenuService;
using Portal.Services.OauthService;
using Portal.Services.OauthService.Configuration;
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
        
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IBookingService, BookingService>();
        services.AddTransient<IFeedbackService, FeedbackService>();
        services.AddTransient<IOauthService, OauthService>();
        services.AddTransient<IMenuService, MenuService>();
        services.AddTransient<IZoneService, ZoneService>();
        services.AddTransient<IInventoryService, InventoryService>();
        services.AddTransient<IPackageService, PackageService>();
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
            .AddGraphQLServer()
            .AddAuthorization()
            // ObjectType
            .AddObjectType<Feedback>(m => { m.Name("Feedback"); })
            // InputObjectType
            // Create Models
            .AddInputObjectType<CreateBooking>(m => { m.Name("CreateBooking"); })
            .AddInputObjectType<CreateFeedback>(m => { m.Name("CreateFeedback"); })
            .AddInputObjectType<CreateDish>(m => { m.Name("CreateDish"); })
            .AddInputObjectType<CreatePackage>(m => { m.Name("CreatePackage"); })
            .AddInputObjectType<CreateInventory>(m => { m.Name("CreateInventory"); })
            .AddInputObjectType<CreateUser>(m => { m.Name("CreateUser"); })
            .AddInputObjectType<CreateZone>(m => { m.Name("CreateZone"); })
            // UpdateModels
            .AddInputObjectType<ConfirmBooking>(m => { m.Name("ConfirmBooking"); })
            .AddInputObjectType<Dish>(m => { m.Name("UpdateDish"); })
            .AddInputObjectType<Package>(m => { m.Name("UpdatePackage"); })
            .AddInputObjectType<Inventory>(m => { m.Name("UpdateInventory"); })
            .AddInputObjectType<UpdateZone>(m => { m.Name("UpdateZone"); })
            // Query
            .AddType<UserQuery>()
            .AddType<MenuQuery>()
            .AddType<ZoneQuery>()
            .AddType<PackageQuery>()
            .AddType<InventoryQuery>()
            .AddType<BookingQuery>()
            .AddType<FeedbackQuery>()
            // Mutation
            .AddType<UserMutation>()
            .AddType<MenuMutation>()
            .AddType<ZoneMutation>()
            .AddType<PackageMutation>()
            .AddType<InventoryMutation>()
            .AddType<BookingMutation>()
            .AddType<FeedbackMutation>()
            .AddQueryType(q =>
            {
                q.Name("Query");
            })
            .AddMutationType(m =>
            {
                m.Name("Mutation");
            })
            .AddErrorFilter<GraphQlErrorFilter>();
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