using Newtonsoft.Json;
using SchoolSystem0.Server.Models;
using System.Net.Http.Headers;

namespace SchoolSystem0.Server.Controllers
{
    public class DistanceService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DistanceService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // Method to get route information with waypoints
        public async Task<string> GetRouteWithWaypointsAsync(double schoolLat, double schoolLon, string waypoints)
        {
            var apiKey = _configuration["Neshan:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API Key is missing.");
            }

            // Construct the Neshan API URL with waypoints
            var requestUri = $"https://api.neshan.org/v4/direction?type=car" +
                $"&origin={schoolLat},{schoolLon}" +
                $"&destination={schoolLat},{schoolLon}" +  // Assuming schoolLat/Lon is both origin & destination (round trip)
                $"&waypoints={waypoints}" +
                $"&avoidTrafficZone=false&avoidOddEvenZone=false&alternative=false&bearing=";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("Api-Key", apiKey); // Add API Key to header

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Neshan API request failed with status code {response.StatusCode}: {errorContent}");
                }

                // Return the raw JSON response as a string
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get route from Neshan API: {ex.Message}");
                throw new Exception("Failed to get route from Neshan API", ex);
            }
        }
    }
}

