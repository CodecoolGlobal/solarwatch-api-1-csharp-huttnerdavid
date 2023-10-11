using System.Text.Json;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{
    private readonly ILogger<JsonProcessor> _logger;

    public JsonProcessor(ILogger<JsonProcessor> logger)
    {
        _logger = logger;
    }

    public GeoData ProcessGeoData(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement lat;
        JsonElement lon;
        if (json.RootElement.ValueKind == JsonValueKind.Array)
        {
            JsonElement firstElement = json.RootElement.EnumerateArray().FirstOrDefault();
            if (firstElement.TryGetProperty("lat", out lat) && firstElement.TryGetProperty("lon", out lon))
            {
                return new GeoData
                {
                    Lon = lon.GetDouble(),
                    Lat = lat.GetDouble(),
                    City = firstElement.GetProperty("name").GetString()
                };
            }
        }

        return new GeoData
        {
            Lon = 0,
            Lat = 0
        };
    }

    public SolarWatch ProcessSolarData(string data, string city)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        return new SolarWatch()
        {
            City = city,
            Date = DateTime.Now,
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString()
        };
    }
}