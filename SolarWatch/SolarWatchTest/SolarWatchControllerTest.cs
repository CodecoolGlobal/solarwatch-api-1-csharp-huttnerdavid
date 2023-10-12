using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Service;
using SolarWatch = SolarWatch.SolarWatch;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private DataProvider _dataProviderMock;
    private JsonProcessor _jsonProcessorMock;
    private SolarWatchController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _dataProviderMock = new DataProvider();
        _jsonProcessorMock = new JsonProcessor();
        _controller = new SolarWatchController(_loggerMock.Object, _dataProviderMock, _jsonProcessorMock);
    }

    [Test]
    public void GetSolarDataReturnsNotFoundIfDataProviderFails()
    {
        var result = _controller.GetSolarData("DAGFASGA");
        Assert.That(result.Result, Is.InstanceOf(typeof(NotFoundObjectResult)));
    }

    [Test]
    public void GetSolarDataReturnsOkIfItGetsCorrectCity()
    {
        var result = _controller.GetSolarData("Budapest");
        Assert.That(result.Result, Is.InstanceOf(typeof(OkObjectResult)));
    }
}
    