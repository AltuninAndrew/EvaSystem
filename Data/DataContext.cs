using EvaSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvaSystem.Data
{
    public class DataContext : IdentityDbContext<UserModel>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<InterectedUserModel> interectedUsers { get; set; }
        public DbSet<PositionModel> Positions { get; set; }
        public DbSet<EvaluationСriterionModel> Criterions { get; set; }
        public DbSet<CriterionToPositionModel> CriterionsToPosition { get; set; }
        public DbSet<ScoreModel> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CriterionToPositionModel>()
                .HasAlternateKey(c => new { c.CriterionName,c.PositionId});
        }
    }
}
