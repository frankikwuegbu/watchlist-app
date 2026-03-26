using Application.Common;
using Application.Common.Interface;
using MediatR;

namespace Application.Movies.Queries;

public record GetDetailsByIdQuery(int Id, string MediaType) : IRequest<Result>;
public class GetDetailsByIdQueryHanldQuery : IRequestHandler<GetDetailsByIdQuery, Result>
{
    private readonly ITmdbServices _tmdbService;

    public GetDetailsByIdQueryHanldQuery(ITmdbServices tmdbService)
    {
        _tmdbService = tmdbService;
    }

    public async Task<Result> Handle(GetDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _tmdbService.GetDetailsByIdAsync(request.Id, request.MediaType);

            if (result == null)
            {
                return Result.Failure("oops! details not found!");
            }

            return Result.Success("details fetched!", result);
        }
        catch (HttpRequestException)
        {
            return Result.Failure("oops! invalid query parameter(s): mediaType can only be 'movie' or 'tv'");
        }
    }
}
