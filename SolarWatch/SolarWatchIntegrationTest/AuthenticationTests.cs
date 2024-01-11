using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SolarWatch.Contracts;
using SolarWatch.Model;
using SolarWatch.Service.Authentication;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SolarWatchIntegrationTest;

public class AuthenticationTests : IDisposable
{
    private SolarWatchFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _output;
    
    public AuthenticationTests()
    {
        _factory = new SolarWatchFactory();
        _client = _factory.CreateClient();
        _output = new TestOutputHelper();
    }
    
    [Fact]
    public async Task TestRegistration()
    {
        var loginRequest = new AuthRequest("user1@email.com", "password1");
        
        var loginResponse = await _client.PostAsync("/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest), 
                Encoding.UTF8, "application/json"));

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        
        if (loginResponse.StatusCode == HttpStatusCode.OK)
        {
            Assert.NotNull(authResponse.Token);
            Assert.Equal("user1@email.com", authResponse.Email);
            Assert.Equal("user1", authResponse.UserName);
        }
        else
        {
            var registrationRequest = 
                new RegistrationRequest("user2@email.com", "user2", "password2");

            var registrationResponse = await _client.PostAsync("/Auth/Register",
                new StringContent(JsonConvert.SerializeObject(registrationRequest), 
                    Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, registrationResponse.StatusCode);
        }
    }
    
    [Fact]
    public async Task TestNoAdminAuthorization()
    {
        var registrationRequest = 
            new RegistrationRequest("user1@email.com", "user1", "password1");
        
        var registrationResponse = await _client.PostAsync("/Auth/Register",
            new StringContent(JsonConvert.SerializeObject(registrationRequest), 
                Encoding.UTF8, "application/json"));
        
        var loginRequest = new AuthRequest("user1@email.com", "password1");
        
        var loginResponse = await _client.PostAsync("/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest), 
                Encoding.UTF8, "application/json"));
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        Assert.NotNull(authResponse.Token);
        var userToken = authResponse.Token;
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);

        var sunsetTimes = new SunsetTimes();
        sunsetTimes.Sunset = "8:00:00";
        sunsetTimes.Name = "Buda";
        sunsetTimes.Sunrise = "9:00:00";

        var setRoleResponse = await _client.PostAsync("/PostSolarData", new StringContent(JsonConvert.SerializeObject(sunsetTimes), Encoding.UTF8, "application/json"));
        
        Assert.Equal(HttpStatusCode.Forbidden, setRoleResponse.StatusCode);
    }

    [Fact]
    public async Task TestNoAuthorization()
    {
        var setRoleResponse = await _client.GetAsync("/GetSolarData?cityName=Budapest");
        
        Assert.Equal(HttpStatusCode.Unauthorized, setRoleResponse.StatusCode);
    }
    
    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}