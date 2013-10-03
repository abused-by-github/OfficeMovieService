using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.DataAccess
{
    class DataContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<Movie> Movies { get; private set; }
        public DbSet<Poll> Polls { get; private set; }

        public DataContext(string connectionString)
            : base(connectionString)
        {
            Users = Set<User>();
            Movies = Set<Movie>();
            Polls = Set<Poll>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Movie>();
            modelBuilder.Entity<Poll>().HasRequired(p => p.Owner);
            modelBuilder.Entity<PollCandidate>().HasMany(c => c.Voters).WithMany();
        }
    }
}
