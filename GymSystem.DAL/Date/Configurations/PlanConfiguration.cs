using GymSystem.DAL.Date.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Date.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
          builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .HasMaxLength(50)
                .HasColumnType("varchar");

            builder.Property(b => b.Description)
                .HasMaxLength(200);

            builder.Property(b => b.Price)
                .HasPrecision(18, 2);   

            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_Plan_Price", "[Price] >= 0");
                tb.HasCheckConstraint("CK_Plan_DurationDays", "[DurationDays] between 1 and 365");
            });
        }
    }
}
