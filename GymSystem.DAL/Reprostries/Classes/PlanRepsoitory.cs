using GymSystem.DAL.Date.DbContexts;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Reprostries.Classes
{
      public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;

       public PlanRepository(GymDbContext dbContext) // new GymDvcontxt automtically 
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plans.Add(plan); // async not needed here because we are not saving changes yet
            return await dbContext.SaveChangesAsync(ct); // async save changes

        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
           dbContext.Plans.Remove(plan);
            return await dbContext.SaveChangesAsync(ct);// Return the deleted plan after saving changes
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Plan> query = tracking ? dbContext.Plans : dbContext.Plans.AsNoTracking();

            return await query.ToListAsync(cancellationToken);
                }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
          return await  dbContext.Plans.FindAsync(id ,ct);
        }

        public Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plans.Update(plan);
            return dbContext.SaveChangesAsync(ct);
        }
    }
}
