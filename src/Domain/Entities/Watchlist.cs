namespace Domain.Entities;

public class Watchlist
{
    public int Id { get; set; }

    private readonly List<Movie> _movies = [];
    public IReadOnlyCollection<Movie> Movies => _movies.AsReadOnly();

    private Watchlist() { } //for EF Core

    public void AddMovie(Movie movie)
    {
        _movies.Add(movie); //do more here!
    }
}
