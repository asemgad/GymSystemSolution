using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainersViewModel;
using GymSystem.DAL.Date.Models;
using GymSystem.DAL.Reprostries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class TrainerSerivce : ITrainerService
    {
        //private readonly IGenericRepository<Trainer> _trainerRepository;
        //private readonly IGenericRepository<Session> _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TrainerSerivce(IUnitOfWork unitOfWork)
        {
            //    _trainerRepository = trainerRepository;
            //    _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;   // ✅ correct direction
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOfWork.GetGenericRepository<Trainer>().AnyAsync(t => t.Email == model.Email);
            var phoneExists = await _unitOfWork.GetGenericRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone);

            if (emailExists || phoneExists)
            {
                return false;
            }

            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Speciality = model.Speciality,
                DateOfBirth = model.DateOfBirth ,// Placeholder for actual date of birth
                Gender = model.Gender, 

                Adress = new Adress
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                }



            };

           //var result =  await _trainerRepository.AddAsync(trainer, ct);
           _unitOfWork.GetGenericRepository<Trainer>().Add(trainer);
            var result = await _unitOfWork.SaveChanges(ct);
           return result>0;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct)
        {
            var trainers = await _unitOfWork.GetGenericRepository<Trainer>().GetAllAsync(cancellationToken : ct);

            if (!trainers.Any())
            {
                return [];
            }

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Speciality
            });

        }

        public async Task<TrainerViewModel?> GetTrainerByIdAsync(int id, CancellationToken ct = default)
        {
           var trainer =  await _unitOfWork.GetGenericRepository<Trainer>().GetByIdAsync(id, ct);
            if(trainer is null)
            {
                return null;
            }
            var model = new TrainerViewModel()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Speciality,
                 Adress = $"Building Number: {trainer.Adress.BuildingNumber}, Street: {trainer.Adress.Street}, City: {trainer.Adress.City}"


            };
            return model;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpadate(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetGenericRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
            {
                return null;
            }

            var model = new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Speciality = trainer.Speciality,
                DateOfBirth = trainer.DateOfBirth,
                Gender = trainer.Gender,
                BuildingNumber = trainer.Adress.BuildingNumber,
                Street = trainer.Adress.Street,
                City = trainer.Adress.City
            };
            return model;
        }

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetGenericRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null)
            {
                return false;
            }

            var trainerHaveSessions = await _unitOfWork.GetGenericRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
            if (trainerHaveSessions)
            {
                return false;
            }

             _unitOfWork.GetGenericRepository<Trainer>().Delete(trainer);
            var result = await _unitOfWork.SaveChanges(ct);

            return result > 0;
        }


        public async Task<bool> UpdateTrainerDetailsAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOfWork.GetGenericRepository<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != id);
            var phoneExists = await _unitOfWork.GetGenericRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != id);

            if (emailExists || phoneExists)
            {
                return false;
            }

            var trainer = await _unitOfWork.GetGenericRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
            {
                return false;
            }

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Speciality = model.Speciality;
            trainer.Adress.BuildingNumber = model.BuildingNumber;
            trainer.Adress.Street = model.Street;
            trainer.Adress.City = model.City;
            trainer.UpdatedAt = DateTime.Now;

             _unitOfWork.GetGenericRepository<Trainer>().Update(trainer);
            var result = await _unitOfWork.SaveChanges(ct);

            return result > 0;

        }
    }
}
