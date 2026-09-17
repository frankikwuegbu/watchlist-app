using Application.Movies;

namespace Application.Common.Interface;

public interface ITmdbServices
{
    Task<List<TmdbMoviesDto>> GetByTitleAsync(string title);
    Task<MovieDetailsDto> GetDetailsByIdAsync(int id, string mediaType);
}
