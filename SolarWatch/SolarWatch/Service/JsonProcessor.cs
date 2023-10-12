using System.Text.Json;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{

    public GeoData ProcessGeoData(string data)
    {
        try
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement lat;
            JsonElement lon;
            if (json.RootElement.ValueKind == JsonValueKind.Array)
            {
                JsonElement firstElement = json.RootElement.EnumerateArray().First();
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
        }
        catch (Exception e)
        {
            return new GeoData
            {
                Lon = null,
                Lat = null
            };
        }
        return new GeoData
        {
            Lon = null,
            Lat = null
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