using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.AnalyticesViewModels;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Classes;
using GymSystem.DAL.Reprostries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class AnalyticsService : IAnalytices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticesViewModel> GetAnalyticsAsync(CancellationToken ct = default)
        {
            var totalMembers = await _unitOfWork.GetGenericRepository<Member>().GetCountAsync(  ct : ct);
            var activeMembers = await _unitOfWork.GetGenericRepository<MemberShip>().GetCountAsync(m => m.EndDate > DateTime.Now, ct);
            var totalTrainers = await _unitOfWork.GetGenericRepository<Trainer>().GetCountAsync(ct : ct);
            var upcomingSessions = await _unitOfWork.GetGenericRepository<Session>().GetCountAsync(s => s.StartDate > DateTime.Now, ct);
            var ongoingSessions = await _unitOfWork.GetGenericRepository<Session>().GetCountAsync(X => X.StartDate <= DateTime.Now && X.EndDate >= DateTime.Now, ct);
            var completedSessions = await _unitOfWork.GetGenericRepository<Session>().GetCountAsync(X => X.EndDate < DateTime.Now, ct  );

            return new AnalyticesViewModel
            {
                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                TotalTrainers = totalTrainers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions
            };
        }

    }
}
