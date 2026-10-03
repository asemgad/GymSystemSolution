
using GymSystem.DAL.Date.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymSystem.DAL.Date.DbContexts
{
    public class GymDbContext : DbContext
    {

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }
        //override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // this will excute all confguratons 
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }

        }
}
