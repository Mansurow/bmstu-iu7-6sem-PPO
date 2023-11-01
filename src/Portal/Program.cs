using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Portal.Extensions;
using Serilog;

namespace Portal;

public class Program
{
    private const string CorsPolicy = "AllowAllCorsPolicy";
    public static async Task Main(string[] args)
    {
        try
        {
            var app = WebApplication.CreateBuilder(args)
                .ConfigurePortalServices(CorsPolicy)
                .ConfigureSerilog()
                .Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                 
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UsePlayground(new PlaygroundOptions
            {
                QueryPath = "/api/v2",
                Path = "/graphql"
            });
            
            await app.MigrateDatabaseAsync();
            await app.AddPortalAdministrator();
            
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(CorsPolicy);
        
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(opt =>
            {
                opt.MapControllers();
                //opt.MapGraphQL("/graphql");
                opt.MapGraphQLHttp("/api/v2");
            });
            // app.MapControllers();

            await app.RunAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "Something went wrong!");
        }
        finally
        {
            Log.Information("Portal Service is stopping...");
            await Log.CloseAndFlushAsync();
        }
    }
}