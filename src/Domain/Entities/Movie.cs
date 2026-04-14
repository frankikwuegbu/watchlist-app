namespace Domain.Entities;

public class Movie
{
    public int Id { get; private set; }
    public int WatchlistId { get; private set; }
    public int TmdbId { get; private set; }
    public string Title { get; private set; }
    public string MediaType { get; private set; }
    public string ReleaseDate { get; private set; }
    public string Overview { get; private set; }
    public string Rating { get; private set; }

    public Movie(int watchlistId, int tmdbId, string releaseDate, string overview, string rating, string title, string mediaType)
    {
        WatchlistId = watchlistId;
        TmdbId = tmdbId;
        ReleaseDate = releaseDate;
        Overview = overview;
        Rating = rating;
        Title = title;
        MediaType = mediaType;
    }

    private Movie() { } //for EF core
}