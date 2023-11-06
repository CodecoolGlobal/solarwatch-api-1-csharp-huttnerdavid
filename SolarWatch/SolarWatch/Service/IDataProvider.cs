using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IDataProvider
{
    public Task<string> ProvideGeoData(string city);
    public Task<string> ProvideSolarData(City city);
}