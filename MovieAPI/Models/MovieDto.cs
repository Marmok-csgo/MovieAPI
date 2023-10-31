namespace MovieAPI.Models;

public class MovieDto
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public int CountryId { get; set; }
    
    public List<int> GenresIds { get; set; } = null!;

    public List<int> PeopleIds { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }
    
    public IFormFile? Poster { get; set; }
    
}