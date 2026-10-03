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

    }
}
