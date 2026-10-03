using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService sessionService)
        { 
            _sessionService = sessionService;
        }

        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDown(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDown(ct), "Id", "CategoryName");
        }
        public async Task<IActionResult> Index()
        {
            var sessions = await _sessionService.GetAllSessionsAsync();
            return View(sessions);
        }

        #region Create


        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownAsync(ct);

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateSessionModel model, CancellationToken ct)
        {
            if (ModelState.IsValid) {
                await PopulateDropDownAsync(ct);

                return View(model); } ;

                var result = await _sessionService.CreateSessionAsync(model, ct);

            if (result)
            {
                TempData["SucessMessage"] = "Session Created";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"]= "Session Failed";
            await PopulateDropDownAsync(ct);
 
            return View(model);

        }

        #endregion


        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAysnc(id, ct);
            if (result.success) return View(result.value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdate(id, ct);
            if (result.success)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDown(ct), "Id", "Name");
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDown(ct), "Id", "Name");
                return View(model);
            }
            var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.succes)
            {
                TempData["SuccessMessage"] = "Session Update";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDown(ct), "Id", "Name");
                return View(model);
            }
        }








    }
}
