using AutoMapper;
using GymManagement.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        //private readonly IGenericRepository<Plan> _planRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansasync(CancellationToken cancellationToken = default)
        {
            var plans = await unitOfWork.GetGenericRepository<Plan>().GetAllAsync(cancellationToken: cancellationToken);

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

        public async Task<PlanViewModel?> GetPlanByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var plan = await unitOfWork.GetGenericRepository<Plan>().GetByIdAsync(id, cancellationToken);
            return plan is null ? null : mapper.Map<PlanViewModel>(plan);
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdate(int id, CancellationToken cancellationToken = default)
        {
            var plan = await unitOfWork.GetGenericRepository<Plan>().GetByIdAsync(id, cancellationToken);


            if (plan == null || !plan.IsActive)
                return null;

            var updatePlanViewModel = mapper.Map<UpdatePlanViewModel>(plan);
            return updatePlanViewModel;
        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel updatePlanViewModel, CancellationToken cancellationToken = default)
        {
            var plan = await unitOfWork.GetGenericRepository<Plan>().GetByIdAsync(id, cancellationToken);

            if (plan == null || !plan.IsActive)
                return Result.NotFound("Plan not found or inactive.");

            if((await unitOfWork.GetGenericRepository<MemberShip>().GetAllAsync( false , cancellationToken)).Any(ms => ms.PlanId == id && ms.EndDate > DateTime.Now))
                return Result.Fail("Cannot update plan. There are active memberships associated with this plan.", ResultKind.Conflict);

            var udpatedPlan = mapper.Map(updatePlanViewModel, plan);
            unitOfWork.GetGenericRepository<Plan>().Update(udpatedPlan);
            var result = await unitOfWork.SaveChanges(cancellationToken);

            if (result > 0)
                return Result.Ok();
            else
                return Result.Fail("Failed to update plan.");
        }
    }
}
