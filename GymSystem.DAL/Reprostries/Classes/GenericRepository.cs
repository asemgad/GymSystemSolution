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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseU, new()
    {
        public GymDbContext dbContext { get; set; }
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            dbSet.Add(entity); // just locally and unit of work will handle it outside with database 
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void  Update(TEntity entity)
        {
            dbSet.Update(entity);
        }
        // will be void as all changes will be saved by unitofwork 
        //public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        //{
        //    dbSet.Add(entity);
        //    return await dbContext.SaveChangesAsync(ct);
        //}

        //public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
        //{
        //    dbSet.Remove(entity);
        //    return await dbContext.SaveChangesAsync(ct);
        //}

        //public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        //{
        //    dbSet.Update(entity);
        //    return await dbContext.SaveChangesAsync(ct);
        //}
        


        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking, CancellationToken cancellationToken = default)
        {
            return await dbSet.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await dbSet.FindAsync( id , ct);
        }



        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return dbSet.AsNoTracking().AnyAsync(predicate, ct);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
           IQueryable<TEntity> query = dbSet.AsNoTracking();
            return query.FirstOrDefaultAsync(predicate, ct);
        }

        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? condition = null, CancellationToken ct = default)
        {
            if (condition != null)
            {
                return dbSet.CountAsync(condition, ct);
            }
            else
            {
                return dbSet.CountAsync(ct);
            }
        }

        //public Task<IEnumerable<TEntity>> GetAllAsync(bool tracking, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
