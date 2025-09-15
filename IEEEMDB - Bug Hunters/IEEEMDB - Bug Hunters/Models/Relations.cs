using System.ComponentModel.DataAnnotations.Schema;

namespace IEEEMDB___Bug_Hunters.Models
{
    public class UserWatchHistory
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }

    public class UserMovieList
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}



