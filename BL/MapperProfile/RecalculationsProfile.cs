using AutoMapper;
using BE.Counter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.MapperProfile
{
    public class RecalculationsProfile : Profile
    {
        public RecalculationsProfile() 
        {

            CreateMap<RecalculationReason, DB.Model.RecalculationReason>();
            CreateMap<Recalculations, DB.Model.Recalculations>();
            CreateMap<BE.Counter.Service,DB.Model.Service>();

            CreateMap<DB.Model.RecalculationReason, RecalculationReason>();
            CreateMap<DB.Model.Recalculations, Recalculations>()
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment.Trim()));
            CreateMap<DB.Model.Service, BE.Counter.Service>();

        }
    }
}
