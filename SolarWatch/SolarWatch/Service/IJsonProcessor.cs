using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    public City ProcessGeoData(string data);
    public SunsetTimes ProcessSolarData(string data, string city);
    public SunsetTimes[] ProcessMultipleSolarData(string data, string city);

}