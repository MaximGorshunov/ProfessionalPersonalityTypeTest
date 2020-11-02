using Microsoft.EntityFrameworkCore;
using Models; 

namespace DBRepository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<UserResult> UserResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<User>(eb =>
            {
                eb.Property(b => b.Login).HasMaxLength(20).IsRequired();
                eb.Property(b => b.IsAdmin).IsRequired();
                eb.Property(b => b.Email).HasMaxLength(30).IsRequired();
                eb.Property(b => b.Birthdate).IsRequired();
                eb.Property(b => b.IsMan).IsRequired();
                eb.Property(b => b.Password).HasMaxLength(30).IsRequired();
            });

            modelBuilder.Entity<User>()
                     .HasKey(k => k.Id);

            modelBuilder.Entity<User>().Ignore(b => b.Role);

            modelBuilder.Entity<UserResult>(eb =>
            {
                eb.Property(b => b.UserId).IsRequired();
                eb.Property(b => b.Date).IsRequired();
                eb.Property(b => b.R).IsRequired();
                eb.Property(b => b.I).IsRequired();
                eb.Property(b => b.A).IsRequired();
                eb.Property(b => b.S).IsRequired();
                eb.Property(b => b.E).IsRequired();
                eb.Property(b => b.C).IsRequired();
            });

            modelBuilder.Entity<UserResult>()
                     .HasKey(k => k.Id);

            modelBuilder.Entity<UserResult>()
                     .HasOne(e => e.User)
                     .WithOne(e => e.UserResult)
                     .HasForeignKey<UserResult>(k => k.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>(eb => { eb.Property(b => b.Number).IsRequired(); });

            modelBuilder.Entity<Question>()
                     .HasKey(k => k.Id);

            modelBuilder.Entity<Question>()
                 .HasOne(e => e.ProfessionFirst)
                 .WithMany(e => e.QuestionFirst)
                 .HasForeignKey(k => k.ProfessionIdFirst)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Question>()
                 .HasOne(e => e.ProfessionSecond)
                 .WithMany(e => e.QuestionSecond)
                 .HasForeignKey(k => k.ProfessionIdSecond)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Profession>(eb =>
            {
                eb.Property(b => b.Name).HasMaxLength(100).IsRequired();
                eb.Property(b => b.ProfType).IsRequired();
            });

            modelBuilder.Entity<Profession>()
                     .HasKey(k => k.Id);
        }
    }
}
