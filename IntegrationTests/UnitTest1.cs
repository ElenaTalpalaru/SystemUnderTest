using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.Json;
using SystemUderTest;

namespace IntegrationTests
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<ITestMarker>>
    {

        private readonly HttpClient client = new();

        public UnitTest1(WebApplicationFactory<ITestMarker> factory)
        {
            // Arrange / Given
            if (Config.RunAgainstDeployedEnvironment)
            {
                client = new HttpClient()
                {
                    BaseAddress = new Uri("https://localhost:7119/")
                };
            }
            else
                client = factory.CreateClient();


        }
        
        [Fact]
        public void Get_Weatherforecast_Returns_WeatherForecast_List()
        {
            // Act / When
            var result = client.GetAsync("weatherforecast").Result;

            // Assert / Then
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            //var jsonAsString = result.Content.ReadAsStringAsync().Result;
            //var jsonAsModel = JsonSerializer.Deserialize<WeatherForecast>(jsonAsString);

            //jsonAsModel.Summary.Should().Be("Hello");
        }
    }
}