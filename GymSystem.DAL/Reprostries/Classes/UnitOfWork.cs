using GymSystem.DAL.Date.DbContexts;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Reprostries.Classes
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly GymDbContext dbContext;
        private readonly Dictionary<string, object> _reprostries= [];

        public UnitOfWork( GymDbContext dbContext , ISessionRepository sessionRepository)
        {
            this.dbContext = dbContext;
            SessionRepository=sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : BaseU, new()
        {
            var typeName = typeof(TEntity).Name;

            if (_reprostries.TryGetValue(typeName , out object? value) )
            {
                return (IGenericRepository<TEntity>) value;   
            }
            else
            {
                var repo = new GenericRepository<TEntity> (dbContext);
                _reprostries[typeName] = repo;
                return repo;
            }
        }

        public async Task<int> SaveChanges(CancellationToken ct = default)
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
