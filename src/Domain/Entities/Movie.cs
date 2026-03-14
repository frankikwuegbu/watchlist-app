namespace Domain.Entities;

public class Movie
{
    public int Id { get; private set; }
    public int WatchlistId { get; private set; }
    public int TmdbId { get; private set; }
    public string Title { get; private set; }
    public string ReleaseDate { get; private set; }
    public string Overview { get; private set; }
    public double Rating { get; private set; }

    private Movie() { } //for EF core
}