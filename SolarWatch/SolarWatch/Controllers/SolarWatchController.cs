using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Service;

namespace SolarWatch.Controllers;

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly IDataProvider _dataProvider;
    private readonly IJsonProcessor _jsonProcessor;

    public SolarWatchController(ILogger<SolarWatchController> logger, IDataProvider dataProvider, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _dataProvider = dataProvider;
    }

    [HttpGet("GetSolarData")]
    public ActionResult<SolarWatch> GetSolarData([Required]string city)
    {
        string unprocessedGeoData = _dataProvider.ProvideGeoData(city);
        GeoData processedGeoData = _jsonProcessor.ProcessGeoData(unprocessedGeoData);
        if (processedGeoData is { Lat: null, Lon: null })
        {
            return NotFound("Error getting solar data!");

        }

        string unprocessedSolarData = _dataProvider.ProvideSolarData(processedGeoData);
        SolarWatch processedSolarData = _jsonProcessor.ProcessSolarData(unprocessedSolarData, processedGeoData.City);
        
        return Ok(processedSolarData);
    }
    
}