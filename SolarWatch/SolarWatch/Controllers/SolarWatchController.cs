using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Data;
using SolarWatch.Model;
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

    [HttpGet("GetSolarData"), Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<SunsetTimes>> GetSolarData([Required] string cityName)
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

            string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(cityData);
            var sunsetData = _jsonProcessor.ProcessSolarData(unprocessedSunsetTimes, cityData.Name);

            dbContext.Add(sunsetData);
            await dbContext.SaveChangesAsync();

            return Ok(sunsetData);
        }

        try
        {
            string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(city);
            var sunsetData = _jsonProcessor.ProcessSolarData(unprocessedSunsetTimes, city.Name);
            dbContext.Add(sunsetData);
            await dbContext.SaveChangesAsync();

            return Ok(sunsetData);
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return NotFound("Error getting solar data");
        }
    }

    [HttpPost("PostSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SunsetTimes>> PostSolarData([Required]string cityName, [Required]string sunset, [Required]string sunrise)
    {
        await using var dbContext = new SolarWatchApiContext();
        var sunsetTimes = dbContext.SunsetTimes.FirstOrDefault(c => c.Name == cityName);
        if (sunsetTimes != null)
        {
            return Conflict("City already exists!");
        }
        try
        {
            dbContext.Add(new SunsetTimes
            {
                Name = cityName,
                Sunrise = sunrise,
                Sunset = sunset
            });
            await dbContext.SaveChangesAsync();

            return Ok("SolarWatch data added!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something went wrong!");
            return BadRequest();
        }
    }

    [HttpPatch("UpdateSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SunsetTimes>> UpdateSolarData([Required]string cityName, [Required]string sunset, [Required]string sunrise)
    {
        await using var dbContext = new SolarWatchApiContext();
        var sunsetTimes = dbContext.SunsetTimes.FirstOrDefault(c => c.Name == cityName);
        if (sunsetTimes == null)
        {
            return NotFound($"City {cityName} not found in the database!");
        }
        try
        {
            sunsetTimes.Sunset = sunset;
            sunsetTimes.Sunrise = sunrise;
            await dbContext.SaveChangesAsync();
            return Ok("Changes saved!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something went wrong!");
            return BadRequest();
        }
    }

    [HttpDelete("DeleteData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<string>> DeleteData([Required]string cityName)
    {
        await using var dbContext = new SolarWatchApiContext();
        var sunsetTimes = dbContext.SunsetTimes.FirstOrDefault(c => c.Name == cityName);
        if (sunsetTimes == null)
        {
            return NotFound($"{cityName} does not exist in the database!");
        }

        try
        {
            dbContext.Remove(sunsetTimes);
            await dbContext.SaveChangesAsync();
            return Ok("Data deleted!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}