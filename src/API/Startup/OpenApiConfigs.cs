using Scalar.AspNetCore;

namespace Watchlist_App.Startup;

public static class OpenApiConfigs
{
    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
    }

    public static void UseOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Watchlist App";
                options.HideClientButton = true;
            });
        }
    }
}
