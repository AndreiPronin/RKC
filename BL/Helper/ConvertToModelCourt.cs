using AutoMapper;
using BE.Court;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public class ConvertToModelCourt
    {
        public DB.Model.Court.CourtGeneralInformation Convert_To_CourtGeneralInformation(DB.Model.Court.CourtGeneralInformation _courtGeneralInformation, CourtGeneralInformation courtGeneralInformation, out ChangedValues changedValues,string User)
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<CourtGeneralInformation, DB.Model.Court.CourtGeneralInformation>();
                cfg.CreateMap<CourtWork, DB.Model.Court.CourtWork>().ForMember(x=>x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                cfg.CreateMap<CourtBankruptcy, DB.Model.Court.CourtBankruptcy>().ForMember(x => x.CourtGeneralInformationId, opt => opt.MapFrom(src => src.CourtGeneralInformationId));
                cfg.CreateMap<CourtInstallmentPlan, DB.Model.Court.CourtInstallmentPlan>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<CourtLitigationWork, DB.Model.Court.CourtLitigationWork>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<CourtWriteOff, DB.Model.Court.CourtWriteOff>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
                cfg.CreateMap<CourtStateDuty, DB.Model.Court.CourtStateDuty>().BeforeMap((s, d) => s.CourtGeneralInformationId = d.CourtGeneralInformationId);
                cfg.CreateMap<CourtExecutionFSSP, DB.Model.Court.CourtExecutionFSSP>().ForMember(x => x.CourtGeneralInformationId, opt => opt.Ignore());
            });
            var mapper = new Mapper(config);
            DB.Model.Court.CourtGeneralInformation model = mapper.Map <CourtGeneralInformation, DB.Model.Court.CourtGeneralInformation>(courtGeneralInformation);

            changedValues = new ChangedValues();
            changedValues.User = User;
            changedValues.DateChanged = DateTime.Now;
            return model;
            //if (!courtGeneralInformation.Pensioner.Equals(courtGeneralInformation.Pensioner))
            //{
            //    _courtGeneralInformation.Pensioner = courtGeneralInformation.Pensioner;
            //    changedValues.Values.Add(new Values
            //    {
            //        NameValue = "Пенсионер",
            //        OldValue = _courtGeneralInformation.Pensioner,
            //        NewValue = courtGeneralInformation.Pensioner,
            //    });
            //}
            //return _courtGeneralInformation;
        }
    }
}
