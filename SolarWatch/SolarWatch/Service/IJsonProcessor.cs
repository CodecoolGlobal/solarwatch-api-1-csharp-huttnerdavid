
namespace SolarWatch.Service;

public interface IJsonProcessor
{
    public GeoData ProcessGeoData(string data);
    public SolarWatch ProcessSolarData(string data, string city);

}