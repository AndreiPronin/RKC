using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.MapperProfile
{
    public class IntegrationProfiles : Profile
    {
        public IntegrationProfiles()
        {
            CreateMap<DB.DataBase.PaymentV2Archive.Counters, DB.DataBase.PaymentV2.Counters>()
                .ForMember(s => s.DtCreate, d => d.Ignore())
                .ForMember(s => s.DtUpdate, d => d.Ignore())
                .ForMember(s => s.UserCreateId, d => d.Ignore())
                .ForMember(s => s.UserUpdateId, d => d.Ignore())
                .ForMember(s => s.AspNetUsers, d => d.Ignore())
                .ForMember(s => s.AspNetUsers1, d => d.Ignore())
                .ForMember(s => s.Payments, d => d.Ignore())
                .ReverseMap()
                .ForMember(s => s.Payments, d => d.Ignore())
                .ForMember(s => s.GUID, d => d.Ignore());
        }
    }
}
