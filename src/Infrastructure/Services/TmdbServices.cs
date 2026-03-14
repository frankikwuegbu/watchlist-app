using Application.Common;
using Application.Common.Interface;
using Application.Movies;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Infrastructure.Services;

public class TmdbServices : ITmdbServices
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TmdbServices(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<MoviesDto>> GetByTitleAsync(string title)
    {
        var apiKey = _configuration["TMDB:ApiKey"];

        var response = await _httpClient.GetFromJsonAsync<TmdbResponse>(
            $"3/search/multi?api_key={apiKey}&query={title}"
        );

        var results = response.Results;

        return results;
    }
}
