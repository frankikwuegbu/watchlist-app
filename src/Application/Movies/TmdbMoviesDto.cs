using System.Text.Json.Serialization;

namespace Application.Movies;

public class TmdbMoviesDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    [JsonPropertyName("original_title")]
    public string? OriginalTitle { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("original_name")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; set; }
}
