using GymSystem.BLL.ViewModels.TrainersViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        public Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct );

        public Task<TrainerViewModel?> GetTrainerByIdAsync(int id, CancellationToken ct = default);

        public Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);



        public Task<TrainerToUpdateViewModel?> GetTrainerToUpadate(int id, CancellationToken ct = default);
        public Task<bool > UpdateTrainerDetailsAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default);

        public Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default);
    }
}
