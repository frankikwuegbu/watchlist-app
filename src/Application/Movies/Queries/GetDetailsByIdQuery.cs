using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Application.Movies.Queries;

public record GetDetailsByIdQuery(
    int TmdbId, 
    string MediaType) : IRequest<Result>;
public class GetDetailsByIdQueryHandler : IRequestHandler<GetDetailsByIdQuery, Result>
{
    private readonly ITmdbServices _tmdbService;
    private readonly IApplicationDbContext _context;
    private readonly IWatchlistServices _watchlistServices;
    private readonly IServiceScopeFactory _scopeFactory;

    public GetDetailsByIdQueryHandler(ITmdbServices tmdbService, IApplicationDbContext context, IWatchlistServices watchlistServices, IServiceScopeFactory scopeFactory)
    {
        _tmdbService = tmdbService;
        _context = context;
        _watchlistServices = watchlistServices;
        _scopeFactory = scopeFactory;
    }

    public async Task<Result> Handle(GetDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            //first check cache for details
            var cachedMedia = await _context.CachedMedia.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TmdbId == request.TmdbId && x.MediaType == request.MediaType, cancellationToken);

            if (cachedMedia is null)
            {
                var output = await _tmdbService.GetDetailsByIdAsync(request.TmdbId, request.MediaType);
                var cacheResult = await _watchlistServices.CacheMovieDetailsAsync(output, cancellationToken);
                cachedMedia = cacheResult;

                var result =  JsonSerializer.Deserialize<object>(cachedMedia.JsonDetails);
                return Result.Success("details fetched!", result);
            }

            var cachedMediaIsStale = (DateTime.UtcNow - cachedMedia.LastUpdated).TotalDays > 7;
            var releaseDateIsNowPast = DateTime.TryParse(cachedMedia.ReleaseDate, out var releaseDate)
                && releaseDate > cachedMedia.LastUpdated
                && releaseDate <= DateTime.UtcNow;

            //update stale cache in the background
            if (cachedMediaIsStale || releaseDateIsNowPast)
                _ = Task.Run(() => UpdateCachedMediaInTheBackground(request.TmdbId, request.MediaType), CancellationToken.None);

            var cachedResult = JsonSerializer.Deserialize<object>(cachedMedia.JsonDetails);
            return Result.Success("details fetched!", cachedResult);
        }
        catch (HttpRequestException)
        {
            return Result.Failure("oops! invalid query parameter(s): mediaType can only be 'movie' or 'tv'");
        }
    }

    private async Task UpdateCachedMediaInTheBackground(int tmdbId, string mediaType)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            var backgroundTmdbServices = scope.ServiceProvider.GetService<ITmdbServices>();
            var backgroundWatchlistServices = scope.ServiceProvider.GetService<IWatchlistServices>();

            var output = await backgroundTmdbServices.GetDetailsByIdAsync(tmdbId, mediaType);

            if (output is not null)
            {
                await backgroundWatchlistServices.UpdatePreviouslyCachedDataAsync(output, CancellationToken.None);
            }
        }
        catch { }
    }
}
