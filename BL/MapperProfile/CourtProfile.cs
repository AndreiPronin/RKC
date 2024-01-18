using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.MapperProfile
{
    public  class CourtProfile
    {
        private MapperConfiguration mapperConfiguration { get; set; }
        private MapperConfiguration mapperConfigurationBe { get; set; }
        public CourtProfile() 
        {
            mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BE.Court.CourtGeneralInformation, DB.Model.Court.CourtGeneralInformation>();
                cfg.CreateMap<BE.Court.CourtWork, DB.Model.Court.CourtWork>().ForMember(x => x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                cfg.CreateMap<BE.Court.CourtBankruptcy, DB.Model.Court.CourtBankruptcy>().ForMember(x => x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                cfg.CreateMap<BE.Court.CourtInstallmentPlan, DB.Model.Court.CourtInstallmentPlan>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<BE.Court.CourtLitigationWork, DB.Model.Court.CourtLitigationWork>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<BE.Court.CourtWriteOff, DB.Model.Court.CourtWriteOff>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<BE.Court.CourtStateDuty, DB.Model.Court.CourtStateDuty>().BeforeMap((s, d) => s.CourtGeneralInformationId = d.CourtGeneralInformationId);
                cfg.CreateMap<BE.Court.CourtExecutionInPF, DB.Model.Court.CourtExecutionInPF>().BeforeMap((s, d) => s.CourtGeneralInformationId = d.CourtGeneralInformationId);
                cfg.CreateMap<BE.Court.CourtExecutionFSSP, DB.Model.Court.CourtExecutionFSSP>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<BE.Court.CourtOwnerInformation, DB.Model.Court.CourtOwnerInformation>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
            });
            mapperConfigurationBe = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DB.Model.Court.CourtGeneralInformation, BE.Court.CourtGeneralInformation>();
                cfg.CreateMap<DB.Model.Court.CourtWork, BE.Court.CourtWork>().ForMember(x => x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                cfg.CreateMap<DB.Model.Court.CourtBankruptcy, BE.Court.CourtBankruptcy>().ForMember(x => x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                cfg.CreateMap<DB.Model.Court.CourtInstallmentPlan, BE.Court.CourtInstallmentPlan>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<DB.Model.Court.CourtLitigationWork, BE.Court.CourtLitigationWork>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<DB.Model.Court.CourtWriteOff, BE.Court.CourtWriteOff>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<DB.Model.Court.CourtStateDuty, BE.Court.CourtStateDuty>().BeforeMap((s, d) => s.CourtGeneralInformationId = d.CourtGeneralInformationId);
                cfg.CreateMap<DB.Model.Court.CourtExecutionInPF, BE.Court.CourtExecutionInPF>().BeforeMap((s, d) => s.CourtGeneralInformationId = d.CourtGeneralInformationId);
                cfg.CreateMap<DB.Model.Court.CourtExecutionFSSP, BE.Court.CourtExecutionFSSP>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<DB.Model.Court.CourtOwnerInformation, BE.Court.CourtOwnerInformation>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
            });
        }
        public Mapper GetMapper()
        {
            return new Mapper(mapperConfiguration);
        }
        public Mapper GetMapperBe()
        {
            return new Mapper(mapperConfigurationBe);
        }
    }
}
