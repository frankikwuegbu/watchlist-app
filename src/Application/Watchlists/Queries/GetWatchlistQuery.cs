using Application.Common;
using Application.Common.Interface;
using Application.Movies;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Watchlists.Queries;

public record GetWatchlistQuery(string Title = "Watchlist App") : IRequest<Result>;

public class GetWatchlistQueryHandler : IRequestHandler<GetWatchlistQuery, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWatchlistQueryHandler(IApplicationDbContext context, IMapper map)
    {
        _context = context;
        _mapper = map;
    }

    public async Task<Result> Handle(GetWatchlistQuery request, CancellationToken cancellationToken = default)
    {
        var watchlist = await _context.Watchlist.AsNoTracking().Include(x => x.Movies).FirstOrDefaultAsync(x => x.Title == request.Title, cancellationToken);

        if (watchlist is null)
        {
            return Result.Failure($"oops! watchlist does not exist!");
        }

        var watchlistMovies = watchlist.Movies;
        var watchlistMoviesDto = _mapper.Map<List<MoviesDto>>(watchlistMovies);

        return Result.Success($"{watchlist.Title}", watchlistMoviesDto);
    }
}
