using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccess.Migrations;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
{
    public class DataContext : DbContext, IUnitOfWork
    {
        static DataContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
        }

        public DbSet<User> Users { get; private set; }
        public DbSet<Movie> Movies { get; private set; }
        public DbSet<Poll> Polls { get; private set; }

        //TODO: can't use IoC because of http://entityframework.codeplex.com/workitem/1131
        //IDbContextFactory implementation didn't work on AppHarbor servers.
        //So, connection string is hardcoded.
        public DataContext() : base("ConnectionString") { }

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

            modelBuilder.Entity<User>().HasOptional(u => u.InvitedBy);
            modelBuilder.Entity<TmdbMovie>();
            modelBuilder.Entity<Movie>().HasOptional(m => m.TmdbMovie).WithMany().HasForeignKey(m => m.TmdbMovieId);

            modelBuilder.Entity<Poll>().HasRequired(p => p.Owner).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Vote>().HasKey(v => new { v.MovieId, v.PollId, v.UserId });

            modelBuilder.Entity<Vote>()
                .HasRequired(v => v.Movie)
                .WithMany(m => m.Votes)
                .HasForeignKey(v => v.MovieId);

            modelBuilder.Entity<Vote>()
                .HasRequired(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId);

            modelBuilder.Entity<Vote>()
                .HasRequired(v => v.Poll)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PollId);
        }

        public void Commit()
        {
            SaveChanges();
        }
    }
}
