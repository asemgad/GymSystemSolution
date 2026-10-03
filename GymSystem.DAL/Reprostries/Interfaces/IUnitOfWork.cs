using GymSystem.DAL.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Reprostries.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : BaseU ,  new();

       public  Task<int> SaveChanges(CancellationToken ct = default);

        public ISessionRepository SessionRepository { get; } 
    }
}
