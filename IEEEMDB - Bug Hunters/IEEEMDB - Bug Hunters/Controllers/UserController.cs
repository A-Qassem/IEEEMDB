using System.Linq;
using System.Threading.Tasks;
using IEEEMDB___Bug_Hunters.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IEEEMDB___Bug_Hunters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db;
        }

        public record RegisterRequest(string username, string email, string password);
        public record LoginRequest(string email, string password);

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
            {
                return BadRequest("username, email and password are required");
            }

            var exists = await _db.Users.AnyAsync(u => u.Email == request.email);
            if (exists)
            {
                return Conflict("Email already registered");
            }

            var user = new User
            {
                Username = request.username,
                Email = request.email,
                Password = request.password
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Profile), new { userId = user.Id }, new { user.Id, user.Username, user.Email });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
            {
                return BadRequest("email and password are required");
            }

            var valid = await _db.Users.AnyAsync(u => u.Email == request.email && u.Password == request.password);
            if (!valid)
            {
                return Unauthorized();
            }
            return Ok(new { success = true });
        }

        [HttpGet("Profile/{userId:int}")]
        public async Task<IActionResult> Profile([FromRoute] int userId)
        {
            var user = await _db.Users
                .Include(u => u.Reviews)
                .Include(u => u.WatchHistory)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var response = new
            {
                user.Id,
                user.Username,
                user.Email,
                reviews = user.Reviews.Select(r => new { r.Id, r.MovieId, r.Rating, r.Comment }),
                ratings = user.Reviews.Select(r => r.Rating).ToList(),
                watchHistory = user.WatchHistory.Select(m => new { m.Id, m.Title })
            };
            return Ok(response);
        }
    }
}



