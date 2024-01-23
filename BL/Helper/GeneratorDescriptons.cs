using BE.Counter;
using BE.Court;
using BE.PersData;
using BL.Services;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BL.Helper
{
    public interface IGeneratorDescriptons
    {
        string Generate(SaveModelIPU saveModelIPU);
        string Generate(PersDataModel PersDataModel, bool IgnorNull = false);
        string Generate(CourtGeneralInformation courtGeneralInformation, DB.Model.Court.CourtGeneralInformation courtGeneralInformationDb, string User);
    }
    public class GeneratorDescriptons :BaseService, IGeneratorDescriptons
    {
        public string Generate(SaveModelIPU saveModelIPU)
        {
            StringBuilder Result = new StringBuilder();
            using (var db= new DbTPlus())
            {
                if (saveModelIPU.FKUB1XVS != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1XVS != saveModelIPU.FKUB1XVS || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ГВС1: было {aLL_LICS.FKUB1XVS} стало {saveModelIPU.FKUB1XVS}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1XV_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1XV_2 != saveModelIPU.FKUB1XV_2 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ГВС2: было {aLL_LICS.FKUB1XV_2} стало {saveModelIPU.FKUB1XV_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1XV_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1XV_3 != saveModelIPU.FKUB1XV_3 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ГВС3: было {aLL_LICS.FKUB1XV_3} стало {saveModelIPU.FKUB1XV_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1XV_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1XV_4 != saveModelIPU.FKUB1XV_4 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ГВС4: было {aLL_LICS.FKUB1XV_4} стало {saveModelIPU.FKUB1XV_4}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_1 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_1 != saveModelIPU.FKUB1OT_1 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ОТП1: было {aLL_LICS.FKUB1OT_1} стало {saveModelIPU.FKUB1OT_1}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_2 != saveModelIPU.FKUB1OT_2 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ОТП2: было {aLL_LICS.FKUB1OT_2} стало {saveModelIPU.FKUB1OT_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_3 != saveModelIPU.FKUB1OT_3 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ОТП3: было {aLL_LICS.FKUB1OT_3} стало {saveModelIPU.FKUB1OT_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_4 != saveModelIPU.FKUB1OT_4 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили начальные показания ОТП4: было {aLL_LICS.FKUB1OT_4} стало {saveModelIPU.FKUB1OT_4}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XVS != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XVS != saveModelIPU.FKUB2XVS || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ГВС1: было {aLL_LICS.FKUB2XVS} стало {saveModelIPU.FKUB2XVS}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XV_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XV_2 != saveModelIPU.FKUB2XV_2 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ГВС2: было {aLL_LICS.FKUB2XV_2} стало {saveModelIPU.FKUB2XV_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XV_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XV_3 != saveModelIPU.FKUB2XV_3 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ГВС3: было {aLL_LICS.FKUB2XV_3} стало {saveModelIPU.FKUB2XV_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XV_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XV_4 != saveModelIPU.FKUB2XV_4 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ГВС4: было {aLL_LICS.FKUB2XV_4} стало {saveModelIPU.FKUB2XV_4}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_1 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_1 != saveModelIPU.FKUB2OT_1 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ОТП1: было {aLL_LICS.FKUB2OT_1} стало {saveModelIPU.FKUB2OT_1}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_2 != saveModelIPU.FKUB2OT_2 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ОТП2: было {aLL_LICS.FKUB2OT_2} стало {saveModelIPU.FKUB2OT_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_3 != saveModelIPU.FKUB2OT_3 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ОТП3: было {aLL_LICS.FKUB2OT_3} стало {saveModelIPU.FKUB2OT_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_4 != saveModelIPU.FKUB2OT_4 || saveModelIPU.OVERWRITE_SEAL == true)
                            Result.Append($"Изменили конечные показания ОТП4: было {aLL_LICS.FKUB2OT_4} стало {saveModelIPU.FKUB2OT_4}  \r\n");

                    }
                }
                IPU_COUNTERS IPU_COUNTERS = db.IPU_COUNTERS.Where(x => x.ID_PU == saveModelIPU.IdPU).FirstOrDefault();
                if (IPU_COUNTERS.FACTORY_NUMBER_PU != saveModelIPU.NumberPU && !string.IsNullOrEmpty(saveModelIPU.NumberPU)) Result.Append($"Изменили номер ПУ: было {IPU_COUNTERS.FACTORY_NUMBER_PU} стало {saveModelIPU.NumberPU} \r\n");
                if (IPU_COUNTERS.DATE_CHECK != saveModelIPU.DATE_CHECK && saveModelIPU.DATE_CHECK != null) Result.Append($"Изменили дату поверки ПУ: было {IPU_COUNTERS.DATE_CHECK}  стало {saveModelIPU.DATE_CHECK}  \r\n");
                if (IPU_COUNTERS.DATE_CHECK_NEXT != saveModelIPU.DATE_CHECK_NEXT && saveModelIPU.DATE_CHECK_NEXT != null) Result.Append($"Изменили дату следующей поверки ПУ: было {IPU_COUNTERS.DATE_CHECK_NEXT} стало {saveModelIPU.DATE_CHECK_NEXT}  \r\n");
                if (IPU_COUNTERS.MODEL_PU != saveModelIPU.MODEL_PU && !string.IsNullOrEmpty(saveModelIPU.MODEL_PU)) Result.Append($"Изменили модель ПУ: было {IPU_COUNTERS.MODEL_PU} стало {saveModelIPU.MODEL_PU}  \r\n");
                if (IPU_COUNTERS.BRAND_PU != saveModelIPU.BRAND_PU && !string.IsNullOrEmpty(saveModelIPU.BRAND_PU)) Result.Append($"Изменили модель Бренд ПУ: было {IPU_COUNTERS.BRAND_PU} стало {saveModelIPU.BRAND_PU}  \r\n");
                if (IPU_COUNTERS.SEALNUMBER != saveModelIPU.SEALNUMBER && !string.IsNullOrEmpty(saveModelIPU.SEALNUMBER)) Result.Append($"Изменили номер пломбы1: было {IPU_COUNTERS.SEALNUMBER} стало {saveModelIPU.SEALNUMBER}  \r\n");
                if (IPU_COUNTERS.INSTALLATIONDATE != saveModelIPU.INSTALLATIONDATE && saveModelIPU.INSTALLATIONDATE != null) Result.Append($"Изменили дату установки: было {IPU_COUNTERS.INSTALLATIONDATE} стало {saveModelIPU.INSTALLATIONDATE}  \r\n");
                if (IPU_COUNTERS.TYPEOFSEAL != saveModelIPU.TYPEOFSEAL && !string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL)) Result.Append($"Изменили тип пломбы1: было {IPU_COUNTERS.TYPEOFSEAL} стало {saveModelIPU.TYPEOFSEAL}  \r\n");
                if (IPU_COUNTERS.TYPEOFSEAL2 != saveModelIPU.TYPEOFSEAL2 && !string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL2)) Result.Append($"Изменили тип пломбы2: было {IPU_COUNTERS.TYPEOFSEAL2} стало {saveModelIPU.TYPEOFSEAL2}  \r\n");
                if (IPU_COUNTERS.SEALNUMBER2 != saveModelIPU.SEALNUMBER2 && !string.IsNullOrEmpty(saveModelIPU.SEALNUMBER2)) Result.Append($"Изменили номер пломбы2: было {IPU_COUNTERS.SEALNUMBER2} стало {saveModelIPU.SEALNUMBER2}  \r\n");
                if (IPU_COUNTERS.CHECKPOINT_DATE != saveModelIPU.CHECKPOINT_DATE && saveModelIPU.CHECKPOINT_DATE != null) Result.Append($"Изменили дату контрольного обхода: было {IPU_COUNTERS.CHECKPOINT_DATE} стало {saveModelIPU.CHECKPOINT_DATE}  \r\n");
                if (IPU_COUNTERS.CHECKPOINT_READINGS != saveModelIPU.CHECKPOINT_READINGS && saveModelIPU.CHECKPOINT_READINGS != null) Result.Append($"Изменили показания контрольного обхода: было {IPU_COUNTERS.CHECKPOINT_READINGS} стало {saveModelIPU.CHECKPOINT_READINGS}  \r\n");
                if (IPU_COUNTERS.OPERATOR_CLOSE_DATE != saveModelIPU.OPERATOR_CLOSE_DATE && saveModelIPU.OPERATOR_CLOSE_DATE != null) Result.Append($"Изменили дату закрытия: было {IPU_COUNTERS.OPERATOR_CLOSE_DATE} стало {saveModelIPU.OPERATOR_CLOSE_DATE}  \r\n");
                if (IPU_COUNTERS.OPERATOR_CLOSE_READINGS != saveModelIPU.OPERATOR_CLOSE_READINGS && saveModelIPU.OPERATOR_CLOSE_READINGS != null) Result.Append($"Изменили конечные показания при закрытии: было {IPU_COUNTERS.OPERATOR_CLOSE_READINGS} стало {saveModelIPU.OPERATOR_CLOSE_READINGS}  \r\n");
                if (IPU_COUNTERS.DESCRIPTION != saveModelIPU.DESCRIPTION && !string.IsNullOrEmpty(saveModelIPU.DESCRIPTION)) Result.Append($"Изменили примечание: {saveModelIPU.DESCRIPTION}\r\n");
                if (saveModelIPU.DIMENSION != null && saveModelIPU.DIMENSION.Id != 0 && IPU_COUNTERS.DIMENSION_ID != saveModelIPU.DIMENSION.Id) Result.Append($"Изменили еденицу измерения: {saveModelIPU.DIMENSION.Name}\r\n");
            }

            return Result.ToString();
        }
        public string Generate(PersDataModel PersDataModel, bool IgnorNull = false)
        {
            StringBuilder Result = new StringBuilder();
            var FlatType = GetFlatTypeLic(PersDataModel.Lic);
            if (!string.IsNullOrEmpty(PersDataModel.FlatTypeId) && PersDataModel.FlatTypeId != FlatType.FlatTypeId)
                Result.Append($"Изменил категорию помещения: было {FlatType.FlatType} стало {PersDataModel.FlatType} \r\n");
            using (var db = new ApplicationDbContext()) {
                PersData PersData = db.PersData.Where(x => x.idPersData == PersDataModel.idPersData).FirstOrDefault();
                if (PersData?.FirstName != PersDataModel.FirstName && (!string.IsNullOrEmpty(PersDataModel.FirstName) || IgnorNull)) 
                    Result.Append($"Изменили Имя: было {PersData.FirstName} стало {PersDataModel.FirstName} \r\n");
                if (PersData?.LastName != PersDataModel.LastName && (!string.IsNullOrEmpty(PersDataModel.LastName) || IgnorNull)) 
                    Result.Append($"Изменили Фамилию: было {PersData.LastName} стало {PersDataModel.LastName} \r\n");
                if (PersData?.MiddleName != PersDataModel.MiddleName && (!string.IsNullOrEmpty(PersDataModel.MiddleName) || IgnorNull)) 
                    Result.Append($"Изменили отчество: было {PersData.MiddleName} стало {PersDataModel.MiddleName} \r\n");
                if (PersData?.DateOfBirth != PersDataModel.DateOfBirth && PersDataModel.DateOfBirth != null)
                    Result.Append($"Изменили дату рождения: было {PersData.DateOfBirth} стало {PersDataModel.DateOfBirth} \r\n");
                //if (PersData.Lic != PersDataModel.Lic && !string.IsNullOrEmpty(PersDataModel.Lic)) Result.Append($"Изменили номер ПУ: было {PersData.Lic} стало {PersDataModel.Lic} \r\n");
                if (PersData?.PlaceOfBirth != PersDataModel.PlaceOfBirth && (!string.IsNullOrEmpty(PersDataModel.PlaceOfBirth) || IgnorNull))
                    Result.Append($"Изменили место рождения: было {PersData.PlaceOfBirth} стало {PersDataModel.PlaceOfBirth} \r\n");
                if (PersData?.PassportSerial != PersDataModel.PassportSerial && (!string.IsNullOrEmpty(PersDataModel.PassportSerial) || IgnorNull)) 
                    Result.Append($"Изменили паспорт серию: было {PersData.PassportSerial} стало {PersDataModel.PassportSerial} \r\n");
                if (PersData?.PassportNumber != PersDataModel.PassportNumber && (!string.IsNullOrEmpty(PersDataModel.PassportNumber) || IgnorNull)) 
                    Result.Append($"Изменили паспорт номер: {PersData.PassportNumber} стало {PersDataModel.PassportNumber} \r\n");
                if (PersData?.PassportIssued != PersDataModel.PassportIssued && (!string.IsNullOrEmpty(PersDataModel.PassportIssued) || IgnorNull)) 
                    Result.Append($"Изменили паспорт выдан: было {PersData.PassportIssued} стало {PersDataModel.PassportIssued} \r\n");
                if (PersData?.PassportDate != PersDataModel.PassportDate && (PersDataModel.PassportDate != null || IgnorNull)) 
                    Result.Append($"Изменили паспорт дату: было {PersData.PassportDate} стало {PersDataModel.PassportDate} \r\n");
                if (PersData?.Tel1 != PersDataModel.Tel1 && (!string.IsNullOrEmpty(PersDataModel.Tel1) || IgnorNull)) 
                    Result.Append($"Изменили телефон1: было {PersData.Tel1} стало {PersDataModel.Tel1} \r\n");
                if (PersData?.Comment1 != PersDataModel.Comment1 && (!string.IsNullOrEmpty(PersDataModel.Comment1) || IgnorNull)) 
                    Result.Append($"Изменили комент к телефон1: было {PersData.Comment1} стало {PersDataModel.Comment1} \r\n");
                if (PersData?.Tel2 != PersDataModel.Tel2 && (!string.IsNullOrEmpty(PersDataModel.Tel2) || IgnorNull))
                    Result.Append($"Изменили телефон2: было {PersData.Tel2} стало {PersDataModel.Tel2} \r\n");
                if (PersData?.Comment2 != PersDataModel.Comment2 && (!string.IsNullOrEmpty(PersDataModel.Comment2) || IgnorNull)) 
                    Result.Append($"Изменили комент к телефон2: было {PersData.Comment2} стало {PersDataModel.Comment2} \r\n");
                if (PersData?.Email != PersDataModel.Email && (!string.IsNullOrEmpty(PersDataModel.Email) || IgnorNull)) 
                    Result.Append($"Изменили Email: было {PersData.Email} стало {PersDataModel.Email} \r\n");
                if (PersData?.Comment != PersDataModel.Comment && (!string.IsNullOrEmpty(PersDataModel.Comment) || IgnorNull)) 
                    Result.Append($"Изменили коментарий: было {PersData.Comment} стало {PersDataModel.Comment} \r\n");
                //if (PersData.UserName != PersDataModel.UserName && !string.IsNullOrEmpty(PersDataModel.UserName)) Result.Append($"Изменили номер ПУ: было {PersData.UserName} стало {PersDataModel.UserName} \r\n");
                if (PersData?.RoomType != PersDataModel.RoomType && (!string.IsNullOrEmpty(PersDataModel.RoomType) || IgnorNull)) 
                    Result.Append($"Изменили собственика: было {PersData.RoomType} стало {PersDataModel.RoomType} \r\n");
                //if (PersData.Main != PersDataModel.Main && !string.IsNullOrEmpty(PersDataModel.Main)) Result.Append($"Изменили номер ПУ: было {PersData.Main} стало {PersDataModel.Main} \r\n");
                if (PersData?.SnilsNumber != PersDataModel.SnilsNumber && (!string.IsNullOrEmpty(PersDataModel.SnilsNumber) || IgnorNull)) 
                    Result.Append($"Изменили снилс: было {PersData.SnilsNumber} стало {PersDataModel.SnilsNumber} \r\n");
                if (PersData?.Inn != PersDataModel.Inn && (!string.IsNullOrEmpty(PersDataModel.Inn) || IgnorNull))
                    Result.Append($"Изменили инн: было {PersData.Inn} стало {PersDataModel.Inn} \r\n");
                if (PersData?.NumberOfPersons != PersDataModel.NumberOfPersons && (PersDataModel.NumberOfPersons != null || IgnorNull))
                    Result.Append($"Изменили количество человек: было {PersData.NumberOfPersons} стало {PersDataModel.NumberOfPersons} \r\n");
                if (PersData?.Square != PersDataModel.Square && (PersDataModel.Square != null || IgnorNull)) 
                    Result.Append($"Изменили площадь: было {PersData.Square} стало {PersDataModel.Square} \r\n");
                if (PersData?.SendingElectronicReceipt != PersDataModel.SendingElectronicReceipt && (!string.IsNullOrEmpty(PersDataModel.SendingElectronicReceipt) || IgnorNull))
                    Result.Append($"Изменили отправка эл/квитанции: было {PersData.SendingElectronicReceipt} стало {PersDataModel.SendingElectronicReceipt} \r\n");
            }
            return Result.ToString();
        }

        public string Generate(CourtGeneralInformation courtGeneralBe, DB.Model.Court.CourtGeneralInformation courtGeneralDb, string User)
        {
            StringBuilder Result = new StringBuilder();
            Result.AppendLine($"<b>{DateTime.Now} Пользователь {User} изменил:</b>");
            if (courtGeneralBe.Pensioner != courtGeneralDb.Pensioner)
                Result.AppendLine($"Пенсионер: было {courtGeneralDb.Pensioner} стало {courtGeneralBe.Pensioner}");
            if (courtGeneralBe.Inn != courtGeneralDb.Inn)
                Result.AppendLine($"Инн: было {courtGeneralDb.Inn} стало {courtGeneralBe.Inn}");
            if (courtGeneralBe.FirstName != courtGeneralDb.FirstName)
                Result.AppendLine($"Имя должника: было {courtGeneralDb.FirstName} стало {courtGeneralBe.FirstName}");
            if (courtGeneralBe.LastName != courtGeneralDb.LastName)
                Result.AppendLine($"Фамилия должника: было {courtGeneralDb.LastName} стало {courtGeneralBe.LastName}");
            if (courtGeneralBe.Surname != courtGeneralDb.Surname)
                Result.AppendLine($"Отчество должника: было {courtGeneralDb.Surname} стало {courtGeneralBe.Surname}");
            if (courtGeneralBe.Region != courtGeneralDb.Region)
                Result.AppendLine($"Регион: было {courtGeneralDb.Region} стало {courtGeneralBe.Region}");
            if (courtGeneralBe.City != courtGeneralDb.City)
                Result.AppendLine($"Город: было {courtGeneralDb.City} стало {courtGeneralBe.City}");
            if (courtGeneralBe.Street != courtGeneralDb.Street)
                Result.AppendLine($"Улица: было {courtGeneralDb.Street} стало {courtGeneralBe.Street}");
            if (courtGeneralBe.Home != courtGeneralDb.Home)
                Result.AppendLine($"Дом: было {courtGeneralDb.Home} стало {courtGeneralBe.Home}");
            if (courtGeneralBe.Flat != courtGeneralDb.Flat)
                Result.AppendLine($"Квартира: было {courtGeneralDb.Flat} стало {courtGeneralBe.Flat}");
            if (courtGeneralBe.Floor != courtGeneralDb.Floor)
                Result.AppendLine($"Пол: было {courtGeneralDb.Floor} стало {courtGeneralBe.Floor}");
            if (courtGeneralBe.ShareOfOwnership != courtGeneralDb.ShareOfOwnership)
                Result.AppendLine($"Вид собственности: было {courtGeneralDb.ShareOfOwnership} стало {courtGeneralBe.ShareOfOwnership}");
            if (courtGeneralBe.CadastrNumber != courtGeneralDb.CadastrNumber)
                Result.AppendLine($"Кадастровый номер: было {courtGeneralDb.CadastrNumber} стало {courtGeneralBe.CadastrNumber}");
            if (courtGeneralBe.AddressRegister != courtGeneralDb.AddressRegister)
                Result.AppendLine($"Адрес регистрации: было {courtGeneralDb.AddressRegister} стало {courtGeneralBe.AddressRegister}");
            if (courtGeneralBe.DateDeath != courtGeneralDb.DateDeath)
                Result.AppendLine($"Дата смерти: было {courtGeneralDb.DateDeath} стало {courtGeneralBe.DateDeath}");
            if (courtGeneralBe.DateBirthday != courtGeneralDb.DateBirthday)
                Result.AppendLine($"Дата рождения: было {courtGeneralDb.DateBirthday} стало {courtGeneralBe.DateBirthday}");
            if (courtGeneralBe.PasportDate != courtGeneralDb.PasportDate)
                Result.AppendLine($"Дата выдачи паспорта: было {courtGeneralDb.PasportDate} стало {courtGeneralBe.PasportDate}");
            if (courtGeneralBe.PasportSeria != courtGeneralDb.PasportSeria)
                Result.AppendLine($"Серия паспорта: было {courtGeneralDb.PasportSeria} стало {courtGeneralBe.PasportSeria}");
            if (courtGeneralBe.PasportNumber != courtGeneralDb.PasportNumber)
                Result.AppendLine($"Номер паспорта: было {courtGeneralDb.PasportNumber} стало {courtGeneralBe.PasportNumber}");
            if (courtGeneralBe.PasportIssue != courtGeneralDb.PasportIssue)
                Result.AppendLine($"Паспорт кем выдан: было {courtGeneralDb.PasportIssue} стало {courtGeneralBe.PasportIssue}");
            if (courtGeneralBe.Snils != courtGeneralDb.Snils)
                Result.AppendLine($"СНИЛС: было {courtGeneralDb.Snils} стало {courtGeneralBe.Snils}");
            if (courtGeneralBe.ExclusionMailing != courtGeneralDb.ExclusionMailing)
                Result.AppendLine($"Исключение из рассылки: было {courtGeneralDb.ExclusionMailing} стало {courtGeneralBe.ExclusionMailing}");
            if (courtGeneralBe.ReasonsExclusionMailing != courtGeneralDb.ReasonsExclusionMailing)
                Result.AppendLine($"Причины исключения из рассылки: было {courtGeneralDb.ReasonsExclusionMailing} стало {courtGeneralBe.ReasonsExclusionMailing}");
            if (courtGeneralBe.ExclusionCourtWork != courtGeneralDb.ExclusionCourtWork)
                Result.AppendLine($"Исключение из судебной работы: было {courtGeneralDb.ExclusionCourtWork} стало {courtGeneralBe.ExclusionCourtWork}");
            if (courtGeneralBe.ReasonsCourtWork != courtGeneralDb.ReasonsCourtWork)
                Result.AppendLine($"Причины исключения из судебной работы: было {courtGeneralDb.ReasonsCourtWork} стало {courtGeneralBe.ReasonsCourtWork}");
            if (courtGeneralBe.Comment != courtGeneralDb.Comment)
                Result.AppendLine($"Примечание: было {courtGeneralDb.Comment} стало {courtGeneralBe.Comment}");
            if (courtGeneralBe.DateCreate != courtGeneralDb.DateCreate)
                Result.AppendLine($"Дата создания: было {courtGeneralDb.DateCreate} стало {courtGeneralBe.DateCreate}");
            if (courtGeneralBe.StatusCard != courtGeneralDb.StatusCard)
                Result.AppendLine($"Статус карточки: было {courtGeneralDb.StatusCard} стало {courtGeneralBe.StatusCard}");
            if (courtGeneralBe.ShareInRight != courtGeneralDb.ShareInRight)
                Result.AppendLine($"Доля в праве: было {courtGeneralDb.ShareInRight} стало {courtGeneralBe.ShareInRight}");
            if (courtGeneralBe.InSolidarityWith != courtGeneralDb.InSolidarityWith)
                Result.AppendLine($"Солидарно с: было {courtGeneralDb.InSolidarityWith} стало {courtGeneralBe.InSolidarityWith}");
            //.CourtWork
            if (courtGeneralBe.CourtWork.SumDebtNowDate != courtGeneralDb.CourtWork.SumDebtNowDate)
                Result.AppendLine($"Текущая сумма задолженности: было {courtGeneralDb.CourtWork.SumDebtNowDate} стало {courtGeneralBe.CourtWork.SumDebtNowDate}");
            if (courtGeneralBe.CourtWork.SumDebtSendCourt != courtGeneralDb.CourtWork.SumDebtSendCourt)
                Result.AppendLine($"Сумма долга отправленная суду: было {courtGeneralDb.CourtWork.SumDebtSendCourt} стало {courtGeneralBe.CourtWork.SumDebtSendCourt}");
            if (courtGeneralBe.CourtWork.SumOdSendCourt != courtGeneralDb.CourtWork.SumOdSendCourt)
                Result.AppendLine($"Сумма основного долга отправленная суду: было {courtGeneralDb.CourtWork.SumOdSendCourt} стало {courtGeneralBe.CourtWork.SumOdSendCourt}");
            if (courtGeneralBe.CourtWork.SumPenySendCourt != courtGeneralDb.CourtWork.SumPenySendCourt)
                Result.AppendLine($"Сумма пеней отправленных суду: было {courtGeneralDb.CourtWork.SumPenySendCourt} стало {courtGeneralBe.CourtWork.SumPenySendCourt}");
            if (courtGeneralBe.CourtWork.SumGP != courtGeneralDb.CourtWork.SumGP)
                Result.AppendLine($"Сумма госпошлины: было {courtGeneralDb.CourtWork.SumGP} стало {courtGeneralBe.CourtWork.SumGP}");
            if (courtGeneralBe.CourtWork.RequisitesSumGP != courtGeneralDb.CourtWork.RequisitesSumGP)
                Result.AppendLine($"Реквизиты суммы госпошлины: было {courtGeneralDb.CourtWork.RequisitesSumGP} стало {courtGeneralBe.CourtWork.RequisitesSumGP}");
            if (courtGeneralBe.CourtWork.RequisitesDateGP != courtGeneralDb.CourtWork.RequisitesDateGP)
                Result.AppendLine($"Реквизиты даты госпошлины: было {courtGeneralDb.CourtWork.RequisitesDateGP} стало {courtGeneralBe.CourtWork.RequisitesDateGP}");
            if (courtGeneralBe.CourtWork.RequisitesNumberGP != courtGeneralDb.CourtWork.RequisitesNumberGP)
                Result.AppendLine($"Реквизиты номера госпошлины: было {courtGeneralDb.CourtWork.RequisitesNumberGP} стало {courtGeneralBe.CourtWork.RequisitesNumberGP}");
            if (courtGeneralBe.CourtWork.AmountOverpaidGP != courtGeneralDb.CourtWork.AmountOverpaidGP)
                Result.AppendLine($"Сумма переплаты госпошлины: было {courtGeneralDb.CourtWork.AmountOverpaidGP} стало {courtGeneralBe.CourtWork.AmountOverpaidGP}");
            if (courtGeneralBe.CourtWork.PeriodDebtBegin != courtGeneralDb.CourtWork.PeriodDebtBegin)
                Result.AppendLine($"Начало периода задолженности: было {courtGeneralDb.CourtWork.PeriodDebtBegin} стало {courtGeneralBe.CourtWork.PeriodDebtBegin}");
            if (courtGeneralBe.CourtWork.PeriodDebtEnd != courtGeneralDb.CourtWork.PeriodDebtEnd)
                Result.AppendLine($"Конец периода задолженности: было {courtGeneralDb.CourtWork.PeriodDebtEnd} стало {courtGeneralBe.CourtWork.PeriodDebtEnd}");
            if (courtGeneralBe.CourtWork.FioSendCourt != courtGeneralDb.CourtWork.FioSendCourt)
                Result.AppendLine($"ФИО отправленное в суд: было {courtGeneralDb.CourtWork.FioSendCourt} стало {courtGeneralBe.CourtWork.FioSendCourt}");
            if (courtGeneralBe.CourtWork.SubmitApplicationCourt != courtGeneralDb.CourtWork.SubmitApplicationCourt)
                Result.AppendLine($"Подача заявления в суд: было {courtGeneralDb.CourtWork.SubmitApplicationCourt} стало {courtGeneralBe.CourtWork.SubmitApplicationCourt}");
            if (courtGeneralBe.CourtWork.NameCourt != courtGeneralDb.CourtWork.NameCourt)
                Result.AppendLine($"Название суда: было {courtGeneralDb.CourtWork.NameCourt} стало {courtGeneralBe.CourtWork.NameCourt}");
            if (courtGeneralBe.CourtWork.AddressCourt != courtGeneralDb.CourtWork.AddressCourt)
                Result.AppendLine($"Адрес суда: было {courtGeneralDb.CourtWork.AddressCourt} стало {courtGeneralBe.CourtWork.AddressCourt}");
            if (courtGeneralBe.CourtWork.DateReceptionCourt != courtGeneralDb.CourtWork.DateReceptionCourt)
                Result.AppendLine($"Дата получения судом: было {courtGeneralDb.CourtWork.DateReceptionCourt} стало {courtGeneralBe.CourtWork.DateReceptionCourt}");
            if (courtGeneralBe.CourtWork.DateReturnCourtSP != courtGeneralDb.CourtWork.DateReturnCourtSP)
                Result.AppendLine($"Дата возврата судом судопроизводства: было {courtGeneralDb.CourtWork.DateReturnCourtSP} стало {courtGeneralBe.CourtWork.DateReturnCourtSP}");
            if (courtGeneralBe.CourtWork.ReasonReturningApplication != courtGeneralDb.CourtWork.ReasonReturningApplication)
                Result.AppendLine($"Причина возврата заявления: было {courtGeneralDb.CourtWork.ReasonReturningApplication} стало {courtGeneralBe.CourtWork.ReasonReturningApplication}");
            if (courtGeneralBe.CourtWork.NumberSP != courtGeneralDb.CourtWork.NumberSP)
                Result.AppendLine($"Номер судопроизводства: было {courtGeneralDb.CourtWork.NumberSP} стало {courtGeneralBe.CourtWork.NumberSP}");
            if (courtGeneralBe.CourtWork.DateSP != courtGeneralDb.CourtWork.DateSP)
                Result.AppendLine($"Дата судопроизводства: было {courtGeneralDb.CourtWork.DateSP} стало {courtGeneralBe.CourtWork.DateSP}");
            if (courtGeneralBe.CourtWork.SumPayAll != courtGeneralDb.CourtWork.SumPayAll)
                Result.AppendLine($"Всего оплачено: было {courtGeneralDb.CourtWork.SumPayAll} стало {courtGeneralBe.CourtWork.SumPayAll}");
            if (courtGeneralBe.CourtWork.SumPayOD != courtGeneralDb.CourtWork.SumPayOD)
                Result.AppendLine($"Сумма оплаты основного долга: было {courtGeneralDb.CourtWork.SumPayOD} стало {courtGeneralBe.CourtWork.SumPayOD}");
            if (courtGeneralBe.CourtWork.SumPayPeny != courtGeneralDb.CourtWork.SumPayPeny)
                Result.AppendLine($"Сумма оплаты пени: было {courtGeneralDb.CourtWork.SumPayPeny} стало {courtGeneralBe.CourtWork.SumPayPeny}");
            if (courtGeneralBe.CourtWork.SumPayGP != courtGeneralDb.CourtWork.SumPayGP)
                Result.AppendLine($"Сумма оплаты госпошлины: было {courtGeneralDb.CourtWork.SumPayGP} стало {courtGeneralBe.CourtWork.SumPayGP}");
            if (courtGeneralBe.CourtWork.Comment != courtGeneralDb.CourtWork.Comment)
                Result.AppendLine($"Комментарий судебной работы: было {courtGeneralDb.CourtWork.Comment} стало {courtGeneralBe.CourtWork.Comment}");
            //.CourtExecutionFSSP
            if (courtGeneralBe.CourtExecutionFSSP.FioSendSpIo != courtGeneralDb.CourtExecutionFSSP.FioSendSpIo)
                Result.AppendLine($"ФИО сотрудника (направившего СП в ИО): было {courtGeneralDb.CourtExecutionFSSP.FioSendSpIo} стало {courtGeneralBe.CourtExecutionFSSP.FioSendSpIo}");
            if (courtGeneralBe.CourtExecutionFSSP.ExecutiveBody != courtGeneralDb.CourtExecutionFSSP.ExecutiveBody)
                Result.AppendLine($"Исполнительный орган (ФССП, ПФ, Банк): было {courtGeneralDb.CourtExecutionFSSP.ExecutiveBody} стало {courtGeneralBe.CourtExecutionFSSP.ExecutiveBody}");
            if (courtGeneralBe.CourtExecutionFSSP.AddressIO != courtGeneralDb.CourtExecutionFSSP.AddressIO)
                Result.AppendLine($"Адрес ИО: было {courtGeneralDb.CourtExecutionFSSP.AddressIO} стало {courtGeneralBe.CourtExecutionFSSP.AddressIO}");
            if (courtGeneralBe.CourtExecutionFSSP.DateSendingApplicationFSSP != courtGeneralDb.CourtExecutionFSSP.DateSendingApplicationFSSP)
                Result.AppendLine($"Дата отправки заявления в ФССП: было {courtGeneralDb.CourtExecutionFSSP.DateSendingApplicationFSSP} стало {courtGeneralBe.CourtExecutionFSSP.DateSendingApplicationFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.SendApplicationFSSP != courtGeneralDb.CourtExecutionFSSP.SendApplicationFSSP)
                Result.AppendLine($"Способ отправки заявления в ФССП: было {courtGeneralDb.CourtExecutionFSSP.SendApplicationFSSP} стало {courtGeneralBe.CourtExecutionFSSP.SendApplicationFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationAll != courtGeneralDb.CourtExecutionFSSP.SumApplicationAll)
                Result.AppendLine($"Сумма по заявлению в ФССП - всего: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationAll} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationAll}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationOd != courtGeneralDb.CourtExecutionFSSP.SumApplicationOd)
                Result.AppendLine($"Сумма по заявлению в ФССП - ОД: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationOd} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationOd}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationPeny != courtGeneralDb.CourtExecutionFSSP.SumApplicationPeny)
                Result.AppendLine($"Сумма по заявлению в ФССП - пени: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationPeny} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationPeny}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationGp != courtGeneralDb.CourtExecutionFSSP.SumApplicationGp)
                Result.AppendLine($"Сумма по заявлению в ФССП - ГП: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationGp} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationGp}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationPFAll != courtGeneralDb.CourtExecutionFSSP.SumApplicationPFAll)
                Result.AppendLine($"Сумма по заявлению в ПФ - всего: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationPFAll} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationPFAll}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationPFOd != courtGeneralDb.CourtExecutionFSSP.SumApplicationPFOd)
                Result.AppendLine($"Сумма по заявлению в ПФ - ОД: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationPFOd} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationPFOd}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationPFPeny != courtGeneralDb.CourtExecutionFSSP.SumApplicationPFPeny)
                Result.AppendLine($"Сумма по заявлению в ПФ - пени: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationPFPeny} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationPFPeny}");
            if (courtGeneralBe.CourtExecutionFSSP.SumApplicationPFGp != courtGeneralDb.CourtExecutionFSSP.SumApplicationPFGp)
                Result.AppendLine($"Сумма по заявлению в ПФ - ГП: было {courtGeneralDb.CourtExecutionFSSP.SumApplicationPFGp} стало {courtGeneralBe.CourtExecutionFSSP.SumApplicationPFGp}");
            if (courtGeneralBe.CourtExecutionFSSP.NumberIP != courtGeneralDb.CourtExecutionFSSP.NumberIP)
                Result.AppendLine($"Номер ИП1: было {courtGeneralDb.CourtExecutionFSSP.NumberIP} стало {courtGeneralBe.CourtExecutionFSSP.NumberIP}");
            if (courtGeneralBe.CourtExecutionFSSP.IPInitiationDate != courtGeneralDb.CourtExecutionFSSP.IPInitiationDate)
                Result.AppendLine($"Дата возбуждения ИП1: было {courtGeneralDb.CourtExecutionFSSP.IPInitiationDate} стало {courtGeneralBe.CourtExecutionFSSP.IPInitiationDate}");
            if (courtGeneralBe.CourtExecutionFSSP.SumDecisionInitateIP != courtGeneralDb.CourtExecutionFSSP.SumDecisionInitateIP)
                Result.AppendLine($"Сумма в постановлении о возбуждении ИП - всего: было {courtGeneralDb.CourtExecutionFSSP.SumDecisionInitateIP} стало {courtGeneralBe.CourtExecutionFSSP.SumDecisionInitateIP}");
            if (courtGeneralBe.CourtExecutionFSSP.IPEndDate != courtGeneralDb.CourtExecutionFSSP.IPEndDate)
                Result.AppendLine($"Дата окончания ИП1: было {courtGeneralDb.CourtExecutionFSSP.IPEndDate} стало {courtGeneralBe.CourtExecutionFSSP.IPEndDate}");
            if (courtGeneralBe.CourtExecutionFSSP.GroundsEndingIP != courtGeneralDb.CourtExecutionFSSP.GroundsEndingIP)
                Result.AppendLine($"Основание окончания ИП1: было {courtGeneralDb.CourtExecutionFSSP.GroundsEndingIP} стало {courtGeneralBe.CourtExecutionFSSP.GroundsEndingIP}");
            if (courtGeneralBe.CourtExecutionFSSP.IPExecutionDate != courtGeneralDb.CourtExecutionFSSP.IPExecutionDate)
                Result.AppendLine($"Дата отзыва ИД с исполнения: было {courtGeneralDb.CourtExecutionFSSP.IPExecutionDate} стало {courtGeneralBe.CourtExecutionFSSP.IPExecutionDate}");
            if (courtGeneralBe.CourtExecutionFSSP.ReasonExecutionIP != courtGeneralDb.CourtExecutionFSSP.ReasonExecutionIP)
                Result.AppendLine($"Причина отзыва ИД с исполнения: было {courtGeneralDb.CourtExecutionFSSP.ReasonExecutionIP} стало {courtGeneralBe.CourtExecutionFSSP.ReasonExecutionIP}");
            if (courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDEndIP != courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDEndIP)
                Result.AppendLine($"Дата поступления оригиналов ИД при окончании ИП1: было {courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDEndIP} стало {courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDEndIP}");
            if (courtGeneralBe.CourtExecutionFSSP.DateRefusalInitiateIP != courtGeneralDb.CourtExecutionFSSP.DateRefusalInitiateIP)
                Result.AppendLine($"Дата отказа в возбуждении ИП1: было {courtGeneralDb.CourtExecutionFSSP.DateRefusalInitiateIP} стало {courtGeneralBe.CourtExecutionFSSP.DateRefusalInitiateIP}");
            if (courtGeneralBe.CourtExecutionFSSP.GroundsRefusalInitiateIP != courtGeneralDb.CourtExecutionFSSP.GroundsRefusalInitiateIP)
                Result.AppendLine($"Основание отказа в возбуждении ИП1: было {courtGeneralDb.CourtExecutionFSSP.GroundsRefusalInitiateIP} стало {courtGeneralBe.CourtExecutionFSSP.GroundsRefusalInitiateIP}");
            if (courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP != courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP)
                Result.AppendLine($"Дата поступления оригинала ИД при отказе в возбуждении ИП1: было {courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP} стало {courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP}");

            if (courtGeneralBe.CourtExecutionFSSP.NumberIP2 != courtGeneralDb.CourtExecutionFSSP.NumberIP2)
                Result.AppendLine($"Номер ИП 2: было {courtGeneralDb.CourtExecutionFSSP.NumberIP2} стало {courtGeneralBe.CourtExecutionFSSP.NumberIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.IPInitiationDate2 != courtGeneralDb.CourtExecutionFSSP.IPInitiationDate2)
                Result.AppendLine($"Дата возбуждения ИП 2: было {courtGeneralDb.CourtExecutionFSSP.IPInitiationDate2} стало {courtGeneralBe.CourtExecutionFSSP.IPInitiationDate2}");
            if (courtGeneralBe.CourtExecutionFSSP.SumDecisionInitateIP2 != courtGeneralDb.CourtExecutionFSSP.SumDecisionInitateIP2)
                Result.AppendLine($"Сумма в постановлении о возбуждении ИП - всего 2: было {courtGeneralDb.CourtExecutionFSSP.SumDecisionInitateIP2} стало {courtGeneralBe.CourtExecutionFSSP.SumDecisionInitateIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.IPEndDate2 != courtGeneralDb.CourtExecutionFSSP.IPEndDate2)
                Result.AppendLine($"Дата окончания ИП 2: было {courtGeneralDb.CourtExecutionFSSP.IPEndDate2} стало {courtGeneralBe.CourtExecutionFSSP.IPEndDate2}");
            if (courtGeneralBe.CourtExecutionFSSP.GroundsEndingIP2 != courtGeneralDb.CourtExecutionFSSP.GroundsEndingIP2)
                Result.AppendLine($"Основание окончания ИП 2: было {courtGeneralDb.CourtExecutionFSSP.GroundsEndingIP2} стало {courtGeneralBe.CourtExecutionFSSP.GroundsEndingIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.IPExecutionDate2 != courtGeneralDb.CourtExecutionFSSP.IPExecutionDate2)
                Result.AppendLine($"Дата отзыва ИД с исполнения 2: было {courtGeneralDb.CourtExecutionFSSP.IPExecutionDate2} стало {courtGeneralBe.CourtExecutionFSSP.IPExecutionDate2}");
            if (courtGeneralBe.CourtExecutionFSSP.ReasonExecutionIP2 != courtGeneralDb.CourtExecutionFSSP.ReasonExecutionIP2)
                Result.AppendLine($"Причина отзыва ИД с исполнения 2: было {courtGeneralDb.CourtExecutionFSSP.ReasonExecutionIP2} стало {courtGeneralBe.CourtExecutionFSSP.ReasonExecutionIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDEndIP2 != courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDEndIP2)
                Result.AppendLine($"Дата поступления оригиналов ИД при окончании ИП 2: было {courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDEndIP2} стало {courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDEndIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.DateRefusalInitiateIP2 != courtGeneralDb.CourtExecutionFSSP.DateRefusalInitiateIP2)
                Result.AppendLine($"Дата отказа в возбуждении ИП 2: было {courtGeneralDb.CourtExecutionFSSP.DateRefusalInitiateIP2} стало {courtGeneralBe.CourtExecutionFSSP.DateRefusalInitiateIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.GroundsRefusalInitiateIP2 != courtGeneralDb.CourtExecutionFSSP.GroundsRefusalInitiateIP2)
                Result.AppendLine($"Основание отказа в возбуждении ИП 2: было {courtGeneralDb.CourtExecutionFSSP.GroundsRefusalInitiateIP2} стало {courtGeneralBe.CourtExecutionFSSP.GroundsRefusalInitiateIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP2 != courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP2)
                Result.AppendLine($"Дата поступления оригинала ИД при отказе в возбуждении ИП 2: было {courtGeneralDb.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP2} стало {courtGeneralBe.CourtExecutionFSSP.DateReceiptOriginalIDcaseRefusalInitiateIP2}");
            if (courtGeneralBe.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP != courtGeneralDb.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP)
                Result.AppendLine($"Статья отказа ИП 1: было {courtGeneralDb.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP} стало {courtGeneralBe.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP}");
            if (courtGeneralBe.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP2 != courtGeneralDb.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP2)
                Result.AppendLine($"Статья отказа ИП 2: было {courtGeneralDb.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP2} стало {courtGeneralBe.CourtExecutionFSSP.SatayGroundsRefusalInitiateIP2}");

            if (courtGeneralBe.CourtExecutionFSSP.FullNameDebtorIP != courtGeneralDb.CourtExecutionFSSP.FullNameDebtorIP)
                Result.AppendLine($"ФИО должника в ИП1: было {courtGeneralDb.CourtExecutionFSSP.FullNameDebtorIP} стало {courtGeneralBe.CourtExecutionFSSP.FullNameDebtorIP}");
            if (courtGeneralBe.CourtExecutionFSSP.IPDateBirth != courtGeneralDb.CourtExecutionFSSP.IPDateBirth)
                Result.AppendLine($"Дата рождения в ИП1: было {courtGeneralDb.CourtExecutionFSSP.IPDateBirth} стало {courtGeneralBe.CourtExecutionFSSP.IPDateBirth}");
            if (courtGeneralBe.CourtExecutionFSSP.SnilsIp != courtGeneralDb.CourtExecutionFSSP.SnilsIp)
                Result.AppendLine($"СНИЛС в ИП1: было {courtGeneralDb.CourtExecutionFSSP.SnilsIp} стало {courtGeneralBe.CourtExecutionFSSP.SnilsIp}");
            if (courtGeneralBe.CourtExecutionFSSP.InnIp != courtGeneralDb.CourtExecutionFSSP.InnIp)
                Result.AppendLine($"ИНН в ИП1: было {courtGeneralDb.CourtExecutionFSSP.InnIp} стало {courtGeneralBe.CourtExecutionFSSP.InnIp}");
            if (courtGeneralBe.CourtExecutionFSSP.PasportIp != courtGeneralDb.CourtExecutionFSSP.PasportIp)
                Result.AppendLine($"Паспорт в ИП1: было {courtGeneralDb.CourtExecutionFSSP.PasportIp} стало {courtGeneralBe.CourtExecutionFSSP.PasportIp}");
            if (courtGeneralBe.CourtExecutionFSSP.AddressIp != courtGeneralDb.CourtExecutionFSSP.AddressIp)
                Result.AppendLine($"Адрес в ИП1: было {courtGeneralDb.CourtExecutionFSSP.AddressIp} стало {courtGeneralBe.CourtExecutionFSSP.AddressIp}");
            if (courtGeneralBe.CourtExecutionFSSP.AccountInformation != courtGeneralDb.CourtExecutionFSSP.AccountInformation)
                Result.AppendLine($"Сведения о счетах: было {courtGeneralDb.CourtExecutionFSSP.AccountInformation} стало {courtGeneralBe.CourtExecutionFSSP.AccountInformation}");
            if (courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiffAccounts != courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiffAccounts)
                Result.AppendLine($"Дата принятой меры приставом по счетам: было {courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiffAccounts} стало {courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiffAccounts}");
            if (courtGeneralBe.CourtExecutionFSSP.ActionTakenBailiffAccounts != courtGeneralDb.CourtExecutionFSSP.ActionTakenBailiffAccounts)
                Result.AppendLine($"Принятая мера приставом по счетам: было {courtGeneralDb.CourtExecutionFSSP.ActionTakenBailiffAccounts} стало {courtGeneralBe.CourtExecutionFSSP.ActionTakenBailiffAccounts}");
            if (courtGeneralBe.CourtExecutionFSSP.InformationAboutRealRstate != courtGeneralDb.CourtExecutionFSSP.InformationAboutRealRstate)
                Result.AppendLine($"Сведения о недвижимости: было {courtGeneralDb.CourtExecutionFSSP.InformationAboutRealRstate} стало {courtGeneralBe.CourtExecutionFSSP.InformationAboutRealRstate}");
            if (courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiff != courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiff)
                Result.AppendLine($"Дата принятой меры приставом по недвижимости: было {courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiff} стало {courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiff}");
            if (courtGeneralBe.CourtExecutionFSSP.ActionTakenBailiff != courtGeneralDb.CourtExecutionFSSP.ActionTakenBailiff)
                Result.AppendLine($"Принятая мера приставом по недвижимости: было {courtGeneralDb.CourtExecutionFSSP.ActionTakenBailiff} стало {courtGeneralBe.CourtExecutionFSSP.ActionTakenBailiff}");
            if (courtGeneralBe.CourtExecutionFSSP.InformationAboutVehicle != courtGeneralDb.CourtExecutionFSSP.InformationAboutVehicle)
                Result.AppendLine($"Сведения о ТС: было {courtGeneralDb.CourtExecutionFSSP.InformationAboutVehicle} стало {courtGeneralBe.CourtExecutionFSSP.InformationAboutVehicle}");
            if (courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiffVehicle != courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiffVehicle)
                Result.AppendLine($"Дата принятой меры приставом по ТС: было {courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiffVehicle} стало {courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiffVehicle}");
            if (courtGeneralBe.CourtExecutionFSSP.PhoneNumbersDebtor != courtGeneralDb.CourtExecutionFSSP.PhoneNumbersDebtor)
                Result.AppendLine($"Номера телефонов должника: было {courtGeneralDb.CourtExecutionFSSP.PhoneNumbersDebtor} стало {courtGeneralBe.CourtExecutionFSSP.PhoneNumbersDebtor}");
            if (courtGeneralBe.CourtExecutionFSSP.IncomePension != courtGeneralDb.CourtExecutionFSSP.IncomePension)
                Result.AppendLine($"Доход/пенсия: было {courtGeneralDb.CourtExecutionFSSP.IncomePension} стало {courtGeneralBe.CourtExecutionFSSP.IncomePension}");
            if (courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiffIncome != courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiffIncome)
                Result.AppendLine($"Дата принятой меры приставом по доходам: было {courtGeneralDb.CourtExecutionFSSP.DateActionTakenBailiffIncome} стало {courtGeneralBe.CourtExecutionFSSP.DateActionTakenBailiffIncome}");
            if (courtGeneralBe.CourtExecutionFSSP.ActionTakenBailiffIncome != courtGeneralDb.CourtExecutionFSSP.ActionTakenBailiffIncome)
                Result.AppendLine($"Принятая мера приставом по доходам: было {courtGeneralDb.CourtExecutionFSSP.ActionTakenBailiffIncome} стало {courtGeneralBe.CourtExecutionFSSP.ActionTakenBailiffIncome}");
            if (courtGeneralBe.CourtExecutionFSSP.NameChange != courtGeneralDb.CourtExecutionFSSP.NameChange)
                Result.AppendLine($"Смена ФИО: было {courtGeneralDb.CourtExecutionFSSP.NameChange} стало {courtGeneralBe.CourtExecutionFSSP.NameChange}");
            if (courtGeneralBe.CourtExecutionFSSP.DeathRegistryOfficeData != courtGeneralDb.CourtExecutionFSSP.DeathRegistryOfficeData)
                Result.AppendLine($"Данные загс о смерти: было {courtGeneralDb.CourtExecutionFSSP.DeathRegistryOfficeData} стало {courtGeneralBe.CourtExecutionFSSP.DeathRegistryOfficeData}");
            if (courtGeneralBe.CourtExecutionFSSP.NumberInheritanceCase != courtGeneralDb.CourtExecutionFSSP.NumberInheritanceCase)
                Result.AppendLine($"№ наследственного дела: было {courtGeneralDb.CourtExecutionFSSP.NumberInheritanceCase} стало {courtGeneralBe.CourtExecutionFSSP.NumberInheritanceCase}");
            if (courtGeneralBe.CourtExecutionFSSP.FullNameNotary != courtGeneralDb.CourtExecutionFSSP.FullNameNotary)
                Result.AppendLine($"ФИО нотариуса: было {courtGeneralDb.CourtExecutionFSSP.FullNameNotary} стало {courtGeneralBe.CourtExecutionFSSP.FullNameNotary}");
            if (courtGeneralBe.CourtExecutionFSSP.MonthCheckInheritance != courtGeneralDb.CourtExecutionFSSP.MonthCheckInheritance)
                Result.AppendLine($"Месяц проверки наследственного дела: было {courtGeneralDb.CourtExecutionFSSP.MonthCheckInheritance} стало {courtGeneralBe.CourtExecutionFSSP.MonthCheckInheritance}");
            if (courtGeneralBe.CourtExecutionFSSP.FullNameHeir != courtGeneralDb.CourtExecutionFSSP.FullNameHeir)
                Result.AppendLine($"ФИО наследника: было {courtGeneralDb.CourtExecutionFSSP.FullNameHeir} стало {courtGeneralBe.CourtExecutionFSSP.FullNameHeir}");
            if (courtGeneralBe.CourtExecutionFSSP.DateActionsBailiff != courtGeneralDb.CourtExecutionFSSP.DateActionsBailiff)
                Result.AppendLine($"Дата прочих действий пристава: было {courtGeneralDb.CourtExecutionFSSP.DateActionsBailiff} стало {courtGeneralBe.CourtExecutionFSSP.DateActionsBailiff}");
            if (courtGeneralBe.CourtExecutionFSSP.ActionsBailiff != courtGeneralDb.CourtExecutionFSSP.ActionsBailiff)
                Result.AppendLine($"Прочие действия пристава: было {courtGeneralDb.CourtExecutionFSSP.ActionsBailiff} стало {courtGeneralBe.CourtExecutionFSSP.ActionsBailiff}");
            if (courtGeneralBe.CourtExecutionFSSP.SumPaymentAllFSSP != courtGeneralDb.CourtExecutionFSSP.SumPaymentAllFSSP)
                Result.AppendLine($"Сумма оплаты всего от ФССП: было {courtGeneralDb.CourtExecutionFSSP.SumPaymentAllFSSP} стало {courtGeneralBe.CourtExecutionFSSP.SumPaymentAllFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.SumPaymentODFSSP != courtGeneralDb.CourtExecutionFSSP.SumPaymentODFSSP)
                Result.AppendLine($"Сумма оплаты ОД от ФССП: было {courtGeneralDb.CourtExecutionFSSP.SumPaymentODFSSP} стало {courtGeneralBe.CourtExecutionFSSP.SumPaymentODFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.DatePaymentODFSSP != courtGeneralDb.CourtExecutionFSSP.DatePaymentODFSSP)
                Result.AppendLine($"Дата оплаты ОД от ФССП: было {courtGeneralDb.CourtExecutionFSSP.DatePaymentODFSSP} стало {courtGeneralBe.CourtExecutionFSSP.DatePaymentODFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.SumPaymentPenyFSSP != courtGeneralDb.CourtExecutionFSSP.SumPaymentPenyFSSP)
                Result.AppendLine($"Сумма оплаты пени от ФССП: было {courtGeneralDb.CourtExecutionFSSP.SumPaymentPenyFSSP} стало {courtGeneralBe.CourtExecutionFSSP.SumPaymentPenyFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.DatePaymentPenyFSSP != courtGeneralDb.CourtExecutionFSSP.DatePaymentPenyFSSP)
                Result.AppendLine($"Дата оплаты пени от ФССП: было {courtGeneralDb.CourtExecutionFSSP.DatePaymentPenyFSSP} стало {courtGeneralBe.CourtExecutionFSSP.DatePaymentPenyFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.SumPaymentGpFSSP != courtGeneralDb.CourtExecutionFSSP.SumPaymentGpFSSP)
                Result.AppendLine($"Сумма оплаты ГП от ФССП: было {courtGeneralDb.CourtExecutionFSSP.SumPaymentGpFSSP} стало {courtGeneralBe.CourtExecutionFSSP.SumPaymentGpFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.DatePaymentGpFSSP != courtGeneralDb.CourtExecutionFSSP.DatePaymentGpFSSP)
                Result.AppendLine($"Дата оплаты ГП от ФССП: было {courtGeneralDb.CourtExecutionFSSP.DatePaymentGpFSSP} стало {courtGeneralBe.CourtExecutionFSSP.DatePaymentGpFSSP}");
            if (courtGeneralBe.CourtExecutionFSSP.DateApplication != courtGeneralDb.CourtExecutionFSSP.DateApplication)
                Result.AppendLine($"Дата обращения: было {courtGeneralDb.CourtExecutionFSSP.DateApplication} стало {courtGeneralBe.CourtExecutionFSSP.DateApplication}");
            if (courtGeneralBe.CourtExecutionFSSP.BriefAppeal != courtGeneralDb.CourtExecutionFSSP.BriefAppeal)
                Result.AppendLine($"Краткая суть обращения: было {courtGeneralDb.CourtExecutionFSSP.BriefAppeal} стало {courtGeneralBe.CourtExecutionFSSP.BriefAppeal}");
            if (courtGeneralBe.CourtExecutionFSSP.DateApplicationSubmission != courtGeneralDb.CourtExecutionFSSP.DateApplicationSubmission)
                Result.AppendLine($"Дата подачи обращения: было {courtGeneralDb.CourtExecutionFSSP.DateApplicationSubmission} стало {courtGeneralBe.CourtExecutionFSSP.DateApplicationSubmission}");
            if (courtGeneralBe.CourtExecutionFSSP.ApplicationSubmissionMethod != courtGeneralDb.CourtExecutionFSSP.ApplicationSubmissionMethod)
                Result.AppendLine($"Способ подачи обращения: было {courtGeneralDb.CourtExecutionFSSP.ApplicationSubmissionMethod} стало {courtGeneralBe.CourtExecutionFSSP.ApplicationSubmissionMethod}");
            if (courtGeneralBe.CourtExecutionFSSP.DateReasonAppealActual != courtGeneralDb.CourtExecutionFSSP.DateReasonAppealActual)
                Result.AppendLine($"Дата ответа на обращение (фактическая): было {courtGeneralDb.CourtExecutionFSSP.DateReasonAppealActual} стало {courtGeneralBe.CourtExecutionFSSP.DateReasonAppealActual}");
            if (courtGeneralBe.CourtExecutionFSSP.BriefSummaryResponseAppeal != courtGeneralDb.CourtExecutionFSSP.BriefSummaryResponseAppeal)
                Result.AppendLine($"Краткая суть ответа на обращение: было {courtGeneralDb.CourtExecutionFSSP.BriefSummaryResponseAppeal} стало {courtGeneralBe.CourtExecutionFSSP.BriefSummaryResponseAppeal}");
            if (courtGeneralBe.CourtExecutionFSSP.AdditionalInformation != courtGeneralDb.CourtExecutionFSSP.AdditionalInformation)
                Result.AppendLine($"Дополнительные сведения (ФССП): было {courtGeneralDb.CourtExecutionFSSP.AdditionalInformation} стало {courtGeneralBe.CourtExecutionFSSP.AdditionalInformation}");
            //CourtExecutionInPF
            if (courtGeneralBe.CourtExecutionInPF.FioSendSpInIo != courtGeneralDb.CourtExecutionInPF.FioSendSpInIo)
                Result.AppendLine($"ФИО сотрудника (направившего СП в ИО): было {courtGeneralDb.CourtExecutionInPF.FioSendSpInIo} стало {courtGeneralBe.CourtExecutionInPF.FioSendSpInIo}");
            if (courtGeneralBe.CourtExecutionInPF.ExecutiveAgency != courtGeneralDb.CourtExecutionInPF.ExecutiveAgency)
                Result.AppendLine($"Исполнительный орган (ФССП, ПФ, Банк): было {courtGeneralDb.CourtExecutionInPF.ExecutiveAgency} стало {courtGeneralBe.CourtExecutionInPF.ExecutiveAgency}");
            if (courtGeneralBe.CourtExecutionInPF.AdresIo != courtGeneralDb.CourtExecutionInPF.AdresIo)
                Result.AppendLine($"Адрес ИО: было {courtGeneralDb.CourtExecutionInPF.AdresIo} стало {courtGeneralBe.CourtExecutionInPF.AdresIo}");
            if (courtGeneralBe.CourtExecutionInPF.DateSendApplicationIdInPf != courtGeneralDb.CourtExecutionInPF.DateSendApplicationIdInPf)
                Result.AppendLine($"Дата отправки заявления+ИД в ПФ о взыскании: было {courtGeneralDb.CourtExecutionInPF.DateSendApplicationIdInPf} стало {courtGeneralBe.CourtExecutionInPF.DateSendApplicationIdInPf}");
            if (courtGeneralBe.CourtExecutionInPF.WaySendOriginalApplicationIdInPf != courtGeneralDb.CourtExecutionInPF.WaySendOriginalApplicationIdInPf)
                Result.AppendLine($"Способ отправки оригиналов заявления+ИД в ПФ: было {courtGeneralDb.CourtExecutionInPF.WaySendOriginalApplicationIdInPf} стало {courtGeneralBe.CourtExecutionInPF.WaySendOriginalApplicationIdInPf}");
            if (courtGeneralBe.CourtExecutionInPF.SumApplicationPfAll != courtGeneralDb.CourtExecutionInPF.SumApplicationPfAll)
                Result.AppendLine($"Сумма по заявлению в ПФ - всего: было {courtGeneralDb.CourtExecutionInPF.SumApplicationPfAll} стало {courtGeneralBe.CourtExecutionInPF.SumApplicationPfAll}");
            if (courtGeneralBe.CourtExecutionInPF.SumApplicationPfOd != courtGeneralDb.CourtExecutionInPF.SumApplicationPfOd)
                Result.AppendLine($"Сумма по заявлению в ПФ - ОД: было {courtGeneralDb.CourtExecutionInPF.SumApplicationPfOd} стало {courtGeneralBe.CourtExecutionInPF.SumApplicationPfOd}");
            if (courtGeneralBe.CourtExecutionInPF.SumApplicationPfPeny != courtGeneralDb.CourtExecutionInPF.SumApplicationPfPeny)
                Result.AppendLine($"Сумма по заявлению в ПФ - пени: было {courtGeneralDb.CourtExecutionInPF.SumApplicationPfPeny} стало {courtGeneralBe.CourtExecutionInPF.SumApplicationPfPeny}");
            if (courtGeneralBe.CourtExecutionInPF.SumApplicationPfGp != courtGeneralDb.CourtExecutionInPF.SumApplicationPfGp)
                Result.AppendLine($"Сумма по заявлению в ПФ - ГП: было {courtGeneralDb.CourtExecutionInPF.SumApplicationPfGp} стало {courtGeneralBe.CourtExecutionInPF.SumApplicationPfGp}");
            if (courtGeneralBe.CourtExecutionInPF.DateReturnPF != courtGeneralDb.CourtExecutionInPF.DateReturnPF)
                Result.AppendLine($"Дата получения к исполнению ПФ: было {courtGeneralDb.CourtExecutionInPF.DateReturnPF} стало {courtGeneralBe.CourtExecutionInPF.DateReturnPF}");
            if (courtGeneralBe.CourtExecutionInPF.LengthStayDay != courtGeneralDb.CourtExecutionInPF.LengthStayDay)
                Result.AppendLine($"Срок нахождения на исполнени (дн.): было {courtGeneralDb.CourtExecutionInPF.LengthStayDay} стало {courtGeneralBe.CourtExecutionInPF.LengthStayDay}");
            if (courtGeneralBe.CourtExecutionInPF.DateReturnIdPF != courtGeneralDb.CourtExecutionInPF.DateReturnIdPF)
                Result.AppendLine($"Дата возврата ИД из ПФ: было {courtGeneralDb.CourtExecutionInPF.DateReturnIdPF} стало {courtGeneralBe.CourtExecutionInPF.DateReturnIdPF}");
            if (courtGeneralBe.CourtExecutionInPF.DateReturnId != courtGeneralDb.CourtExecutionInPF.DateReturnId)
                Result.AppendLine($"Дата отзыва ИД: было {courtGeneralDb.CourtExecutionInPF.DateReturnId} стало {courtGeneralBe.CourtExecutionInPF.DateReturnId}");
            if (courtGeneralBe.CourtExecutionInPF.ReasonReturnIdPf != courtGeneralDb.CourtExecutionInPF.ReasonReturnIdPf)
                Result.AppendLine($"Причина отзыва/возврата ИД из ПФ: было {courtGeneralDb.CourtExecutionInPF.ReasonReturnIdPf} стало {courtGeneralBe.CourtExecutionInPF.ReasonReturnIdPf}");
            if (courtGeneralBe.CourtExecutionInPF.NumberMailPfReturnId != courtGeneralDb.CourtExecutionInPF.NumberMailPfReturnId)
                Result.AppendLine($"Исх. № письма от ПФ о возврате ИД: было {courtGeneralDb.CourtExecutionInPF.NumberMailPfReturnId} стало {courtGeneralBe.CourtExecutionInPF.NumberMailPfReturnId}");
            if (courtGeneralBe.CourtExecutionInPF.SumExecutionPf != courtGeneralDb.CourtExecutionInPF.SumExecutionPf)
                Result.AppendLine($"Сумма исполнения от ПФ: было {courtGeneralDb.CourtExecutionInPF.SumExecutionPf} стало {courtGeneralBe.CourtExecutionInPF.SumExecutionPf}");
            if (courtGeneralBe.CourtExecutionInPF.Comment != courtGeneralDb.CourtExecutionInPF.Comment)
                Result.AppendLine($"Примечание (ПФ): было {courtGeneralDb.CourtExecutionInPF.Comment} стало {courtGeneralBe.CourtExecutionInPF.Comment}");
            //CourtInstallmentPlan
            if (courtGeneralBe.CourtInstallmentPlan.DateAcceptanceApplicationRestructuring != courtGeneralDb.CourtInstallmentPlan.DateAcceptanceApplicationRestructuring)
                Result.AppendLine($"Дата  принятия заявления на  реструктуризацию (рассрочку): было {courtGeneralDb.CourtInstallmentPlan.DateAcceptanceApplicationRestructuring} стало {courtGeneralBe.CourtInstallmentPlan.DateAcceptanceApplicationRestructuring}");
            if (courtGeneralBe.CourtInstallmentPlan.AmountRestructuring != courtGeneralDb.CourtInstallmentPlan.AmountRestructuring)
                Result.AppendLine($"Сумма Реструктуризации (рассрочки): было {courtGeneralDb.CourtInstallmentPlan.AmountRestructuring} стало {courtGeneralBe.CourtInstallmentPlan.AmountRestructuring}");
            if (courtGeneralBe.CourtInstallmentPlan.StartingMonthRestructuring != courtGeneralDb.CourtInstallmentPlan.StartingMonthRestructuring)
                Result.AppendLine($"Начальный месяц реструктуризации: было {courtGeneralDb.CourtInstallmentPlan.StartingMonthRestructuring} стало {courtGeneralBe.CourtInstallmentPlan.StartingMonthRestructuring}");
            if (courtGeneralBe.CourtInstallmentPlan.FinalMonthRestructuring != courtGeneralDb.CourtInstallmentPlan.FinalMonthRestructuring)
                Result.AppendLine($"Конечный месяц реструктуризации: было {courtGeneralDb.CourtInstallmentPlan.FinalMonthRestructuring} стало {courtGeneralBe.CourtInstallmentPlan.FinalMonthRestructuring}");
            if (courtGeneralBe.CourtInstallmentPlan.FinalMonthRestructuring != courtGeneralDb.CourtInstallmentPlan.FinalMonthRestructuring)
                Result.AppendLine($"Конечный месяц реструктуризации: было {courtGeneralDb.CourtInstallmentPlan.FinalMonthRestructuring} стало {courtGeneralBe.CourtInstallmentPlan.FinalMonthRestructuring}");
            if (courtGeneralBe.CourtInstallmentPlan.AmountMonthlyRestructuringPayment != courtGeneralDb.CourtInstallmentPlan.AmountMonthlyRestructuringPayment)
                Result.AppendLine($"Сумма ежемесячного платежа по реструктуризации: было {courtGeneralDb.CourtInstallmentPlan.AmountMonthlyRestructuringPayment} стало {courtGeneralBe.CourtInstallmentPlan.AmountMonthlyRestructuringPayment}");
            if (courtGeneralBe.CourtInstallmentPlan.DateStartPayment != courtGeneralDb.CourtInstallmentPlan.DateStartPayment)
                Result.AppendLine($"Дата начала платежей: было {courtGeneralDb.CourtInstallmentPlan.DateStartPayment} стало {courtGeneralBe.CourtInstallmentPlan.DateStartPayment}");
            if (courtGeneralBe.CourtInstallmentPlan.DateEndPayment != courtGeneralDb.CourtInstallmentPlan.DateEndPayment)
                Result.AppendLine($"Дата окончания платежей: было {courtGeneralDb.CourtInstallmentPlan.DateEndPayment} стало {courtGeneralBe.CourtInstallmentPlan.DateEndPayment}");
            if (courtGeneralBe.CourtInstallmentPlan.AmountPaymentRestructuring != courtGeneralDb.CourtInstallmentPlan.AmountPaymentRestructuring)
                Result.AppendLine($"Сумма оплаты по реструктуризации: было {courtGeneralDb.CourtInstallmentPlan.AmountPaymentRestructuring} стало {courtGeneralBe.CourtInstallmentPlan.AmountPaymentRestructuring}");
            if (courtGeneralBe.CourtInstallmentPlan.Comment != courtGeneralDb.CourtInstallmentPlan.Comment)
                Result.AppendLine($"Примечание (реструктуризация): было {courtGeneralDb.CourtInstallmentPlan.Comment} стало {courtGeneralBe.CourtInstallmentPlan.Comment}");
            //CourtBankruptcy
            if (courtGeneralBe.CourtBankruptcy.BankruptcyCaseNumber != courtGeneralDb.CourtBankruptcy.BankruptcyCaseNumber)
                Result.AppendLine($"№ банкротного дела: было {courtGeneralDb.CourtBankruptcy.BankruptcyCaseNumber} стало {courtGeneralBe.CourtBankruptcy.BankruptcyCaseNumber}");
            if (courtGeneralBe.CourtBankruptcy.DateDeterminationAcceptance != courtGeneralDb.CourtBankruptcy.DateDeterminationAcceptance)
                Result.AppendLine($"Дата  определения о принятии  заявления о банкротстве судом: было {courtGeneralDb.CourtBankruptcy.DateDeterminationAcceptance} стало {courtGeneralBe.CourtBankruptcy.DateDeterminationAcceptance}");
            if (courtGeneralBe.CourtBankruptcy.DateDeterminationCompletion != courtGeneralDb.CourtBankruptcy.DateDeterminationCompletion)
                Result.AppendLine($"Дата определения о завершении реализации имущества: было {courtGeneralDb.CourtBankruptcy.DateDeterminationCompletion} стало {courtGeneralBe.CourtBankruptcy.DateDeterminationCompletion}");
            if (courtGeneralBe.CourtBankruptcy.DateDeterminationApplication != courtGeneralDb.CourtBankruptcy.DateDeterminationApplication)
                Result.AppendLine($"Дата принятия заявления нами: было {courtGeneralDb.CourtBankruptcy.DateDeterminationApplication} стало {courtGeneralBe.CourtBankruptcy.DateDeterminationApplication}");
            if (courtGeneralBe.CourtBankruptcy.SumWriteOff != courtGeneralDb.CourtBankruptcy.SumWriteOff)
                Result.AppendLine($"Сумма списания: было {courtGeneralDb.CourtBankruptcy.SumWriteOff} стало {courtGeneralBe.CourtBankruptcy.SumWriteOff}");
            if (courtGeneralBe.CourtBankruptcy.DateWriteOffBegin != courtGeneralDb.CourtBankruptcy.DateWriteOffBegin)
                Result.AppendLine($"Начальный период списания: было {courtGeneralDb.CourtBankruptcy.DateWriteOffBegin} стало {courtGeneralBe.CourtBankruptcy.DateWriteOffBegin}");
            if (courtGeneralBe.CourtBankruptcy.DateWriteOffEnd != courtGeneralDb.CourtBankruptcy.DateWriteOffEnd)
                Result.AppendLine($"Конечный период списания: было {courtGeneralDb.CourtBankruptcy.DateWriteOffEnd} стало {courtGeneralBe.CourtBankruptcy.DateWriteOffEnd}");
            if (courtGeneralBe.CourtBankruptcy.WriteOffStatus != courtGeneralDb.CourtBankruptcy.WriteOffStatus)
                Result.AppendLine($"Статус списания : было {courtGeneralDb.CourtBankruptcy.WriteOffStatus} стало {courtGeneralBe.CourtBankruptcy.WriteOffStatus}");
            if (courtGeneralBe.CourtBankruptcy.DateWrite != courtGeneralDb.CourtBankruptcy.DateWrite)
                Result.AppendLine($"Дата списания: было {courtGeneralDb.CourtBankruptcy.DateWrite} стало {courtGeneralBe.CourtBankruptcy.DateWrite}");
            if (courtGeneralBe.CourtBankruptcy.Comment != courtGeneralDb.CourtBankruptcy.Comment)
                Result.AppendLine($"Примечание (банкротство): было {courtGeneralDb.CourtBankruptcy.Comment} стало {courtGeneralBe.CourtBankruptcy.Comment}");
            //CourtLitigationWork
            if (courtGeneralBe.CourtLitigationWork.DateDecisionCansel != courtGeneralDb.CourtLitigationWork.DateDecisionCansel)
                Result.AppendLine($"Дата определения об отмене СП: было {courtGeneralDb.CourtLitigationWork.DateDecisionCansel} стало {courtGeneralBe.CourtLitigationWork.DateDecisionCansel}");
            if (courtGeneralBe.CourtLitigationWork.DateReceipt != courtGeneralDb.CourtLitigationWork.DateReceipt)
                Result.AppendLine($"Дата получения определения об отмене СП: было {courtGeneralDb.CourtLitigationWork.DateReceipt} стало {courtGeneralBe.CourtLitigationWork.DateReceipt}");
            if (courtGeneralBe.CourtLitigationWork.DateSubmission != courtGeneralDb.CourtLitigationWork.DateSubmission)
                Result.AppendLine($"Дата передачи искового заявления в суд: было {courtGeneralDb.CourtLitigationWork.DateSubmission} стало {courtGeneralBe.CourtLitigationWork.DateSubmission}");
            if (courtGeneralBe.CourtLitigationWork.DateSendPirRCO != courtGeneralDb.CourtLitigationWork.DateSendPirRCO)
                Result.AppendLine($"Дата передачи документов в ПИР РЦПО: было {courtGeneralDb.CourtLitigationWork.DateSendPirRCO} стало {courtGeneralBe.CourtLitigationWork.DateSendPirRCO}");
            if (courtGeneralBe.CourtLitigationWork.AmountWithdrawnAll != courtGeneralDb.CourtLitigationWork.AmountWithdrawnAll)
                Result.AppendLine($"Взысканная сумма - всего: было {courtGeneralDb.CourtLitigationWork.AmountWithdrawnAll} стало {courtGeneralBe.CourtLitigationWork.AmountWithdrawnAll}");
            if (courtGeneralBe.CourtLitigationWork.AmountWithdrawnOd != courtGeneralDb.CourtLitigationWork.AmountWithdrawnOd)
                Result.AppendLine($"Взысканная сумма - ОД: было {courtGeneralDb.CourtLitigationWork.AmountWithdrawnOd} стало {courtGeneralBe.CourtLitigationWork.AmountWithdrawnOd}");
            if (courtGeneralBe.CourtLitigationWork.AmountWithdrawnPeny != courtGeneralDb.CourtLitigationWork.AmountWithdrawnPeny)
                Result.AppendLine($"Взысканная сумма - пени: было {courtGeneralDb.CourtLitigationWork.AmountWithdrawnPeny} стало {courtGeneralBe.CourtLitigationWork.AmountWithdrawnPeny}");
            if (courtGeneralBe.CourtLitigationWork.AmountRecoveredExpenses != courtGeneralDb.CourtLitigationWork.AmountRecoveredExpenses)
                Result.AppendLine($"Взысканная сумма - расходов: было {courtGeneralDb.CourtLitigationWork.AmountRecoveredExpenses} стало {courtGeneralBe.CourtLitigationWork.AmountRecoveredExpenses}");
            if (courtGeneralBe.CourtLitigationWork.AmountWithdrawnGp != courtGeneralDb.CourtLitigationWork.AmountWithdrawnGp)
                Result.AppendLine($"Взысканная сумма - ГП: было {courtGeneralDb.CourtLitigationWork.AmountWithdrawnGp} стало {courtGeneralBe.CourtLitigationWork.AmountWithdrawnGp}");
            if (courtGeneralBe.CourtLitigationWork.NameCourt != courtGeneralDb.CourtLitigationWork.NameCourt)
                Result.AppendLine($"Наименование суда: было {courtGeneralDb.CourtLitigationWork.NameCourt} стало {courtGeneralBe.CourtLitigationWork.NameCourt}");
            if (courtGeneralBe.CourtLitigationWork.AddressCourt != courtGeneralDb.CourtLitigationWork.AddressCourt)
                Result.AppendLine($"Адресс суда: было {courtGeneralDb.CourtLitigationWork.AddressCourt} стало {courtGeneralBe.CourtLitigationWork.AddressCourt}");
            if (courtGeneralBe.CourtLitigationWork.FioSendCourt != courtGeneralDb.CourtLitigationWork.FioSendCourt)
                Result.AppendLine($"ФИО сотрудника (направившего дело в суд): было {courtGeneralDb.CourtLitigationWork.FioSendCourt} стало {courtGeneralBe.CourtLitigationWork.FioSendCourt}");
            if (courtGeneralBe.CourtLitigationWork.HowSubmitApplicationCourt != courtGeneralDb.CourtLitigationWork.HowSubmitApplicationCourt)
                Result.AppendLine($"Способ отправки заявления в суд: было {courtGeneralDb.CourtLitigationWork.HowSubmitApplicationCourt} стало {courtGeneralBe.CourtLitigationWork.HowSubmitApplicationCourt}");
            if (courtGeneralBe.CourtLitigationWork.SumIskaAll != courtGeneralDb.CourtLitigationWork.SumIskaAll)
                Result.AppendLine($"Cумма иска - всего: было {courtGeneralDb.CourtLitigationWork.SumIskaAll} стало {courtGeneralBe.CourtLitigationWork.SumIskaAll}");
            if (courtGeneralBe.CourtLitigationWork.SumOdSendCourt != courtGeneralDb.CourtLitigationWork.SumOdSendCourt)
                Result.AppendLine($"Сумма ОД, предъявленная в суд: было {courtGeneralDb.CourtLitigationWork.SumOdSendCourt} стало {courtGeneralBe.CourtLitigationWork.SumOdSendCourt}");
            if (courtGeneralBe.CourtLitigationWork.SumPenySendCourt != courtGeneralDb.CourtLitigationWork.SumPenySendCourt)
                Result.AppendLine($"Сумма пени, предъявленная в суд: было {courtGeneralDb.CourtLitigationWork.SumPenySendCourt} стало {courtGeneralBe.CourtLitigationWork.SumPenySendCourt}");
            if (courtGeneralBe.CourtLitigationWork.SumOtherCourt != courtGeneralDb.CourtLitigationWork.SumOtherCourt)
                Result.AppendLine($"Сумма прочих расходов: было {courtGeneralDb.CourtLitigationWork.SumOtherCourt} стало {courtGeneralBe.CourtLitigationWork.SumOtherCourt}");
            if (courtGeneralBe.CourtLitigationWork.SumStateDuty != courtGeneralDb.CourtLitigationWork.SumStateDuty)
                Result.AppendLine($"Сумма госпошлины (указанная в иске): было {courtGeneralDb.CourtLitigationWork.SumStateDuty} стало {courtGeneralBe.CourtLitigationWork.SumStateDuty}");
            if (courtGeneralBe.CourtLitigationWork.PeriodDebtBegin != courtGeneralDb.CourtLitigationWork.PeriodDebtBegin)
                Result.AppendLine($"период задолженности начальный: было {courtGeneralDb.CourtLitigationWork.PeriodDebtBegin} стало {courtGeneralBe.CourtLitigationWork.PeriodDebtBegin}");
            if (courtGeneralBe.CourtLitigationWork.PeriodDebtEnd != courtGeneralDb.CourtLitigationWork.PeriodDebtEnd)
                Result.AppendLine($"период задолженности конечный: было {courtGeneralDb.CourtLitigationWork.PeriodDebtEnd} стало {courtGeneralBe.CourtLitigationWork.PeriodDebtEnd}");
            if (courtGeneralBe.CourtLitigationWork.DateDecision != courtGeneralDb.CourtLitigationWork.DateDecision)
                Result.AppendLine($"Дата решения: было {courtGeneralDb.CourtLitigationWork.DateDecision} стало {courtGeneralBe.CourtLitigationWork.DateDecision}");
            if (courtGeneralBe.CourtLitigationWork.SumOverpaidGP != courtGeneralDb.CourtLitigationWork.SumOverpaidGP)
                Result.AppendLine($"Сумма излишне уплаченной ГП: было {courtGeneralDb.CourtLitigationWork.SumOverpaidGP} стало {courtGeneralBe.CourtLitigationWork.SumOverpaidGP}");
            if (courtGeneralBe.CourtLitigationWork.SumPayGP != courtGeneralDb.CourtLitigationWork.SumPayGP)
                Result.AppendLine($"Сумма уплаченной ГП: было {courtGeneralDb.CourtLitigationWork.SumPayGP} стало {courtGeneralBe.CourtLitigationWork.SumPayGP}");
            if (courtGeneralBe.CourtLitigationWork.DateEntryDecision != courtGeneralDb.CourtLitigationWork.DateEntryDecision)
                Result.AppendLine($"Дата вступления решения в з.с.: было {courtGeneralDb.CourtLitigationWork.DateEntryDecision} стало {courtGeneralBe.CourtLitigationWork.DateEntryDecision}");
            if (courtGeneralBe.CourtLitigationWork.RequestDateIl != courtGeneralDb.CourtLitigationWork.RequestDateIl)
                Result.AppendLine($"Дата запроса ИЛ: было {courtGeneralDb.CourtLitigationWork.RequestDateIl} стало {courtGeneralBe.CourtLitigationWork.RequestDateIl}");
            if (courtGeneralBe.CourtLitigationWork.DateIssueIL != courtGeneralDb.CourtLitigationWork.DateIssueIL)
                Result.AppendLine($"Дата выдачи ИЛ: было {courtGeneralDb.CourtLitigationWork.DateIssueIL} стало {courtGeneralBe.CourtLitigationWork.DateIssueIL}");
            if (courtGeneralBe.CourtLitigationWork.DateFactGetIL != courtGeneralDb.CourtLitigationWork.DateFactGetIL)
                Result.AppendLine($"Дата фактического получения ИЛ: было {courtGeneralDb.CourtLitigationWork.DateFactGetIL} стало {courtGeneralBe.CourtLitigationWork.DateFactGetIL}");
            if (courtGeneralBe.CourtLitigationWork.NumberIl != courtGeneralDb.CourtLitigationWork.NumberIl)
                Result.AppendLine($"Номер ИЛ: было {courtGeneralDb.CourtLitigationWork.NumberIl} стало {courtGeneralBe.CourtLitigationWork.NumberIl}");
            if (courtGeneralBe.CourtLitigationWork.Comment != courtGeneralDb.CourtLitigationWork.Comment)
                Result.AppendLine($"Реквизиты ГП - дата платежного поручения: было {courtGeneralBe.CourtLitigationWork.Comment} стало {courtGeneralBe.CourtLitigationWork.Comment}");
            //CourtStateDuty
            if (courtGeneralBe.CourtStateDuty.DateSendOnReturnFNS != courtGeneralDb.CourtStateDuty.DateSendOnReturnFNS)
                Result.AppendLine($"Дата направления на возврат г/п в ФНС: было {courtGeneralDb.CourtStateDuty.DateSendOnReturnFNS} стало {courtGeneralBe.CourtStateDuty.DateSendOnReturnFNS}");
            if (courtGeneralBe.CourtStateDuty.DateReturnFNS != courtGeneralDb.CourtStateDuty.DateReturnFNS)
                Result.AppendLine($"Дата возврата заявления ФНС: было {courtGeneralDb.CourtStateDuty.DateReturnFNS} стало {courtGeneralBe.CourtStateDuty.DateReturnFNS}");
            if (courtGeneralBe.CourtStateDuty.ReasonReturn != courtGeneralDb.CourtStateDuty.ReasonReturn)
                Result.AppendLine($"Причина возврата заявления ФНС: было {courtGeneralDb.CourtStateDuty.ReasonReturn} стало {courtGeneralBe.CourtStateDuty.ReasonReturn}");
            if (courtGeneralBe.CourtStateDuty.Comment != courtGeneralDb.CourtStateDuty.Comment)
                Result.AppendLine($"Примечание (ФНС): было {courtGeneralDb.CourtStateDuty.Comment} стало {courtGeneralBe.CourtStateDuty.Comment}");
            //CourtWriteOff
            if (courtGeneralBe.CourtWriteOff.DocumentsPreparedWriteOff != courtGeneralDb.CourtWriteOff.DocumentsPreparedWriteOff)
                Result.AppendLine($"Документы подготовлены к списанию (да/нет): было {courtGeneralDb.CourtWriteOff.DocumentsPreparedWriteOff} стало {courtGeneralBe.CourtWriteOff.DocumentsPreparedWriteOff}");
            if (courtGeneralBe.CourtWriteOff.SumWriteOff != courtGeneralDb.CourtWriteOff.SumWriteOff)
                Result.AppendLine($"Сумма списания: было {courtGeneralDb.CourtWriteOff.SumWriteOff} стало {courtGeneralBe.CourtWriteOff.SumWriteOff}");
            if (courtGeneralBe.CourtWriteOff.DateWriteOffBegin != courtGeneralDb.CourtWriteOff.DateWriteOffBegin)
                Result.AppendLine($"Начальный период списания: было {courtGeneralDb.CourtWriteOff.DateWriteOffBegin} стало {courtGeneralBe.CourtWriteOff.DateWriteOffBegin}");
            if (courtGeneralBe.CourtWriteOff.DateWriteOffEnd != courtGeneralDb.CourtWriteOff.DateWriteOffEnd)
                Result.AppendLine($"Конечный период списания: было {courtGeneralDb.CourtWriteOff.DateWriteOffEnd} стало {courtGeneralBe.CourtWriteOff.DateWriteOffEnd}");
            if (courtGeneralBe.CourtWriteOff.WriteOffStatus != courtGeneralDb.CourtWriteOff.WriteOffStatus)
                Result.AppendLine($"Статус списания: было {courtGeneralDb.CourtWriteOff.WriteOffStatus} стало {courtGeneralBe.CourtWriteOff.WriteOffStatus}");
            if (courtGeneralBe.CourtWriteOff.DateWriteOff != courtGeneralDb.CourtWriteOff.DateWriteOff)
                Result.AppendLine($"Дата списания: было {courtGeneralDb.CourtWriteOff.DateWriteOff} стало {courtGeneralBe.CourtWriteOff.DateWriteOff}");
            if (courtGeneralBe.CourtWriteOff.Comment != courtGeneralDb.CourtWriteOff.Comment)
                Result.AppendLine($"Примечание (списания): было {courtGeneralDb.CourtWriteOff.Comment} стало {courtGeneralBe.CourtWriteOff.Comment}");

            //CourtOwnerInformation
            if (courtGeneralBe.CourtOwnerInformation.OwnerLastName != courtGeneralDb.CourtOwnerInformation.OwnerLastName)
                Result.AppendLine($"Фамилия собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerLastName} стало {courtGeneralBe.CourtOwnerInformation.OwnerLastName}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerFirstName != courtGeneralDb.CourtOwnerInformation.OwnerFirstName)
                Result.AppendLine($"Имя собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerFirstName} стало {courtGeneralBe.CourtOwnerInformation.OwnerFirstName}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerSurname != courtGeneralDb.CourtOwnerInformation.OwnerSurname)
                Result.AppendLine($"Отчество собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerSurname} стало {courtGeneralBe.CourtOwnerInformation.OwnerSurname}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerFloor != courtGeneralDb.CourtOwnerInformation.OwnerFloor)
                Result.AppendLine($"Пол собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerFloor} стало {courtGeneralBe.CourtOwnerInformation.OwnerFloor}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerDateBirthday != courtGeneralDb.CourtOwnerInformation.OwnerDateBirthday)
                Result.AppendLine($"Дата рождения собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerDateBirthday} стало {courtGeneralBe.CourtOwnerInformation.OwnerDateBirthday}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerPlaceBirth != courtGeneralDb.CourtOwnerInformation.OwnerPlaceBirth)
                Result.AppendLine($"Место рождения собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerPlaceBirth} стало {courtGeneralBe.CourtOwnerInformation.OwnerPlaceBirth}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerTypeDocuments != courtGeneralDb.CourtOwnerInformation.OwnerTypeDocuments)
                Result.AppendLine($"Вид документа, удостоверяющего личность собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerTypeDocuments} стало {courtGeneralBe.CourtOwnerInformation.OwnerTypeDocuments}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerPasportSeria != courtGeneralDb.CourtOwnerInformation.OwnerPasportSeria)
                Result.AppendLine($"Серия документа собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerPasportSeria} стало {courtGeneralBe.CourtOwnerInformation.OwnerPasportSeria}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerPasportNumber != courtGeneralDb.CourtOwnerInformation.OwnerPasportNumber)
                Result.AppendLine($"Номер документа собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerPasportNumber} стало {courtGeneralBe.CourtOwnerInformation.OwnerPasportNumber}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerPasportDate != courtGeneralDb.CourtOwnerInformation.OwnerPasportDate)
                Result.AppendLine($"Дата выдачи документа собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerPasportDate} стало {courtGeneralBe.CourtOwnerInformation.OwnerPasportDate}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerPasportIssue != courtGeneralDb.CourtOwnerInformation.OwnerPasportIssue)
                Result.AppendLine($"Орган выдавший документ собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerPasportIssue} стало {courtGeneralBe.CourtOwnerInformation.OwnerPasportIssue}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerInn != courtGeneralDb.CourtOwnerInformation.OwnerInn)
                Result.AppendLine($"ИНН собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerInn} стало {courtGeneralBe.CourtOwnerInformation.OwnerInn}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerSnils != courtGeneralDb.CourtOwnerInformation.OwnerSnils)
                Result.AppendLine($"СНИЛС собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerSnils} стало {courtGeneralBe.CourtOwnerInformation.OwnerSnils}");
            if (courtGeneralBe.CourtOwnerInformation.OwnerAddressRegister != courtGeneralDb.CourtOwnerInformation.OwnerAddressRegister)
                Result.AppendLine($"Адрес регистрации собственника: было {courtGeneralDb.CourtOwnerInformation.OwnerAddressRegister} стало {courtGeneralBe.CourtOwnerInformation.OwnerAddressRegister}");

            var res = Result.ToString();
            if (res.Contains("было"))
                return Result.ToString();
            else
                return string.Empty;
            
        }
        private string SerializerToXML<T>(T model)
        {
            XmlSerializer serializer = new XmlSerializer(model.GetType());
            using (var sww = new StringWriter())
            {
                serializer.Serialize(sww, model);
                return sww.ToString() +  DateTime.Now;
            }
        }
        
    }
}
