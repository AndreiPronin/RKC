using BE.Counter;
using DB.Model;
using System;

namespace BL.Extention
{
    public static class CounterExtensions
    {
        public static IpuGisReading ConvertToIpuGisReading(this IPU_COUNTERS ipuSource, ALL_LICS_ARCHIVE allLics, AddressMKD address, FlatMkd flat)
        {
            IpuGisReading result = new IpuGisReading();
            result.IdPu = ipuSource.ID_PU;
            result.FulLic = ipuSource.FULL_LIC;
            result.Type = ipuSource.TYPE_PU;
            result.FactoryNumber = ipuSource.FACTORY_NUMBER_PU;
            result.Brand = ipuSource.BRAND_PU;
            result.Model = ipuSource.MODEL_PU;
            result.GisId = ipuSource.GIS_ID_PU;
            result.IdGku = flat.IdGku;
            result.IsClosed = ipuSource.CLOSE_;
            result.Els = flat.Els;
            result.UniqueApartmentNumber = flat.UniqueApartmentNumber;
            result.Fias = address.Fias;
            result.DateCheck = ipuSource.DATE_CHECK;
            result.LastReadingDate = ipuSource.LastReadingDate;
            result.InterVerificationInterval = DateTimes.GetYear(ipuSource.InterVerificationInterval ?? 0);
            result.DateCheckNext = ipuSource.DATE_CHECK_NEXT;
            result.InstallationDate = ipuSource.INSTALLATIONDATE;
            result.Address = address.CityType + " " + address.City + ", " + address.Street + " "
                + address.StreetType + ", " + address.HouseType + " " + address.House + address.Building;
            result.TypeOfPU = ipuSource.TypeOfPu;

            if (result.IsClosed == true)
            {
                result.IpuArchiveReasonGisId = ipuSource.IpuArchiveReason?.GisId;
                result.DateClose = ipuSource.DATE_CLOSE;
            }
            else
            {
                result.IpuRecoverReasonGisId = ipuSource.IpuRecoverReason?.GisId;
                result.RecoverDate = ipuSource?.RecoverDate;

                switch (result.Type)
                {
                    case "ГВС1":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1)
                        {
                            result.FinalReadings = allLics.FKUB2XVS ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ГВС2":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1 && allLics.FKUBSXV_2 == 1)
                        {
                            result.FinalReadings = allLics.FKUB2XV_2 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ГВС3":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1 && allLics.FKUBSXV_3 == 1)
                        {
                            result.FinalReadings = allLics.FKUB2XV_3 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ГВС4":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1 && allLics.FKUBSXV_4 == 1)
                        {
                            result.FinalReadings = allLics.FKUB2XV_4 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ОТП1":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_1 == 1)
                        {
                            result.FinalReadings = allLics.FKUB2OT_1 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ОТП2":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_2 == 1)
                        {
                            result.FinalReadings = allLics.FKUB2OT_2 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ОТП3":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_3 == 1)
                        {
                            result.FinalReadings = allLics.FKUB2OT_3 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ОТП4":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_4 == 1)
                        {
                            result.FinalReadings = allLics.FKUB2OT_4 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    default:
                        result.ServiceType = "";
                        result.FinalReadings = null;
                        break;
                }
            }

            if (ipuSource.DIMENSION != null)
            {
                result.Dimension = ipuSource.DIMENSION.DIMENSION_NAME;
            }
            else if (ipuSource.DIMENSION_ID != null)
            {
                switch (ipuSource.DIMENSION_ID)
                {
                    case 1:
                        result.Dimension = "Кубический метр";
                        break;
                    case 2:
                        result.Dimension = "Гигакалория";
                        break;
                    case 3:
                        result.Dimension = "Киловатт-час";
                        break;
                    default :
                        result.Dimension = result.Type.StartsWith("ГВС") ? "Кубический метр" : "Гигакалория";
                        break;
                }
            }
            else if (result.Type.StartsWith("ГВС"))
            {
                result.Dimension = "Кубический метр";
            }
            else if (result.Type.StartsWith("ОТП"))
            {
                result.Dimension = "Гигакалория";
            }

            if (ipuSource.IpuArchiveReasonId == 13)
            {
                result.FinalReadings = null;
            }

            return result;
        }
        public static IpuGisReadingActive ConvertToIpuGisReading(this IPU_COUNTERS ipuSource, ALL_LICS allLics, AddressMKD address, FlatMkd flat)
        {
            IpuGisReadingActive result = new IpuGisReadingActive();
            result.IdPu = ipuSource.ID_PU;
            result.FulLic = ipuSource.FULL_LIC;
            result.Type = ipuSource.TYPE_PU;
            result.FactoryNumber = ipuSource.FACTORY_NUMBER_PU;
            result.Brand = ipuSource.BRAND_PU;
            result.Model = ipuSource.MODEL_PU;
            result.GisId = ipuSource.GIS_ID_PU;
            result.IdGku = flat?.IdGku;
            result.LastReadingDate = ipuSource.LastReadingDate;
            result.InterVerificationInterval = DateTimes.GetYear(ipuSource.InterVerificationInterval ?? 0);
            result.UniqueApartmentNumber = flat?.UniqueApartmentNumber;
            result.Fias = address?.Fias;
            result.Els = flat.Els;
            result.DateCheck = ipuSource.DATE_CHECK;
            result.DateCheckNext = ipuSource.DATE_CHECK_NEXT;
            result.IsClosed = ipuSource.CLOSE_.HasValue ? ipuSource.CLOSE_.Value : false;
            result.InstallationDate = ipuSource.INSTALLATIONDATE;
            result.Address = address?.CityType + " " + address?.City + ", " + address?.Street + " "
                + address?.StreetType + ", " + address?.HouseType + " " + address?.House + address?.Building;
            result.TypeOfPU = ipuSource.TypeOfPu;

            if (result.IsClosed == true)
            {
                result.IpuArchiveReasonGisId = ipuSource.IpuArchiveReason?.GisId;
                result.DateClose = ipuSource.DATE_CLOSE;
            }
            else
            {
                result.IpuRecoverReasonGisId = ipuSource.IpuRecoverReason?.GisId;
                result.RecoverDate = ipuSource.RecoverDate;

                switch (result.Type)
                {
                    case "ГВС1":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2XVS ?? 0M;
                            break;
                        }
                        result.HasNewReadings = false;
                        result.FinalReadings = null;
                        break;
                    case "ГВС2":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1 && allLics.FKUBSXV_2 == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2XV_2 ?? 0M;
                            break;
                        }
                        result.HasNewReadings = false;
                        result.FinalReadings = null;
                        break;
                    case "ГВС3":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1 && allLics.FKUBSXV_3 == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2XV_3 ?? 0M;
                            break;
                        }
                        result.HasNewReadings = false;
                        result.FinalReadings = null;
                        break;
                    case "ГВС4":
                        result.ServiceType = "Горячая вода";
                        if (allLics.FKUBSXVS == 1 && allLics.FKUBSXV_4 == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2XV_4 ?? 0M;
                            break;
                        }
                        result.FinalReadings = null;
                        break;
                    case "ОТП1":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_1 == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2OT_1 ?? 0M;
                            break;
                        }
                        result.HasNewReadings = false;
                        result.FinalReadings = null;
                        break;
                    case "ОТП2":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_2 == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2OT_2 ?? 0M;
                            break;
                        }
                        result.HasNewReadings = false;
                        result.FinalReadings = null;
                        break;
                    case "ОТП3":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_3 == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2OT_3 ?? 0M;
                            break;
                        }
                        result.HasNewReadings = false;
                        result.FinalReadings = null;
                        break;
                    case "ОТП4":
                        result.ServiceType = "Тепловая энергия";
                        if (allLics.FKUBSOT_1 == 1 && allLics.FKUBSOT_4 == 1)
                        {
                            result.HasNewReadings = true;
                            result.FinalReadings = allLics.FKUB2OT_4 ?? 0M;
                            break;
                        }
                        result.HasNewReadings = false;
                        result.FinalReadings = null;
                        break;
                    default:
                        result.ServiceType = "";
                        result.FinalReadings = null;
                        break;
                }
            }

            if (ipuSource.DIMENSION != null)
            {
                result.Dimension = ipuSource.DIMENSION.DIMENSION_NAME;
            }
            else if (ipuSource.DIMENSION_ID != null)
            {
                switch (ipuSource.DIMENSION_ID)
                {
                    case 1:
                        result.Dimension = "Кубический метр";
                        break;
                    case 2:
                        result.Dimension = "Гигакалория";
                        break;
                    case 3:
                        result.Dimension = "Киловатт-час";
                        break;
                    default:
                        result.Dimension = result.Type.StartsWith("ГВС") ? "Кубический метр" : "Гигакалория";
                        break;
                }
            }
            else if (result.Type.StartsWith("ГВС"))
            {
                result.Dimension = "Кубический метр";
            }
            else if (result.Type.StartsWith("ОТП"))
            {
                result.Dimension = "Гигакалория";
            }

            if (ipuSource.IpuArchiveReasonId == 13)
            {
                result.HasNewReadings = false;
                result.FinalReadings = null;
            }

            return result;
        }
        public static decimal? ConvertToIpuGisReading(this IPU_COUNTERS ipuSource, ALL_LICS allLics)
        {
            decimal? result = null;

            switch (ipuSource.TYPE_PU)
            {
                case "ГВС1":
                    result = allLics.FKUB2XVS;
                    break;
                case "ГВС2":
                    result = allLics.FKUB2XV_2;
                    break;
                case "ГВС3":
                    result = allLics.FKUB2XV_3;
                    break;
                case "ГВС4":
                    result = allLics.FKUB2XV_4;
                    break;
                case "ОТП1":
                    result = allLics.FKUB2OT_1;
                    break;
                case "ОТП2":
                    result = allLics.FKUB2OT_2;
                    break;
                case "ОТП3":
                    result = allLics.FKUB2OT_3;
                    break;
                case "ОТП4":
                    result = allLics.FKUB2OT_4;
                    break;
                default:
                    throw new Exception("не найден тип прибора учета");
            }
            return result;
        }
    }
}
