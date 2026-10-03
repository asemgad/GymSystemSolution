using GymSystem.DAL.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Reprostries.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAllAsync( bool tracking = false , CancellationToken cancellationToken  = default);
        // the diffriece between Ienmurable with task is that Ienmurable is a collection of items that can be enumerated
        // , while task is an asynchronous operation that may or may not return a value. In this case, the GetAllAsync method returns a task that
        // will eventually yield an IEnumerable of Plan objects when the operation is complete.

        Task<Plan?> GetByIdAsync (int id,  CancellationToken ct = default);

        Task<int> AddAsync(Plan plan, CancellationToken ct = default);
        Task<int> UpdateAsync(Plan plan, CancellationToken ct = default);
        Task<int> DeleteAsync(Plan plan, CancellationToken ct = default);

    }
}
