using Microsoft.Build.Framework;

namespace MovieAPI.Models;

public class MovieDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public int CountryId { get; set; }

    [Required]
    public List<int>? GenresIds { get; set; }

    [Required]
    public List<int>? PeopleIds { get; set; }

    [Required]
    public DateOnly ReleaseDate { get; set; }

    [Required]
    public IFormFile? Poster { get; set; }
    
    [Required]
    public IFormFile? Video { get; set; }
}
