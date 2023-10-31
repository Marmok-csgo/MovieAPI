namespace MovieAPI.Models.HttpApi;

public class Genre
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class RootObj
{
    public List<Genre> genres { get; set; }
}