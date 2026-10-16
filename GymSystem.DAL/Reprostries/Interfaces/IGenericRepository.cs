using GymSystem.DAL.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Reprostries.Interfaces
{
    public interface IGenericRepository <TEntity> where TEntity : BaseU , new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        void Add(TEntity entity);
       void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<bool> AnyAsync( Expression<Func<TEntity, bool>> predicate,CancellationToken ct = default);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> ? condition = null, CancellationToken ct = default);
    }
}
