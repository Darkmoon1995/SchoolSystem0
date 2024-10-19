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

        public async Task<double> GetDistanceAsync(double studentLat, double studentLon, double schoolLat, double schoolLon)
        {
            var apiKey = _configuration["Neshan:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API Key is missing.");
            }

            var requestUri = $"https://api.neshan.org/v4/direction?type=car&origin={studentLat},{studentLon}&destination={schoolLat},{schoolLon}&avoidTrafficZone=false&avoidOddEvenZone=false&alternative=false&bearing=";

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

                var responseContent = await response.Content.ReadAsStringAsync();
                var directionResponse = JsonConvert.DeserializeObject<NeshanDirectionResponse>(responseContent);

                return directionResponse?.routes?.FirstOrDefault()?.legs?.FirstOrDefault()?.distance?.value ?? 0.0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get distance from Neshan API: {ex.Message}");
                throw new Exception("Failed to get distance from Neshan API", ex);
            }
        }
    }
}

