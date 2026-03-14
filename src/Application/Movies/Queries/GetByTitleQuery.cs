using Application.Common;
using Application.Common.Interface;
using MediatR;

namespace Application.Movies.Queries;

public record GetByTitleQuery(string Title) : IRequest<Result>;

public class GetByTitleQueryHandler : IRequestHandler<GetByTitleQuery, Result>
{
    private readonly ITmdbServices _tmdbServices;

    public GetByTitleQueryHandler(ITmdbServices tmdbServices)
    {
        _tmdbServices = tmdbServices;
    }

    public async Task<Result> Handle(GetByTitleQuery request, CancellationToken cancellationToken = default)
    {
        var result = await _tmdbServices.GetByTitleAsync(request.Title);

        if (result.Count == 0)
        {
            return Result.Failure("Movie or series not found!");
        }

        return Result.Success("Found!", result);
    }
}