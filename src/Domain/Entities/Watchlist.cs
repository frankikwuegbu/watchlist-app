namespace Domain.Entities;

public class Watchlist
{
    public int Id { get; set; }
    public string Title { get; set; }

    private readonly List<Movie> _movies = [];
    public IReadOnlyCollection<Movie> Movies => _movies.AsReadOnly();
    private Watchlist() { } //for EF Core
    public Watchlist(string title)
    {
        Title = title;
    }
    public void AddMovie(Movie movie)
    {
        ArgumentNullException.ThrowIfNull(movie);

        if (_movies.Any(x => x.TmdbId == movie.TmdbId))
        {
            throw new InvalidOperationException($"{movie.Title} has already been added to the watchlist!");
        }

        _movies.Add(movie);
    }
}
