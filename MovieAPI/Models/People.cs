namespace MovieAPI.Models;

public class People
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public bool IsAuthor { get; set; }
    
    public List<Movie>? Movies { get; set; }
}