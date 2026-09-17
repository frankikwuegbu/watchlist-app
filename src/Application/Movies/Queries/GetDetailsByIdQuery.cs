using Application.Common;
using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

    public GetDetailsByIdQueryHandler(ITmdbServices tmdbService, IApplicationDbContext context, IWatchlistServices watchlistServices)
    {
        _tmdbService = tmdbService;
        _context = context;
        _watchlistServices = watchlistServices;
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
                var cacheResult = await _watchlistServices.CacheMovieDetailsAsync(output.Entity as MovieDetailsDto, cancellationToken);
                cachedMedia = cacheResult.Entity as CachedMedia;
            }

            var cachedMediaIsStale = (DateTime.UtcNow - cachedMedia.LastUpdated).TotalDays > 7;
            var releaseDateIsNowPast = DateTime.TryParse(cachedMedia.ReleaseDate, out var releaseDate)
                && releaseDate > cachedMedia.LastUpdated
                && releaseDate <= DateTime.UtcNow;

            if (cachedMediaIsStale || releaseDateIsNowPast)
            {
                var output = await _tmdbService.GetDetailsByIdAsync(request.TmdbId, request.MediaType);
                var cachedResult = await _watchlistServices.UpdatePreviouslyCachedDataAsync(output.Entity as MovieDetailsDto, cancellationToken);
                cachedMedia = cachedResult.Entity as CachedMedia;
            }

            var result = JsonSerializer.Deserialize<object>(cachedMedia.JsonDetails);

            return Result.Success("details fetched!", result);
        }
        catch (HttpRequestException)
        {
            return Result.Failure("oops! invalid query parameter(s): mediaType can only be 'movie' or 'tv'");
        }
    }
}
