using AutoMapper;
using BE.Counter;
using BE.MkdInformation;
using DB.DataBase;
using DB.FunctionModel;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IMkdInformationService
    {
        List<AddressMKD> SearchMkd(SearchModel searchModel);
        MainInformationModel GetAddressMKD(int Id);
        HistoryOdpuModel GetHistoryOdpu(int Id, DateTime DateFrom, DateTime DateTo);
        List<RecalculationsForMKDByCadrBe> HistoryRecalculation(int AddressId);
    }
    public class MkdInformationService : IMkdInformationService
    {
        private readonly IMapper _mapper;
        public MkdInformationService(IMapper mapper) 
        {
            _mapper = mapper;
        }
        public List<AddressMKD> SearchMkd(SearchModel searchModel)
        {
            using (var db = new DbTPlus())
            {
                try
                {
                    IQueryable<AddressMKD> query = db.addresses;
                    if (searchModel.AddressId != 0)
                        query = query.Where(x => x.AddressId == searchModel.AddressId);
                    if (!string.IsNullOrEmpty(searchModel.Street))
                        query = query.Where(x => x.Street.Contains(searchModel.Street));
                    if (!string.IsNullOrEmpty(searchModel.House))
                        query = query.Where(x => x.House == searchModel.House);
                    if (!string.IsNullOrEmpty(searchModel.Building))
                        query = query.Where(x => x.Building.Contains(searchModel.Building));

                    return query.Take(100).ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        public MainInformationModel GetAddressMKD(int Id)
        {
           
            using (var db = new DbTPlus())
            {
                var MainInform = new MainInformationModel();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AddressMKD, AddressMKDBe>();
                    cfg.CreateMap<AddressReadings, AddressReadingsBe>();

                });
                var mapper = new Mapper(config);

                var resultAdress = db.addresses.FirstOrDefault(x => x.AddressId == Id);
                var resultAdressReadings = db.addressReadings.OrderByDescending(x=>x.Period).FirstOrDefault(x => x.AddressId == Id);

                MainInform.AddressMKD = mapper.Map<AddressMKDBe>(resultAdress);
                MainInform.AddressReadings = mapper.Map<AddressReadingsBe>(resultAdressReadings);
                if(MainInform.AddressReadings == null)
                    MainInform.AddressReadings = new AddressReadingsBe();
                return MainInform;
            }
        }

        public HistoryOdpuModel GetHistoryOdpu(int Id, DateTime DateFrom, DateTime DateTo)
        {
            using (var db = new DbTPlus())
            {
                var history = new HistoryOdpuModel();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AllowNullCollections = true;
                    cfg.CreateMap<AddressReadings, AddressReadingsBe>().ReverseMap();
                    cfg.CreateMap<AddressMKD, AddressMKDBe>().ReverseMap();
                });
                var mapper = new Mapper(config);
                var resultAdress = db.addresses.FirstOrDefault(x => x.AddressId == Id);
                var resultAdressReadings = db.addressReadings.OrderByDescending(x => x.Period).Where(x => x.AddressId == Id 
                && x.Period >= DateFrom && x.Period <= DateTo ).ToList();
                history.addressMKD = mapper.Map<AddressMKDBe>(resultAdress);
                history.addressReadings = mapper.Map<List<AddressReadingsBe>>(resultAdressReadings);
              
                return history;
            }
        }

        public List<RecalculationsForMKDByCadrBe> HistoryRecalculation(int AddressId)
        {
            using(var db = new DbTPlus())
            {
                var result = db.Database.SqlQuery<RecalculationsForMKDByCadr>($"select * from [dbo].[GetRecalculationsForMKDByCadr]('{AddressId}')").ToList();
                return _mapper.Map<List<RecalculationsForMKDByCadrBe>>(result);
            }
        }
    }
}
