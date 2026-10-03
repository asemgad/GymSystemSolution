using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Interfaces;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.Services.Classes;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {

        //private readonly DbContext.GymDbContext _context; // Private so no one can access it outside of this class and readonly so it can only be set in the constructor and not modified later 

        //private readonly IPlanRepository planRepository;
        //private readonly IGenericRepository<Plan> planRepository;
        private readonly IPlanService planService; // we can use IPlanService instead of IGenericRepository<Plan> to get all plans

        //public PlanController(IGenericRepository<Plan> planRepository) // new planreposrity automatically we replaced IPlanRepository
        //{
        //    this.planService = planRepository; // Dependency Injection: The planRepository is injected into the PlanController through its constructor.
        //                                          // This allows for better testability and separation of concerns,
        //                                          // as the controller does not need to know about the concrete implementation of the repository.
        //}

        // snd version of constructor using IPlanService instead of IGenericRepository<Plan>

        public PlanController(IPlanService planService)
        {
            this.planService = planService; // Dependency Injection: The planRepository is injected into the PlanController through its constructor.
                                            // This allows for better testability and separation of concerns,
                                            // as the controller does not need to know about the concrete implementation of the repository.
        }

        //public PlanController()
        //{
        //    _context = new GymDbContext();
        //}

        // Index action method to display the list of plans
        // Get baseUrl/Plan/Index
        public async Task<IActionResult> Index( CancellationToken ct) // we can access it in web browser by going to baseUrl/Plan/Index
        {
            //var plans = await _context.Plans.ToListAsync();
            var plans = await planService.GetAllPlansasync();
            return View(plans);
        }

        // Details action method to display the details of a specific plan
        // Get baseUrl/Plan/Details/{id}
        //public async Task<IActionResult >Details(int id , CancellationToken ct)
        //{
        //    //var plan = await _context.Plans.FindAsync(id);
        //    var plan = await planRepository.GetByIdAsync(id, ct: ct);
        //    if (plan == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(plan);
        //}
    }
}
