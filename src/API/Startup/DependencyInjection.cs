using Application.Common.Interface;
using Infrastructure.Services;

namespace Watchlist_App.Startup;

public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApiServices();

        //tmdb service
        builder.Services.AddHttpClient<ITmdbServices, TmdbServices>(client =>
        {
            client.BaseAddress = new Uri("https://api.themoviedb.org/3");
        });
    }
}
