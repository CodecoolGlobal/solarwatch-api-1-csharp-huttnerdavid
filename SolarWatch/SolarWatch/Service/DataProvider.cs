using System.Net;
using SolarWatch.Model;

namespace SolarWatch.Service;

public class DataProvider : IDataProvider
{

    public async Task<string> ProvideGeoData(string city)
    {
        const string apiKey = "e56678b478391cd1d9c6aef789487f4d";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}";

        var client = new HttpClient();
        
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> ProvideSolarData(City city)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={city.Lat}&lng={city.Lon}";
        
        var client = new HttpClient();
        
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}