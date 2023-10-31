namespace MovieAPI.Models.HttpApi;

public class DataPeople
{
    public bool Adult { get; set; }
    public int Gender { get; set; }
    public int Id { get; set; }
    public string? KnownForDepartment { get; set; }
    public string? Name { get; set; }
    public string? OriginalName { get; set; }
    public double Popularity { get; set; }
    public string? ProfilePath { get; set; }
    public int CastId { get; set; }
    public string? Character { get; set; }
    public string? CreditId { get; set; }
    public int Order { get; set; }
    
}

public class PeopleRootObject
{
    public int Id { get; set; }
    
    public List<DataPeople> Cast { get; set; } = null!;
}