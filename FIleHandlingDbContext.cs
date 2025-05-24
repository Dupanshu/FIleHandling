using Microsoft.EntityFrameworkCore;

namespace FIleHandling
{
	public class FIleHandlingDbContext : DbContext
	{
		public DbSet<Movie> Movies { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Rating> Ratings{ get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=DUPANSHUS-VIVOB\SQLEXPRESS;Database=FileHandlingDb; Integrated Security=True; TrustServerCertificate=True;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// movies table
			modelBuilder.Entity<Movie>()
				.HasKey(m => m.MovieId);


			// users table
			modelBuilder.Entity<User>()
				.HasKey(u => u.UserId);


			// ratings table
			modelBuilder.Entity<Rating>()
				.HasKey(r => r.UserId);

			// ratings table relationships
			modelBuilder.Entity<Rating>()
				.HasOne(r => r.Movie)
				.WithMany(r => r.Ratings)
				.HasForeignKey(m => m.MovieId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_Ratings_Movies");

			modelBuilder.Entity<Rating>()
				.HasOne(u => u.User)
				.WithMany(r => r.Ratings)
				.HasForeignKey(u => u.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_Ratings_Users");
		}
	}
}
