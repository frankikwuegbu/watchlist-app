namespace Application.Watchlists;

public class MoviesDto
{
    public int Id { get; private set; }
    public int TmdbId { get; private set; }
    public string Title { get; private set; }
    public string ReleaseDate { get; private set; }
    public string Rating { get; private set; }
}
