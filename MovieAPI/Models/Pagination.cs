namespace MovieAPI.Models;

public class Pagination
{
    public string? Next { get; set; }
    
    public string? Previous { get; set; }
    
    public int Current { get; set; }
    
    public int Total { get; set; }
}