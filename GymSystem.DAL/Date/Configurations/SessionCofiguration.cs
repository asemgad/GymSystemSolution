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
    internal class SessionCofiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("Session_Capacity_Check", "Capacity Between 1 AND 20");
                tb.HasCheckConstraint("Session_EndDate_Check", "EndDate > StartDate");
            });
        }
    }
}
