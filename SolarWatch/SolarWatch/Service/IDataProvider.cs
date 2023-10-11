namespace SolarWatch.Service;

public interface IDataProvider
{
    public string ProvideGeoData(string city);
    public string ProvideSolarData(GeoData geoData);
}