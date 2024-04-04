using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SteamBibUi.Models
{
    internal class SteamApp
    {
        public int Id { get; set; }
        public int Appid { get; set; }
        public string Name { get; set; }

        public string ImageUrl => $"https://cdn.akamai.steamstatic.com/steam/apps/{Id}/header.jpg";

        public List<string> Genres { get; set; }
    }

    internal class GetAppDetails
    {
        private readonly HttpClient _client;

        public GetAppDetails()
        {
            _client = new HttpClient();
        }

        public async Task PopulateGenresAsync(SteamApp app)
        {
            string url = $"https://localhost:7099/api/AppDetails/{app.Appid}";

            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            try
            {
                var receivedContent = JsonSerializer.Deserialize<Dictionary<string, AppDetails>>(content, options);

                if (receivedContent != null && receivedContent.ContainsKey(app.Appid.ToString()))
                {
                    var appDetail = receivedContent[app.Appid.ToString()];
                    if (appDetail.Data != null && appDetail.Data.Genres != null)
                    {
                        app.Genres = appDetail.Data.Genres.Select(g => g.Description).ToList();
                    }
                }
            }
            catch (JsonException ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                // You can choose to continue processing here or just skip the current item
            }
        }
    }
}
