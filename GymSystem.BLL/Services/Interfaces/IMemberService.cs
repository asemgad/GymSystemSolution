using GymManagement.BLL.Common;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        //public Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct);
        Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);

        public Task<IEnumerable<MemberViewModel>> GetallAsync( CancellationToken ct = default);

        public Task<MemberViewModel?> GetMemberByIdAsync(int id, CancellationToken ct = default);
        public Task<HealthRecordViewModel?> GetHealthRecordByIdAsync(int id, CancellationToken ct = default);

        public Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);

        public Task<bool> UpdateMemberDetailsAsync( int id ,MemberToUpdateViewModel model, CancellationToken ct = default);

        public Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default);


    }
}
