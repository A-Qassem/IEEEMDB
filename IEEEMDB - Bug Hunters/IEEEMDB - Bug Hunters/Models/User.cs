using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IEEEMDB___Bug_Hunters.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Password { get; set; } = string.Empty;

        public List<Review> Reviews { get; set; } = new List<Review>();

        public List<Movie> WatchHistory { get; set; } = new List<Movie>();

        public List<Movie> MovieLists { get; set; } = new List<Movie>();

        public List<int> Ratings { get; set; } = new List<int>();
    }
}


