using GymSystem.DAL.Date.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Date.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.CategoryName)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Category { Id = 1 , CategoryName = "Cardio"},
                new Category { Id = 2 , CategoryName = "Strength"},
                new Category { Id = 5 , CategoryName = "Yoga"},
                new Category { Id = 3 , CategoryName = "Boxing"},
                new Category { Id = 4 , CategoryName = "CrossFit"}
            );
        }
    }
}
