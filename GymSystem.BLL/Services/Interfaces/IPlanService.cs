using GymManagement.BLL.Common;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        public Task<IEnumerable<PlanViewModel>> GetAllPlansasync( CancellationToken cancellationToken = default);

        public Task<PlanViewModel?> GetPlanByIdAsync(int id, CancellationToken cancellationToken = default);

        public Task<UpdatePlanViewModel?> GetPlanToUpdate(int id, CancellationToken cancellationToken = default);

        public Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel updatePlanViewModel, CancellationToken cancellationToken = default);


    }
}
