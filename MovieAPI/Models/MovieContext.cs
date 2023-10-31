using Microsoft.EntityFrameworkCore;

namespace MovieAPI.Models;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options){}

    public DbSet<Movie> Movies { get; set; } = null!;
    
    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<Role> Roles { get; set; } = null!;
    
    public DbSet<Country> Countries { get; set; } = null!;
    
    public DbSet<Genre> Genres { get; set; } = null!;
    
    public DbSet<People> People { get; set; } = null!;
}