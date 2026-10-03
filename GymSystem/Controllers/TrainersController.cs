using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainersViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class TrainersController : Controller
    {
       public ITrainerService trainerService;
        public TrainersController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }
        // Get BaseUrl/Trainers/Index
        // index - get all trainers
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }

        // Get BaseUrl/Trainers/Details/{id}
        // TrainerDetails - get trainer by id

         [HttpGet]   

         public async Task<IActionResult> TrainerDetails(int id, CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerByIdAsync(id, ct);
            if (trainer == null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        #region Create Trainer

        // Get BaseUrl/Trainers/Create
        // Create - Show empty form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create),model);
            }
            var result = await trainerService.CreateTrainerAsync(model, ct);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
           
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Trainer

        [HttpGet]

        public async Task<IActionResult> EditTrainer(int id, CancellationToken ct)
        {
            var trainerToUpdate = await trainerService.GetTrainerToUpadate(id, ct);
            if (trainerToUpdate == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(trainerToUpdate);
        }

        [HttpPost]

        public async Task<IActionResult> EditTrainer(int id , TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(EditTrainer), model);
            }

            var result = await trainerService.UpdateTrainerDetailsAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer updated successfully.";
            else
                TempData["ErrorMessage"] = "Failed to update Trainer.";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Remove Trainer


        [HttpGet]
        public async Task<IActionResult> RemoveTrainer(int id, CancellationToken ct)
        {
            var trainer = await trainerService.GetTrainerByIdAsync(id, ct);
            if (trainer is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> RemoveTrainerConfirmed(int id, CancellationToken ct)
        {
            var result = await trainerService.RemoveTrainerAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer removed successfully.";
            else
                TempData["ErrorMessage"] = "Failed to remove Trainer.";
            return RedirectToAction(nameof(Index));
        }



        #endregion
    }
}
