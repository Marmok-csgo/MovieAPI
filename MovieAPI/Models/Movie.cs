namespace MovieAPI.Models;

public class Movie
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public DateOnly ReleaseDate { get; set; }
}