using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service;

public class JsonProcessor : IJsonProcessor
{

    public City ProcessGeoData(string data)
    {
        try
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement lat;
            JsonElement lon;
            JsonElement state;
            if (json.RootElement.ValueKind == JsonValueKind.Array)
            {
                JsonElement firstElement = json.RootElement.EnumerateArray().First();
                if (firstElement.TryGetProperty("lat", out lat) && firstElement.TryGetProperty("lon", out lon))
                {
                    if (firstElement.TryGetProperty("state", out state))
                    {
                        return new City
                        {
                            Lon = lon.GetDouble(),
                            Lat = lat.GetDouble(),
                            Name = firstElement.GetProperty("name").GetString(),
                            State = state.GetString(),
                            Country = firstElement.GetProperty("country").GetString()
                        };
                    }
                    return new City
                    {
                        Lon = lon.GetDouble(),
                        Lat = lat.GetDouble(),
                        Name = firstElement.GetProperty("name").GetString(),
                        Country = firstElement.GetProperty("country").GetString()
                    };
                }


            }
        }
        catch (Exception e)
        {
            return new City
            {
                Lon = null,
                Lat = null
            };
        }
        return new City
        {
            Lon = null,
            Lat = null
        };
    }

    public SunsetTimes ProcessSolarData(string data, string city)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        return new SunsetTimes
        {
            Name = city,
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString()
        };
    }
}