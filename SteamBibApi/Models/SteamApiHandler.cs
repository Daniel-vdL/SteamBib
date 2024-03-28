using System.Text.Json;

namespace SteamBibApi.Models
{
    public class SteamApiHandler
    {
        private readonly HttpClient _client;

        public SteamApiHandler()
        {
            _client = new HttpClient();
            GetAppsAsync().Wait();
        }

        public async Task<RootObject> GetAppsAsync()
        {
            var steam = System.Configuration.ConfigurationManager.AppSettings["steamKey"];
            string url = $"https://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=json&key={steam}";

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<RootObject>(content, options);

        }
    }
}
