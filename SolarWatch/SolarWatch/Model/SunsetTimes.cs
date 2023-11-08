namespace SolarWatch.Model;

public class SunsetTimes
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Sunset { get; set; }
    public string? Sunrise { get; set; }
}