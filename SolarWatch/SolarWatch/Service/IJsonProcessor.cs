using SolarWatch.Model;

namespace SolarWatch.Service;

public interface IJsonProcessor
{
    public City ProcessGeoData(string data);
    public SunsetTimes ProcessSolarData(string data, string city);

}