namespace MovieAPI.Models.HttpApi;

public class Data
{
    public string? Name { get; set; }
    public string? Iso2 { get; set; }
    public double? Long { get; set; }
    public double? Lat { get; set; }
}

public class RootObject
{
    public bool Error { get; set; }
    public string? Msg { get; set; }
    public List<Data>? Data { get; set; }
}