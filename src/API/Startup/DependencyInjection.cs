using Application.Common.Interface;
using Application.Movies.Queries;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Watchlist_App.Startup;

public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApiServices();

        //controller services
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                //ignores properties that return null
                options.JsonSerializerOptions.DefaultIgnoreCondition = 
                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

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

        //application dbcontext
        builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }
}
