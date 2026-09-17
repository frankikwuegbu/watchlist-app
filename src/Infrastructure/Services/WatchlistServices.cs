using Application.Common;
using Application.Common.Interface;
using Application.Movies;
using Domain.Entities;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class WatchlistServices : IWatchlistServices
    {
        private readonly IApplicationDbContext _context;

        public WatchlistServices(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> CacheMovieDetailsAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken)
        {
            try
            {
                var jsonDetails = JsonSerializer.Serialize(movieDetails);

                var cachedMedia = new CachedMedia
                {
                    TmdbId = movieDetails.Id,
                    MediaType = movieDetails.MediaType,
                    ReleaseDate = movieDetails.ReleaseDate,
                    LastUpdated = DateTime.UtcNow,
                    JsonDetails = jsonDetails
                };

                _context.CachedMedia.Add(cachedMedia);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Media details cached successfully.", cachedMedia);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occurred while caching the media details: {ex.Message}");
            }
        }

        public async Task<Result> UpdatePreviouslyCachedDataAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken)
        {
            try
            {
                var existingCache = _context.CachedMedia.FirstOrDefault(c => c.TmdbId == movieDetails.Id && c.MediaType == movieDetails.MediaType);
                if (existingCache != null)
                {
                    existingCache.ReleaseDate = movieDetails.ReleaseDate;
                    existingCache.LastUpdated = DateTime.UtcNow;
                    existingCache.JsonDetails = JsonSerializer.Serialize(movieDetails);

                    _context.CachedMedia.Update(existingCache);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("media details cached successfully", existingCache);
                }
                return Result.Failure("No previously cached media found to update.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occurred while updating the cached media details: {ex.Message}");
            }
        }
    }
}
