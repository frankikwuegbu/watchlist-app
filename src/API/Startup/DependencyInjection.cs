using Application.Common.Interface;
using Application.Movies.Queries;
using Infrastructure.Services;
using MediatR;

namespace Watchlist_App.Startup;

public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApiServices();

        //controller services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        //tmdb service
        builder.Services.AddHttpClient<ITmdbServices, TmdbServices>(client =>
        {
            client.BaseAddress = new Uri("https://api.themoviedb.org/3");
        });

        //mediatr service
        builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(GetByTitleQuery).Assembly)
        );
    }
}
