using AutoMapper;
using GymManagement.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Date.Models.Enums;
using GymSystem.DAL.Reprostries.Classes;
using GymSystem.DAL.Reprostries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return false;
            if (model.StartDate < DateTime.Now) return false;
            if (model.EndDate > DateTime.Now) ;
            if(model.Capacity <= 0) return false;

            var trainer = await _unitOfWork.GetGenericRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer == null) return false;

            var category = await _unitOfWork.GetGenericRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category == null) return false;

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var CategorySpecialty);
            if (!isValid || trainer.Speciality != CategorySpecialty) return false;



            var session = _mapper.Map<CreateSessionModel, Session>(model);
            _unitOfWork.GetGenericRepository<Session>().Add(session);
            var result = await _unitOfWork.SaveChanges();

            return result > 0;


        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync();
            if (sessions == null || !sessions.Any())
            {
                return null;
            }
            var sessionViewModels = sessions.Select(s => new SessionViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Capacity = s.Capacity,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                 TrainerName = s.Trainer.Name,
            });

            foreach (var sessionViewModel in sessionViewModels)
            {
                var availableSlots = sessionViewModel.Capacity - await sessionRepo.GetCountOfBookedSlotsAsync(sessionViewModel.Id, ct);
                sessionViewModel.AvailableSlots = availableSlots;
                // N + 1 problem: For each session, we are making a separate database call to get the count of booked slots. This can lead to performance issues if there are many sessions. Consider fetching all booked slots counts in a single query and then mapping them to the sessions to avoid multiple database calls.
            }
            return sessionViewModels;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDown(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetGenericRepository<Category>().GetAllAsync();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDown(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetGenericRepository<Trainer>().GetAllAsync();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<Result<SessionViewModel>> GetSessionByIdAysnc(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategoryAsync(sessionId, ct);
            if (session is null)
                return Result<SessionViewModel>.NotFound("Session Not Found");
            else
            {
                var mappedSession = _mapper.Map<SessionViewModel>(session);
                mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);

                return Result<SessionViewModel>.Ok(mappedSession);

            }
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdate(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);

            if(session is  null )
            {
              return Result<UpdateSessionViewModel>.NotFound("Session Not Found");
            }

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Can not Update Session that has Started");

            var BookedSession = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (BookedSession > 0)
            {
                return Result<UpdateSessionViewModel>.Fail("can not udpate on going session ");
            }

            var mappedSession = _mapper.Map<Session,UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mappedSession);


        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id, ct);
            if(session == null)
                 if (session is null) return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Can Not Edit Session That Has Started");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After StartDate");

            var BookedCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id);
            if (BookedCount > 0)
                return Result.Fail("Can Not Update Session That Has Booking");

            if (model.StartDate <= DateTime.Now)
                return Result.Fail("Start Date Must Be in Future");

            var trainer = await _unitOfWork.GetGenericRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetGenericRepository<Category>().GetByIdAsync(session.CategoryId, ct);

            var isValid = Enum.TryParse<Speciality>(category?.CategoryName, true, out var CategorySpecialty);
            if (!isValid || trainer.Speciality != CategorySpecialty) return Result.Validation("Can Not Create This Session To This Trainer");

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
            _unitOfWork.SessionRepository.Update(session);

            var result = await _unitOfWork.SaveChanges(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Fail To Update Session");

        }

       
    }
}
