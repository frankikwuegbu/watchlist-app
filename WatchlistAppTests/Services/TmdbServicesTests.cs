using Application.Common;
using Application.Movies;
using Infrastructure.Services;
using RichardSzalay.MockHttp;
using System.Net.Http.Json;

namespace WatchlistAppTests.Services;

public class TmdbServicesTests
{
    [Fact]
    public async Task GetByTitle_ShouldReturnList()
    {
        //Arrange
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("http://example.com/3/search")
            .Respond("application/json",
                @"{""page"": 1, ""results"": [{""media_type"": ""movie""}], ""total_pages"": 1, ""total_results"": 1}");

        var client = new HttpClient(mockHttp);

        //Act
        var result = await client.GetFromJsonAsync<TmdbResponse>("http://example.com/3/search");

        //Assert
        Assert.True(result.Results.Count >= 0);
    }

    [Fact]
    public void MoviesAndTvFromResults_ShouldAddOnlyMoviesAndTv()
    {
        //Arrange
        var tmdbResponse = new TmdbResponse
        {
            Page = 1,
            Results = 
                [
                new MoviesDto { MediaType = "movie" },
                new MoviesDto { MediaType = "tv" },
                new MoviesDto { MediaType = "person" }
                ],
            TotalPages = 1,
            TotalResults = 1
        };

        var results = tmdbResponse.Results;

        //Act
        var actual = TmdbServices.MoviesAndTvFromResults(results);

        //Assert
        Assert.True(actual.Count == 2);
    }

    [Fact]
    public async Task GetDetailsById_ShouldReturnDetails_WithValidIdAndMediaType()
    {
        //Arrange
        var mockHttp = new MockHttpMessageHandler();

        var validID = 1;
        var validMediaType = "movie";

        mockHttp.When($"http://example.com/3/{validMediaType}/{validID}")
            .Respond("application/json",
                @"{""page"": 1, ""results"": [{""media_type"": ""movie""}], ""total_pages"": 1, ""total_results"": 1}");

        var client = new HttpClient(mockHttp);

        //Act
        var response = await client.GetFromJsonAsync<MovieDetailsDto>($"http://example.com/3/{validMediaType}/{validID}");

        //Assert
        Assert.NotNull(response);
    }
}
