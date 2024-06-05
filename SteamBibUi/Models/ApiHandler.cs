using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SteamBibUi.Models
{
    internal class ApiHandler
    {
        private List<AppDetails> AppDetailsList;
        public AppData appDetails { get; set; }
        private readonly HttpClient _client;

        public static List<AppDetails> steamAppDetails = new List<AppDetails>();

        public ApiHandler()
        {
            _client = new HttpClient();
        }

        public async Task<List<SteamApp>> GetSteamAppsAsync()
        {
            string url = "https://localhost:7099/api/SteamApps";

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<SteamApp>>(content, options);
        }

        public async Task FillSteamAppsAsync()
        {
            string url = "https://localhost:7099/api/SteamApps/FillSteamApps";
            var response = await _client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> DeleteSteamAppAsync(int id)
        {
            try
            {
                string url = $"https://localhost:7099/api/SteamApps/{id}";
                var response = await _client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error deleting Steam app: {ex.Message}");
                return false;
            }
        }
    }
}
