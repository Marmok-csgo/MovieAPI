namespace MovieAPI.Models;

public class MovieDto
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public int CountryId { get; set; }
    
    public List<int> GenresIds { get; set; }
    
    public DateOnly ReleaseDate { get; set; }
    
    public IFormFile? Poster { get; set; }
    
    public string? Author { get; set; }
}