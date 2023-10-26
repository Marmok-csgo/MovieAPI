namespace MovieAPI.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Country? Country { get; set; }
    public int CountryId { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string? Poster { get; set; }
    public string? Author { get; set; }
    
}