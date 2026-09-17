namespace Domain.Entities
{
    public class CachedMedia
    {
        public int Id { get; set; }
        public int TmdbId { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public string JsonDetails { get; set; } = string.Empty;
    }
}
