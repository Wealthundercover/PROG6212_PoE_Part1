using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public AuthController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            // Fallback: If RoleId wasn't passed or is 0, default to Participant (ID 2)
            if (user.RoleId == 0)
            {
                user.RoleId = 2;
            }

            // Verify the role actually exists in the database before saving
            bool roleExists = await _context.Roles.AnyAsync(r => r.RoleId == user.RoleId);
            if (!roleExists)
            {
                return BadRequest(new { message = $"The provided RoleId ({user.RoleId}) does not exist in the database." });
            }

            // Simple registration endpoint for PoE part requirements
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.UserId }, user);
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.PasswordHash);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(user);
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}