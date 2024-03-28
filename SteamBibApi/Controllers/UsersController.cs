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
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
        {
            var users = await _context.Users
                .ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                });
            }

            return userDtos;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserLoginDto userLoginDto)
        {
            if (id != userLoginDto.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Username = userLoginDto.Username;
            user.Password = SecureHasher.Hash(userLoginDto.Password);

            try
            {
                await _context.SaveChangesAsync();
                userLoginDto.Password = "";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserLoginDto userLoginDto)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'AppDbContext.User'  is null.");
            }

            var user = new User()
            {
                Username = userLoginDto.Username,
                Password = SecureHasher.Hash(userLoginDto.Password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userLoginDto.Id = user.Id;

            userLoginDto.Password = "";
            return CreatedAtAction("GetUser", new { id = user.Id }, userLoginDto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users/Login
        [HttpPost("Login")]
        public async Task<ActionResult<User>> PostUserLogin(UserLoginDto userLoginDto)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'AppDbContext.User'  is null.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userLoginDto.Username);

            if (user == null)
            {
                return NotFound();
            }

            else if (VerifyPassword(userLoginDto.Password, user.Password))
            {
                userLoginDto.Id = user.Id;
                userLoginDto.StatusId = user.StatusId;
                userLoginDto.Password = "";
                return CreatedAtAction("PostUserLogin", userLoginDto);
            }

            return NotFound();

        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return SecureHasher.Verify(password, hashedPassword);
        }
    }
}
