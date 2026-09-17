using Application.Movies;

namespace Application.Common.Interface;

public interface ITmdbServices
{
    Task<List<TmdbMoviesDto>> GetByTitleAsync(string title);
    Task<Result> GetDetailsByIdAsync(int id, string mediaType);
}
