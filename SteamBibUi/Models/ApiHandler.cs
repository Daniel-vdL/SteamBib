using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SteamBibUi.Models
{
    internal class ApiHandler
    {
        private readonly HttpClient _client;

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
    }
}
