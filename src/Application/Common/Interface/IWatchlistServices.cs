using Application.Movies;
using Domain.Entities;

namespace Application.Common.Interface
{
    public interface IWatchlistServices
    {
        Task<CachedMedia> CacheMovieDetailsAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken);
        Task<CachedMedia> UpdatePreviouslyCachedDataAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken);
    }
}
