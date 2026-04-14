using Application.Common;
using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Watchlists.Command;

public record AddToWatchlistCommand(int Id, string MediaType = "movie or tv") : IRequest<Result>;

public class AddToWatchlistCommandHandler : IRequestHandler<AddToWatchlistCommand, Result>
{
    private readonly ITmdbServices _tmdbServices;
    private readonly IApplicationDbContext _context;

    public AddToWatchlistCommandHandler(ITmdbServices tmdbServices, IApplicationDbContext context)
    {
        _tmdbServices = tmdbServices;
        _context = context;
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

        var movieDetails = await _tmdbServices.GetDetailsByIdAsync(request.Id, request.MediaType);

        //get media type from simple movie details
        var movieList = await _tmdbServices.GetByTitleAsync(movieDetails.Title ?? movieDetails.OriginalName);
        var specificMovie = movieList.FirstOrDefault(x => x.Id == request.Id);

        if (movieDetails is null || specificMovie is null)
        {
            return Result.Failure("oops! movie details not found. Cannot add to watchlist");
        }

        var movie = new Movie(
            watchlist.Id,
            request.Id,
            movieDetails.ReleaseDate ?? movieDetails.FirstAirDate,
            movieDetails.Overview,
            movieDetails.VoteAverage.ToString("0.0"),
            movieDetails.OriginalName ?? movieDetails.Title,
            specificMovie.MediaType
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
