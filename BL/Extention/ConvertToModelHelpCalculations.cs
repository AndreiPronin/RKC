using BE.PersData;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class ConvertToModelHelpCalc
    {
        public static List<HelpCalculationsModel> ConvertToModelHelpCalculations(this List<HelpCalculationsModel> helpCalculationsModel, List<HelpСalculations> helpСalculations)
        {
            List<HelpCalculationsModel> helpCalculationsModels = new List<HelpCalculationsModel>();
            foreach(var Item in helpСalculations)
            {
                helpCalculationsModels.Add(new HelpCalculationsModel
                { 
                    DK = Item.DK,
                    DOM = Item.DOM,
                    FIO = Item.FIO,
                    GvsHeatingRecalculation = Item.GvsHeatingRecalculation,
                    GvsHeatingСalculation = Item.GvsHeatingСalculation,
                    HeatingRecalculation = Item.HeatingRecalculation,
                    SN = Item.SN,
                    HeatingСalculation = Item.HeatingСalculation,
                    HvHeatingRecalculation = Item.HvHeatingRecalculation,
                    HvHeatingСalculation = Item.HvHeatingСalculation,
                    KW = Item.KW,
                    LIC = Item.LIC,
                    NumberPerson = Item.NumberPerson,
                    Peny = Item.Peny,
                    PenySNpenySR = Item.PenySNpenySR,
                    Peny_dk = Item.Peny_dk,
                    Peny_tdk = Item.Peny_tdk,
                    Period = Item.Period,
                    SN15 = Item.SN15,
                    Sp = Item.Sp,
                    Square = Item.Square,
                    Tdk = Item.Tdk,
                    TdkPeny_tdk = Item.TdkPeny_tdk,
                    UL = Item.UL,
                });
            }
            return helpCalculationsModels;
        }
    }
}
