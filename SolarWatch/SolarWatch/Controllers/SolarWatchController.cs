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
    private readonly SolarWatchApiContext _dbContext;

    public SolarWatchController(ILogger<SolarWatchController> logger, IDataProvider dataProvider,
        IJsonProcessor jsonProcessor, SolarWatchApiContext dbContext)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _dataProvider = dataProvider;
        _dbContext = dbContext;
    }

    [HttpGet("GetSolarData"), Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<SunsetTimes>> GetSolarData([Required] string cityName)
    {
        var city = _dbContext.Cities.FirstOrDefault(c => c.Name == cityName);
        if (city == null)
        {
            string unprocessedCityData = await _dataProvider.ProvideGeoData(cityName);
            var cityData = _jsonProcessor.ProcessGeoData(unprocessedCityData);
            if (cityData is { Lat: null, Lon: null })
            {
                return NotFound("Solar data not found in SolarWatch!");
            }

            _dbContext.Add(cityData);

            string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(cityData);
            var sunsetData = _jsonProcessor.ProcessSolarData(unprocessedSunsetTimes, cityData.Name);

            _dbContext.Add(sunsetData);
            await _dbContext.SaveChangesAsync();

            return Ok(sunsetData);
        }

        try
        {
            string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(city);
            var sunsetData = _jsonProcessor.ProcessSolarData(unprocessedSunsetTimes, city.Name);
            _dbContext.Add(sunsetData);
            await _dbContext.SaveChangesAsync();

            return Ok(sunsetData);
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return BadRequest("Error getting solar data");
        }
    }

    [HttpGet("GetSolarDataRange"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<SunsetTimes[]>> GetSolarDataRange([Required] string cityName, [Required] string startDate, [Required] string endDate)
    {
        DateTime startDateParsed;
        DateTime endDateParsed;
        if (DateTime.TryParse(startDate,out startDateParsed) && DateTime.TryParse(endDate, out endDateParsed))
        {
            var city = _dbContext.Cities.FirstOrDefault(c => c.Name == cityName);
            if (city == null)
            {
                string unprocessedCityData = await _dataProvider.ProvideGeoData(cityName);
                var cityData = _jsonProcessor.ProcessGeoData(unprocessedCityData);
                if (cityData is { Lat: null, Lon: null })
                {
                    return NotFound("Solar data not found in SolarWatch!");
                }

                _dbContext.Add(cityData);

                string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(cityData, startDateParsed, endDateParsed);
                var sunsetData = _jsonProcessor.ProcessMultipleSolarData(unprocessedSunsetTimes, cityData.Name);

                foreach (var sunset in sunsetData)
                {
                    _dbContext.Add(sunset);
                }
                
                await _dbContext.SaveChangesAsync();

                return Ok(sunsetData);
            }
            try
            {
                string unprocessedSunsetTimes = await _dataProvider.ProvideSolarData(city, startDateParsed, endDateParsed);
                var sunsetData = _jsonProcessor.ProcessMultipleSolarData(unprocessedSunsetTimes, city.Name);

                foreach (var sunset in sunsetData)
                {
                    _dbContext.Add(sunset);
                }
                
                await _dbContext.SaveChangesAsync();

                return Ok(sunsetData);
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error getting solar data");
                return BadRequest("Error getting solar data");
            }
        }

        return BadRequest("Date was not in the right format!");
    }

    [HttpPost("PostSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SunsetTimes>> PostSolarData([Required]string cityName, [Required]string sunset, [Required]string sunrise)
    {
        var sunsetTimes = _dbContext.SunsetTimes.FirstOrDefault(c => c.Name == cityName);
        if (sunsetTimes != null)
        {
            return Conflict("City already exists!");
        }
        try
        {
            _dbContext.Add(new SunsetTimes
            {
                Name = cityName,
                Sunrise = sunrise,
                Sunset = sunset
            });
            await _dbContext.SaveChangesAsync();

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
        var sunsetTimes = _dbContext.SunsetTimes.FirstOrDefault(c => c.Name == cityName);
        if (sunsetTimes == null)
        {
            return NotFound($"City {cityName} not found in the database!");
        }
        try
        {
            sunsetTimes.Sunset = sunset;
            sunsetTimes.Sunrise = sunrise;
            await _dbContext.SaveChangesAsync();
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
        var sunsetTimes = _dbContext.SunsetTimes.FirstOrDefault(c => c.Name == cityName);
        if (sunsetTimes == null)
        {
            return NotFound($"{cityName} does not exist in the database!");
        }

        try
        {
            _dbContext.Remove(sunsetTimes);
            await _dbContext.SaveChangesAsync();
            return Ok("Data deleted!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}