using System.Net;

namespace SolarWatch.Service;

public class DataProvider : IDataProvider
{
    private readonly ILogger<DataProvider> _logger;
    public DataProvider(ILogger<DataProvider> logger)
    {
        _logger = logger;
    }
    public string ProvideGeoData(string city)
    {
        const string apiKey = "e56678b478391cd1d9c6aef789487f4d";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}";

        var client = new WebClient();

        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);
        return client.DownloadString(url);
    }

    public string ProvideSolarData(GeoData geoData)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={geoData.Lat}&lng={geoData.Lon}";
        
        var client = new WebClient();

        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);
        return client.DownloadString(url);
    }
}