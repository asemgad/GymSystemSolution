using GymSystem.DAL.Date.DbContexts;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Reprostries.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;
        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<Session> query = dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }
        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId);
        }

        public async Task<Session?> GetSessionByIdWithTrainerAndCategoryAsync(int sesstionId, CancellationToken ct)
        {
            return await dbContext.Sessions.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == sesstionId);
        }
    }
}
