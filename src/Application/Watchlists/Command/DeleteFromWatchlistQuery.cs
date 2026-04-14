using Application.Common;
using Application.Common.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Watchlists.Command
{
    public record DeleteFromWatchlistQuery(int Id) : IRequest<Result>;

    public class DeleteFromWatchlistQueryHandler : IRequestHandler<DeleteFromWatchlistQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteFromWatchlistQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteFromWatchlistQuery request, CancellationToken cancellationToken)
        {
            //get watchlist
            var watchlist = await _context.Watchlist.Include(x => x.Movies).FirstOrDefaultAsync(cancellationToken);

            if (watchlist == null)
            {
                return Result.Failure("there are no movies in your wachlist");
            }

            //check for movie
            var movie = watchlist.Movies.FirstOrDefault(x => x.TmdbId == request.Id);

            if (movie is null)
            {
                return Result.Failure("oops! the selected movie or tv show does not exist in your watchlist");
            }

            try
            {
                watchlist.RemoveMovie(movie);
            }
            catch (InvalidOperationException e)
            {
                return Result.Failure(e.Message);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success($"{movie.Title} has been removed from your watchlist");
        }
    }
}