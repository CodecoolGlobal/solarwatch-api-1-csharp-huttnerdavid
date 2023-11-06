using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Data;
using SolarWatch.Service;

namespace SolarWatch.Controllers;

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly IDataProvider _dataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SolarWatchController(ILogger<SolarWatchController> logger, IDataProvider dataProvider,
        IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _dataProvider = dataProvider;
    }

    [HttpGet("GetSolarData")]
    public async Task<ActionResult<SolarWatch>> GetSolarData([Required] string cityName)
    {
        await using var dbContext = new SolarWatchApiContext();
        var city = dbContext.Cities.FirstOrDefault(c => c.Name == cityName);
        if (city == null)
        {
            string unprocessedCityData = await _dataProvider.ProvideGeoData(cityName);
            var cityData = _jsonProcessor.ProcessGeoData(unprocessedCityData);
            if (cityData is { Lat: null, Lon: null })
            {
                return NotFound("Solar data not found in SolarWatch!");
            }

            dbContext.Add(cityData);
            await dbContext.SaveChangesAsync();

            string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(cityData);
            var sunsetData = _jsonProcessor.ProcessSolarData(unprocessedSunsetTimes, cityData.Name);

            return Ok(sunsetData);
        }

        try
        {
            string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(city);
            var sunsetData = _jsonProcessor.ProcessSolarData(unprocessedSunsetTimes, city.Name);

            return Ok(sunsetData);
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return NotFound("Error getting solar data");
        }
    }
}