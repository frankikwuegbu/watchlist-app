using Application.Common;
using Application.Common.Interface;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Watchlists.Queries;

public record GetWatchlistByMediaTypeQuery(string MediaType) : IRequest<Result>;

public class GetWatchlistByMediaTypeQueryHandler : IRequestHandler<GetWatchlistByMediaTypeQuery, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWatchlistByMediaTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Handle(GetWatchlistByMediaTypeQuery request, CancellationToken cancellationToken)
    {
        if (request.MediaType is not ("movie" or "tv"))
        {
            return Result.Failure("oops! media type can only be 'movie' or 'tv'");
        }
        //check for watchlist
        var watchlist = await _context.Watchlist.Include(x => x.Movies).FirstOrDefaultAsync(cancellationToken);

        if (watchlist is null)
        {
            return Result.Failure("oops! there are no movies in your watchlist");
        }

        //filter by media type
        var movies = watchlist.Movies.Where(x => x.MediaType == request.MediaType).ToList();
        var moviesDto = _mapper.Map<List<MoviesDto>>(movies);

        return Result.Success($"{request.MediaType}", moviesDto);
    }
}