using AutoMapper;
using BE.MkdInformation;
using DB.FunctionModel;

namespace BL.MapperProfile
{
    public class MkdProfile : Profile
    {
        public MkdProfile()
        {
            CreateMap<RecalculationsForMKDByCadrBe, RecalculationsForMKDByCadr>();
            CreateMap<RecalculationsForMKDByCadr, RecalculationsForMKDByCadrBe>();
        }
    }
}
