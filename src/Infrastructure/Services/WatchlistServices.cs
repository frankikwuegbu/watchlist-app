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

        public async Task<CachedMedia> CacheMovieDetailsAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken)
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
                return cachedMedia;
            }
            catch (Exception)
            {
                return new CachedMedia { };
            }
        }

        public async Task<CachedMedia> UpdatePreviouslyCachedDataAsync(MovieDetailsDto movieDetails, CancellationToken cancellationToken)
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
                    return existingCache;
                }
                return new CachedMedia { };
            }
            catch (Exception)
            {
                return new CachedMedia { };
            }
        }
    }
}
