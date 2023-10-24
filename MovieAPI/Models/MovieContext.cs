using Microsoft.EntityFrameworkCore;

namespace MovieAPI.Models;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options){}

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql
    //         ("host=localhost;Port=7777;Username=postgres;Password=0814;Database=movie;");
    public DbSet<Movie> Movies { get; set; } = null!;
    
    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<Role> Roles { get; set; } = null!;
}