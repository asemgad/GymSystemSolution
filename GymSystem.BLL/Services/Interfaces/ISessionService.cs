using GymManagement.BLL.Common;
using GymSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<bool> CreateSessionAsync(CreateSessionModel model, CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionByIdAysnc(int sessionId, CancellationToken ct);
        //Task<bool> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct = default);
        //Task<bool> DeleteSessionAsync(int sessionId, CancellationToken ct = default);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDown(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDown(CancellationToken ct = default);

        Task<Result<UpdateSessionViewModel>> GetSessionToUpdate( int id ,CancellationToken ct = default);
        Task<Result> UpdateSessionAsync( int id , UpdateSessionViewModel model,CancellationToken ct = default);


    }
}
