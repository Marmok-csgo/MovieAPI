namespace MovieAPI.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Country? Country { get; set; }
    public int CountryId { get; set; }
    
    public List<Genre> Genres { get; set; } = null!;

    public List<People> People { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }
    public string? Poster { get; set; }
}