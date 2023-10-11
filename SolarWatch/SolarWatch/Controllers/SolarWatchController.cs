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
    public SolarWatch GetSolarData([Required]string city)
    {
        string unprocessedGeoData = _dataProvider.ProvideGeoData(city);
        GeoData processedGeoData = _jsonProcessor.ProcessGeoData(unprocessedGeoData);
        if (processedGeoData is { Lat: 0, Lon: 0 })
        {
            return new SolarWatch
            {
                Date = DateTime.Now,
                City = "City not found!"
            };
        }

        string unprocessedSolarData = _dataProvider.ProvideSolarData(processedGeoData);
        SolarWatch processedSolarData = _jsonProcessor.ProcessSolarData(unprocessedSolarData, processedGeoData.City);
        
        return processedSolarData;
    }
    
}