using AutoMapper;
using BE.PersData;
using DB.DataBase.PaymentV2;
using PaymentsArchive = DB.DataBase.PaymentV2Archive.Payment;

namespace BL.MapperProfile
{
    public class PersDataProfile : Profile
    {
        public PersDataProfile()
        {
            CreateMap<Payments, PaymentHistoryResponse>()
                .ForMember(x => x.PaymentDateDay, d => d.MapFrom(x => x.PaymentDateDay))
                .ForMember(x => x.PaymentDate, d => d.MapFrom(x => x.PaymentDate))
                .ForMember(x => x.TransactionAmount, d => d.MapFrom(x => x.TransactionAmount))
                .ForMember(x => x.OrganizationName, d => d.MapFrom(x => x.Orgs.Name));
            CreateMap<PaymentsArchive, PaymentHistoryResponse>()
               .ForMember(x => x.PaymentDateDay, d => d.MapFrom(x => x.PaymentDateDay))
               .ForMember(x => x.PaymentDate, d => d.MapFrom(x => x.PaymentDate))
               .ForMember(x => x.TransactionAmount, d => d.MapFrom(x => x.TransactionAmount))
               .ForMember(x => x.OrganizationName, d => d.MapFrom(x => x.OrgName));
        }
    }
}
