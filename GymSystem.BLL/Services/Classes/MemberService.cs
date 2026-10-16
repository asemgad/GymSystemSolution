using AutoMapper;
using GymManagement.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
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
    public class MemberService : IMemberService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<MemberShip> _memberShipRepository;
        //private readonly IGenericRepository<Plan> _planRepository;
        //private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        //private readonly IGenericRepository<Booking> _bookingRepository; will replace them with unit of work

        //private readonly IUnitOfWork _unitOfWork;

        //public MemberService(IGenericRepository<Member> memberRepository , IGenericRepository<MemberShip> membershipRepository
        //    , IGenericRepository<Plan> planRepository
        //    , IGenericRepository<HealthRecord> healthRecordRepository,
        //    IGenericRepository<Booking> bookingRepository)
        //public MemberService(IUnitOfWork unitOfWork , IMapper mapper)
        public MemberService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
            //_memberRepository = memberRepository;
            //_memberShipRepository = membershipRepository;
            //_planRepository = planRepository;
            //_healthRecordRepository = healthRecordRepository;
            //_bookingRepository = bookingRepository;


        }
        public async Task<IEnumerable<MemberViewModel>> GetallAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetGenericRepository<Member>().GetAllAsync();
            if (!members.Any())
            {
                //return Enumerable.Empty<MemberViewModel>();
                return [];
            }
            //List<MemberViewModel> memberViewModels = new List<MemberViewModel>();

            //foreach (var member in members)
            //{
            //    memberViewModels.Add(new MemberViewModel
            //    {
            //        Id = member.Id,
            //        Photo = member.Photo,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.Phone,
            //        Gender = member.Gender.ToString()
            //    });
            //}

            //var memberViewModels = members.Select(member => new MemberViewModel
            //{
            //    Id = member.Id,
            //    Photo = member.Photo,
            //    Name = member.Name,
            //    Email = member.Email,
            //    Phone = member.Phone,
            //    Gender = member.Gender.ToString()
            //}).ToList();

            var memberViewModels = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);

            return memberViewModels;
        }




        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            // check email 
            var emailExists = await _unitOfWork.GetGenericRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            // check phone
            var phoneExists = await _unitOfWork.GetGenericRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);
            // if email or phone exists return false
            if (emailExists || phoneExists)
            {
                return Result.Fail("Email or phone number already exists.");
            }

            //var member = new Member()
            //{
            //    Email = model.Email,
            //    Name = model.Name,
            //    Phone = model.Phone,
            //    Gender = model.Gender,
            //    DateOfBirth = model.DateOfBirth,
            //    Adress = new Adress()
            //    {
            //        Street = model.Street,
            //        BuildingNumber = model.BuildingNumber,
            //        City = model.City,

            //    },
            //    HealthRecord = new HealthRecord()
            //    {
            //        BloodType = model.healthRecordViewModel.BloodType,
            //        Weight = model.healthRecordViewModel.Weight,
            //        Height = model.healthRecordViewModel.Height,
            //        Note = model.healthRecordViewModel.Note
            //    }



            //};


            var member = _mapper.Map<Member>(model); // Map the model to the member entity
            // isteasd of  var memvber.Map<CreateMemberViewModel , Member>()


            // var result = await _memberRepository.Add(member); // Add the member to the repository and retuns the rows affected to knwo if anything is added
            _unitOfWork.GetGenericRepository<Member>().Add(member);// Locally and at the end we will do it to database
            var result = await _unitOfWork.SaveChanges(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to create member.");

        }


        
        public async Task<MemberViewModel?> GetMemberByIdAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetGenericRepository<Member>().GetByIdAsync(id, ct);

            if (member == null)
            {
                return null;
            }
            //var model = new MemberViewModel()
            //{
            //    Phone = member.Phone,
            //    Name = member.Name,
            //    Gender = member.Gender.ToString(),
            //    DateOfBirth = member.DateOfBirth,
            //    Address = $"{member.Adress.Street}, {member.Adress.BuildingNumber}, {member.Adress.City}"

            //};
            // Insted with mapper 
            var model = _mapper.Map<Member,MemberViewModel>(member);
            var activeMembership = await _unitOfWork.GetGenericRepository<MemberShip>().FirstOrDefaultAsync(x => x.MemberId == member.Id && x.EndDate > DateTime.Now);

            if (activeMembership is not  null)
            {
                var plan = await _unitOfWork.GetGenericRepository<Plan>().GetByIdAsync(activeMembership.PlanId);

                model.PlanName = plan?.Name ?? "No Plan";
                model.MemberShipCreateDate = activeMembership.CreatedAt;
                model.MemberShipEndDate = activeMembership.EndDate.Value;
            }

            return model;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordByIdAsync(int id, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetGenericRepository<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId ==id , ct);

            if (record is  null )
            {
                return null; 
            }
            //var model = new HealthRecordViewModel()
            //{
            //    Height = record.Height,
            //    Weight = record.Weight,
            //    BloodType = record.BloodType,
            //    Note = record.Note

            //};

           
            return _mapper.Map<HealthRecordViewModel>(record);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetGenericRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null)
            {
                return null;
            }
            var model = new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Adress.BuildingNumber,
                City = member.Adress.City,
                Street = member.Adress.Street
            };
            return model;
        }


        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
           var member = await _unitOfWork.GetGenericRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
            {
                return false;
            }

            var emailExists = await _unitOfWork.GetGenericRepository<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id, ct);
            var phoneExists = await _unitOfWork.GetGenericRepository<Member>().AnyAsync(x => x.Phone == model.Phone && x.Id != id, ct);

            if(emailExists || phoneExists)
            {
                return false;
            }


            //member.Email = model.Email;
            //member.Phone = model.Phone;
            //member.Adress.BuildingNumber = model.BuildingNumber;
            //member.Adress.City = model.City;
            //member.Adress.Street = model.Street;
            //member.UpdatedAt = DateTime.Now;

            _mapper.Map(model, member);
            //var result = await _memberRepository.UpdateAsync(member, ct); befoere unit of work 
           _unitOfWork.GetGenericRepository<Member>().Update(member);
            var result = await _unitOfWork.SaveChanges(ct);
            return result > 0;
        }

        public async Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetGenericRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null)
            {
                return false;
            }

            var hasBookings = await _unitOfWork.GetGenericRepository<Booking>().AnyAsync(x => x.MemberId == memberId  && x.Session.StartDate > DateTime.Now , ct);
            if (hasBookings)
            {
                return false;
            }

            _unitOfWork.GetGenericRepository<Member>().Delete(member);
            var result = await _unitOfWork.SaveChanges(ct);
            return result > 0;
        }

    



        }
    }

