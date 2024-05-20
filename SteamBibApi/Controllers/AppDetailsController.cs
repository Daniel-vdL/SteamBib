using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamBibApi.Models;

namespace SteamBibApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppDetailsController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly HttpClient _client;

        public AppDetailsController(AppDbContext context)
        {
            _context = context;
            _client = new HttpClient();
        }

        // GET api/<AppDetailsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            SteamAppDetailsScuffed data = _context.steamAppDetailsScuffeds.Find(id);
            if (data != null)
            {
                return data.Blob;
            }

            string url = $"https://store.steampowered.com/api/appdetails?appids={id}";

            var response = _client.GetAsync(url).Result;
            var blob = response.Content.ReadAsStringAsync().Result;
            if (blob != null)
            {
                _context.steamAppDetailsScuffeds.Add(new SteamAppDetailsScuffed() { Id = id, Blob = blob });
                _context.SaveChanges();
            }

            return blob;
        }
    }
}
