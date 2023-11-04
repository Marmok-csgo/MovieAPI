namespace MovieAPI.Responses;

public class MovieResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CountryName { get; set; }
    
    public DateOnly RealeseDate { get; set; }
    
    public string? Poster { get; set; }
    
    public string? Author { get; set; }
    
    public List<string> Artists { get; set; }
    
    public List<string> Genres { get; set; }

    public MovieResponse(int id, string? name, string? description, string? countryName, DateOnly realeseDate, string? poster, string author, List<string> artists, List<string> genres)
    {
        Id = id;
        Name = name;
        Description = description;
        CountryName = countryName;
        RealeseDate = realeseDate;
        Poster = poster;
        Author = author;
        Artists = artists;
        Genres = genres;
    }
}