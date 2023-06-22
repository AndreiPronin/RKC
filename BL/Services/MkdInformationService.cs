using BE.Counter;
using BE.MkdInformation;
using DB.DataBase;
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
        AddressMKD GetAddressMKD(int Id);
    }
    public class MkdInformationService : IMkdInformationService
    {
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
        public AddressMKD GetAddressMKD(int Id)
        {
            using (var db = new DbTPlus())
            {
                var result = db.addresses.FirstOrDefault(x => x.AddressId == Id);
                return result;
            }
        }
    }
}
