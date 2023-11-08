using MovieAPI.Responses;

namespace MovieAPI.Models;

public class MovieAndPaginationResponse
{
    public List<MovieResponse>? Movies { get;  set; }
    
    public Pagination? Links { get; set; }
}