
using Portal.Extensions;
using Serilog;

namespace Portal;

public static class Program
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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            await app.MigrateDatabaseAsync();
            // await app.AddPortalAdministrator();
            
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(CorsPolicy);
        
            app.UseAuthentication();
            app.UseAuthorization();
        
            app.UseEndpoints(opt =>
            {
                opt.MapControllers();
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