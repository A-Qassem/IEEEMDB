using Microsoft.EntityFrameworkCore;

namespace IEEEMDB___Bug_Hunters.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<UserWatchHistory> UserWatchHistories => Set<UserWatchHistory>();
        public DbSet<UserMovieList> UserMovieLists => Set<UserMovieList>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserWatchHistory>().HasKey(x => new { x.UserId, x.MovieId });
            modelBuilder.Entity<UserWatchHistory>()
                .HasOne(x => x.User)
                .WithMany(u => u.WatchHistory)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserWatchHistory>()
                .HasOne(x => x.Movie)
                .WithMany(m => m.WatchedByUsers)
                .HasForeignKey(x => x.MovieId);

            modelBuilder.Entity<UserMovieList>().HasKey(x => new { x.UserId, x.MovieId });
            modelBuilder.Entity<UserMovieList>()
                .HasOne(x => x.User)
                .WithMany(u => u.MovieLists)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<UserMovieList>()
                .HasOne(x => x.Movie)
                .WithMany(m => m.ListedByUsers)
                .HasForeignKey(x => x.MovieId);
        }
    }
}



