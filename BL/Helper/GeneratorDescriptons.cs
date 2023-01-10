using BE.Counter;
using BE.PersData;
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
        string Generate(PersDataModel PersDataModel);
    }
    public class GeneratorDescriptons : IGeneratorDescriptons
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
            }

            return Result.ToString();
        }
        public string Generate(PersDataModel PersDataModel)
        {
            StringBuilder Result = new StringBuilder();
            using (var db = new ApplicationDbContext()) {
                PersData PersData = db.PersData.Where(x => x.idPersData == PersDataModel.idPersData).FirstOrDefault();
                if (PersData?.FirstName != PersDataModel.FirstName && !string.IsNullOrEmpty(PersDataModel.FirstName)) Result.Append($"Изменили Имя: было {PersData.FirstName} стало {PersDataModel.FirstName} \r\n");
                if (PersData?.LastName != PersDataModel.LastName && !string.IsNullOrEmpty(PersDataModel.LastName)) Result.Append($"Изменили Фамилию: было {PersData.LastName} стало {PersDataModel.LastName} \r\n");
                if (PersData?.MiddleName != PersDataModel.MiddleName && !string.IsNullOrEmpty(PersDataModel.MiddleName)) Result.Append($"Изменили отчество: было {PersData.MiddleName} стало {PersDataModel.MiddleName} \r\n");
                if (PersData?.DateOfBirth != PersDataModel.DateOfBirth && PersDataModel.DateOfBirth != null ) Result.Append($"Изменили дату рождения: было {PersData.DateOfBirth} стало {PersDataModel.DateOfBirth} \r\n");
                //if (PersData.Lic != PersDataModel.Lic && !string.IsNullOrEmpty(PersDataModel.Lic)) Result.Append($"Изменили номер ПУ: было {PersData.Lic} стало {PersDataModel.Lic} \r\n");
                if (PersData?.PlaceOfBirth != PersDataModel.PlaceOfBirth && !string.IsNullOrEmpty(PersDataModel.PlaceOfBirth)) Result.Append($"Изменили место рождения: было {PersData.PlaceOfBirth} стало {PersDataModel.PlaceOfBirth} \r\n");
                if (PersData?.PassportSerial != PersDataModel.PassportSerial && !string.IsNullOrEmpty(PersDataModel.PassportSerial)) Result.Append($"Изменили паспорт серию: было {PersData.PassportSerial} стало {PersDataModel.PassportSerial} \r\n");
                if (PersData?.PassportNumber != PersDataModel.PassportNumber && !string.IsNullOrEmpty(PersDataModel.PassportNumber)) Result.Append($"Изменили паспорт номер: {PersData.PassportNumber} стало {PersDataModel.PassportNumber} \r\n");
                if (PersData?.PassportIssued != PersDataModel.PassportIssued && !string.IsNullOrEmpty(PersDataModel.PassportIssued)) Result.Append($"Изменили паспорт выдан: было {PersData.PassportIssued} стало {PersDataModel.PassportIssued} \r\n");
                if (PersData?.PassportDate != PersDataModel.PassportDate && PersDataModel.PassportDate != null) Result.Append($"Изменили паспорт дату: было {PersData.PassportDate} стало {PersDataModel.PassportDate} \r\n");
                if (PersData?.Tel1 != PersDataModel.Tel1 && !string.IsNullOrEmpty(PersDataModel.Tel1)) Result.Append($"Изменили телефон1: было {PersData.Tel1} стало {PersDataModel.Tel1} \r\n");
                if (PersData?.Comment1 != PersDataModel.Comment1 && !string.IsNullOrEmpty(PersDataModel.Comment1)) Result.Append($"Изменили комент к телефон1: было {PersData.Comment1} стало {PersDataModel.Comment1} \r\n");
                if (PersData?.Tel2 != PersDataModel.Tel2 && !string.IsNullOrEmpty(PersDataModel.Tel2)) Result.Append($"Изменили телефон2: было {PersData.Tel2} стало {PersDataModel.Tel2} \r\n");
                if (PersData?.Comment2 != PersDataModel.Comment2 && !string.IsNullOrEmpty(PersDataModel.Comment2)) Result.Append($"Изменили комент к телефон2: было {PersData.Comment2} стало {PersDataModel.Comment2} \r\n");
                if (PersData?.Email != PersDataModel.Email && !string.IsNullOrEmpty(PersDataModel.Email)) Result.Append($"Изменили Email: было {PersData.Email} стало {PersDataModel.Email} \r\n");
                if (PersData?.Comment != PersDataModel.Comment && !string.IsNullOrEmpty(PersDataModel.Comment)) Result.Append($"Изменили коментарий: было {PersData.Comment} стало {PersDataModel.Comment} \r\n");
                //if (PersData.UserName != PersDataModel.UserName && !string.IsNullOrEmpty(PersDataModel.UserName)) Result.Append($"Изменили номер ПУ: было {PersData.UserName} стало {PersDataModel.UserName} \r\n");
                if (PersData?.RoomType != PersDataModel.RoomType && !string.IsNullOrEmpty(PersDataModel.RoomType)) Result.Append($"Изменили собственика: было {PersData.RoomType} стало {PersDataModel.RoomType} \r\n");
                //if (PersData.Main != PersDataModel.Main && !string.IsNullOrEmpty(PersDataModel.Main)) Result.Append($"Изменили номер ПУ: было {PersData.Main} стало {PersDataModel.Main} \r\n");
                if (PersData?.SnilsNumber != PersDataModel.SnilsNumber && !string.IsNullOrEmpty(PersDataModel.SnilsNumber)) Result.Append($"Изменили снилс: было {PersData.SnilsNumber} стало {PersDataModel.SnilsNumber} \r\n");
                if (PersData?.Inn != PersDataModel.Inn && !string.IsNullOrEmpty(PersDataModel.Inn)) Result.Append($"Изменили инн: было {PersData.Inn} стало {PersDataModel.Inn} \r\n");
                if (PersData?.NumberOfPersons != PersDataModel.NumberOfPersons && PersDataModel.NumberOfPersons != null) Result.Append($"Изменили количество человек: было {PersData.NumberOfPersons} стало {PersDataModel.NumberOfPersons} \r\n");
                if (PersData?.Square != PersDataModel.Square && PersDataModel.Square != null) Result.Append($"Изменили площадь: было {PersData.Square} стало {PersDataModel.Square} \r\n");
                if (PersData?.SendingElectronicReceipt != PersDataModel.SendingElectronicReceipt && !string.IsNullOrEmpty(PersDataModel.SendingElectronicReceipt)) Result.Append($"Изменили отправка эл/квитанции: было {PersData.SendingElectronicReceipt} стало {PersDataModel.SendingElectronicReceipt} \r\n");
            }
            return Result.ToString();
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
