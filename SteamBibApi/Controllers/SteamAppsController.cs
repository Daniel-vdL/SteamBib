using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamBibApi.Models;

namespace SteamBibApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SteamAppsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SteamAppsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SteamApps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SteamAppDto>>> GetSteamApps()
        {
            var steamApps = await _context.SteamApps
                  .ToListAsync();

            var steamAppDtos = new List<SteamAppDto>();

            foreach (var steamApp in steamApps)
            {
                steamAppDtos.Add(new SteamAppDto
                {
                    Id = steamApp.Id,
                    Name = steamApp.Name,
                    Appid = steamApp.Appid
                });
            }

            return steamAppDtos;
        }

        // DELETE: api/SteamApps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSteamApp(int id)
        {
            if (_context.SteamApps == null)
            {
                return NotFound();
            }
            var steamApp = await _context.SteamApps.FindAsync(id);
            if (steamApp == null)
            {
                return NotFound();
            }

            _context.SteamApps.Remove(steamApp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SteamAppExists(int id)
        {
            return (_context.SteamApps?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static bool _isDatabaseFilled = false;

        [HttpPost("FillSteamApps")]
        public async Task<IActionResult> FillSteamApps()
        {
            try
            {
                if (_isDatabaseFilled)
                {
                    return BadRequest("Database has already been filled.");
                }

                var steamApiHandler = new SteamApiHandler();
                var appList = await steamApiHandler.GetAppsAsync();

                foreach (var apps in appList.Applist.Apps)
                {
                    var steamApp = new SteamApp
                    {
                        Appid = apps.Appid,
                        Name = apps.Name
                    };

                    _context.SteamApps.Add(steamApp);
                }

                await _context.SaveChangesAsync();

                _isDatabaseFilled = true;

                return Ok("Steam apps data filled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
