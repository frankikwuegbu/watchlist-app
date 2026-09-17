using Application.Common;
using Application.Common.Interface;
using Application.Movies;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Application.Watchlists.Command;

public record AddToWatchlistCommand(int TmdbId, string MediaType = "movie or tv") : IRequest<Result>;

public class AddToWatchlistCommandHandler : IRequestHandler<AddToWatchlistCommand, Result>
{
    private readonly ITmdbServices _tmdbServices;
    private readonly IApplicationDbContext _context;
    private readonly IWatchlistServices _watchlistServices;

    public AddToWatchlistCommandHandler(ITmdbServices tmdbServices, IApplicationDbContext context, IWatchlistServices watchlistServices)
    {
        _tmdbServices = tmdbServices;
        _context = context;
        _watchlistServices = watchlistServices;
    }

    public async Task<Result> Handle(AddToWatchlistCommand request, CancellationToken cancellationToken)
    {
        var watchlist = await _context.Watchlist
            .Include(x => x.Movies)
            .FirstOrDefaultAsync(cancellationToken);

        if (watchlist is null)
        {
            watchlist = new Watchlist("Watchlist App");
            await _context.Watchlist.AddAsync(watchlist, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var cachedDetails = await _context.CachedMedia
            .FirstOrDefaultAsync(x => x.TmdbId == request.TmdbId && x.MediaType == request.MediaType, cancellationToken);

        if (cachedDetails is null)
        {
            return Result.Failure("oops! movie details not found. Cannot add to watchlist");
        }

        var movieDetails = JsonSerializer.Deserialize<MovieDetailsDto>(cachedDetails.JsonDetails);

        var movie = new Movie(
            watchlist.Id,
            request.TmdbId,
            movieDetails.ReleaseDate ?? movieDetails.FirstAirDate,
            movieDetails.Overview,
            movieDetails.VoteAverage.ToString("0.0"),
            movieDetails.OriginalName ?? movieDetails.Title,
            movieDetails.MediaType
            );

        try
        {
            watchlist.AddMovie(movie);
        }
        catch (InvalidOperationException e)
        {
            return Result.Failure(e.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success($"{movie.Title} has been added to your watchlist!");
    }
}
