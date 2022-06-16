using BE.PersData;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public static class СomparisonModel
    {
        public static bool PersDataModel_To_PersData(PersData persData, PersDataModel persDataModel)
        {
            if(persData.FirstName != persDataModel.FirstName
                || persData.MiddleName != persDataModel.MiddleName
                || persData.LastName != persDataModel.LastName
                || persData.Square != persDataModel.Square
                || persData.NumberOfPersons != persDataModel.NumberOfPersons)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
