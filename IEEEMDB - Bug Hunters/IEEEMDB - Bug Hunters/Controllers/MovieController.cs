using System.Linq;
using System.Threading.Tasks;
using IEEEMDB___Bug_Hunters.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IEEEMDB___Bug_Hunters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MovieController(AppDbContext db)
        {
            _db = db;
        }

        public record MovieMutateRequest(string title, string? description, string video);
        public record ReviewRequest(int movieId, int userId, int rating, string? comment);

        // Admin only (simplified - no auth gate for brevity)
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] MovieMutateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.title) || string.IsNullOrWhiteSpace(request.video))
            {
                return BadRequest("title and video are required");
            }
            var movie = new Movie { Title = request.title, Description = request.description, VideoUrl = request.video };
            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Search), new { title = request.title }, new { movie.Id, movie.Title });
        }

        // Admin only
        [HttpPut("Manage/{id:int}")]
        public async Task<IActionResult> Manage([FromRoute] int id, [FromBody] MovieMutateRequest request)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(request.title)) movie.Title = request.title;
            movie.Description = request.description;
            if (!string.IsNullOrWhiteSpace(request.video)) movie.VideoUrl = request.video;
            await _db.SaveChangesAsync();
            return Ok(new { movie.Id, movie.Title });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string? title)
        {
            var query = _db.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(m => m.Title.Contains(title));
            }
            var results = await query
                .Select(m => new { m.Id, m.Title, m.Description, m.VideoUrl })
                .ToListAsync();
            return Ok(results);
        }

        [HttpPost("Review")]
        public async Task<IActionResult> Review([FromBody] ReviewRequest request)
        {
            if (request.rating < 1 || request.rating > 5)
            {
                return BadRequest("rating must be 1-5");
            }

            var userExists = await _db.Users.AnyAsync(u => u.Id == request.userId);
            var movieExists = await _db.Movies.AnyAsync(m => m.Id == request.movieId);
            if (!userExists || !movieExists) return BadRequest("invalid userId or movieId");

            var review = new Review
            {
                MovieId = request.movieId,
                UserId = request.userId,
                Rating = request.rating,
                Comment = request.comment
            };
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            return Ok(new { review.Id });
        }

        [HttpGet("Review/{movieId:int}")]
        public async Task<IActionResult> Reviews([FromRoute] int movieId)
        {
            var reviews = await _db.Reviews
                .Where(r => r.MovieId == movieId)
                .Select(r => new { r.Id, r.UserId, r.Rating, r.Comment })
                .ToListAsync();
            return Ok(reviews);
        }
    }
}



