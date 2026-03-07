namespace Watchlist_App.Startup;

public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApiServices();
    }
}
