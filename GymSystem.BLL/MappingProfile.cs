using AutoMapper;
using GymSystem.BLL.ViewModels.MemberViewModels;
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
                .ForMember(dest => dest.Address , opt => opt.MapFrom(src => $"{src.Adress.Street}, {src.Adress.BuildingNumber}, {src.Adress.City}"))
                .ForMember(dest => dest.DateOfBirth , opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()));
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
        }
    }
}
