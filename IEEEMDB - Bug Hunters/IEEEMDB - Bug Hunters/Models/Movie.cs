using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IEEEMDB___Bug_Hunters.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        public List<Review> Reviews { get; set; } = new List<Review>();

        public List<UserWatchHistory> WatchedByUsers { get; set; } = new List<UserWatchHistory>();
        public List<UserMovieList> ListedByUsers { get; set; } = new List<UserMovieList>();
    }
}


