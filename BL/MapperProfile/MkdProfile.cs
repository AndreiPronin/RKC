using AutoMapper;
using BE.MkdInformation;
using DB.FunctionModel;
using DB.Model;

namespace BL.MapperProfile
{
    public class MkdProfile : Profile
    {
        public MkdProfile()
        {
            CreateMap<RecalculationsForMKDByCadrBe, RecalculationsForMKDByCadr>();
            CreateMap<RecalculationsForMKDByCadr, RecalculationsForMKDByCadrBe>();
            CreateMap<AddressMKD, AddressMKDBe>();
            CreateMap<AddressReadings, AddressReadingsBe>();
        }
    }
}
