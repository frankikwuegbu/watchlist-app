using Application.Movies;

namespace Application.Common.Interface
{
    public interface IWatchlistServices
    {
        Task<Result> CacheMovieDetailsAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken);
        Task<Result> UpdatePreviouslyCachedDataAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken);
    }
}
