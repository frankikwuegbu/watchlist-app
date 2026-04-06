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

    public async Task<List<TmdbMoviesDto>> GetByTitleAsync(string title)
    {
        var apiKey = _configuration["TMDB:ApiKey"];

        var response = await _httpClient.GetFromJsonAsync<TmdbResponse>(
            $"3/search/multi?api_key={apiKey}&query={title}"
        );

        var results = response?.Results;

        //output should only be movies or tv shows
        return MoviesAndTvFromResults(results);
    }

    public async Task<MovieDetailsDto> GetDetailsByIdAsync(int id, string mediaType)
    {
        var apiKey = _configuration["TMDB:ApiKey"];

        var response = await _httpClient.GetFromJsonAsync<MovieDetailsDto>(
            $"/3/{mediaType}/{id}?api_key={apiKey}"
            );

        return response;
    }

    public static List<TmdbMoviesDto> MoviesAndTvFromResults(List<TmdbMoviesDto> results)
    {
        var moviesAndTv = new List<TmdbMoviesDto>();

        foreach (var result in results)
        {
            if (result.MediaType != "person")
            {
                moviesAndTv.Add(result);
            }
        }

        return moviesAndTv;
    }
}