namespace MovieAPI.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public Role? Role { get; set; }
    public int RoleId { get; set; }
}