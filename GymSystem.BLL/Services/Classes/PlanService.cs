using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public PlanService(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansasync( CancellationToken cancellationToken = default)
        {
            var plans = await _planRepository.GetAllAsync( cancellationToken : cancellationToken);

            if (!plans.Any())
                return [];
           
            var plansList = plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DurationDays = p.DurationDays,
                Description = p.Description,
                IsActive = p.IsActive
            });

            return plansList;
        }


    }
}
