using System.Text.Json.Serialization;

namespace Application.Movies;

public class MovieDetailsDto
{
    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string? BackdropPath { get; set; }

    [JsonPropertyName("episode_run_time")]
    public object[] EpisodeRuntime { get; set; } = [];

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; set; }

    [JsonPropertyName("genres")]
    public Genre[] Genres { get; set; } = [];

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("in_production")]
    public bool InProduction { get; set; }

    [JsonPropertyName("languages")]
    public string[] Languages { get; set; } = [];

    [JsonPropertyName("last_air_date")]
    public string? LastAirDate { get; set; }

    [JsonPropertyName("next_episode_to_air")]
    public object? NextEpisodeToAir { get; set; }

    [JsonPropertyName("number_of_episodes")]
    public int NumberOfEpisodes { get; set; }

    [JsonPropertyName("number_of_seasons")]
    public int NumberOfSeasons { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("vote_average")]
    public float VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    public class Genre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
