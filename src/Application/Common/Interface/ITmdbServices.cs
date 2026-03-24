using Application.Movies;

namespace Application.Common.Interface;

public interface ITmdbServices
{
    Task<List<MoviesDto>> GetByTitleAsync(string title);
    Task<MovieDetailsDto> GetDetailsByIdAsync(int id, string mediaType);
}
