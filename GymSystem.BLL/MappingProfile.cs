using AutoMapper;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Date.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add your mapping configurations here
            // Example:
            // CreateMap<SourceModel, DestinationModel>();


            #region Member Mapping
            CreateMap<Member, MemberViewModel>()
                //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) do not need to map Id because it has the same name
                .ForMember(dest => dest.Address , opt => opt.MapFrom(src => $"{src.Adress.Street}, {src.Adress.BuildingNumber}, {src.Adress.City}")) // needed to be mapped manullay here frist
                .ForMember(dest => dest.DateOfBirth , opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => new Adress
                {
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber,
                    City = src.City
                }))
                .ForMember(dest => dest.HealthRecord , opt => opt.MapFrom(src => src.healthRecordViewModel));

            CreateMap<HealthRecord , HealthRecordViewModel>().ReverseMap(); // from 1 to 2 and from 2 to 1

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) => // will be applies after the ampping for complex objects
                {
                    dest.Adress.Street = src.Street;
                    dest.Adress.BuildingNumber = src.BuildingNumber;
                    dest.Adress.City = src.City;

                });
            
         




            #endregion

            #region Session Mapping
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name)).ReverseMap();

            CreateMap<CreateSessionModel, Session>();

            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();



            #endregion

            #region Plan Mapping

            CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();

            CreateMap<Plan, PlanViewModel>().ReverseMap();

            #endregion
        }
    }
}
