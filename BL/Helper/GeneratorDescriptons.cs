using BE.Counter;
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
                        if (aLL_LICS.FKUB1XVS != saveModelIPU.FKUB1XVS)
                            Result.Append($"Изменили начальные показания ГВС1: было {aLL_LICS.FKUB1XVS} стало {saveModelIPU.FKUB1XVS}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1XV_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1XV_2 != saveModelIPU.FKUB1XV_2)
                            Result.Append($"Изменили начальные показания ГВС2: было {aLL_LICS.FKUB1XV_2} стало {saveModelIPU.FKUB1XV_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1XV_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1XV_3 != saveModelIPU.FKUB1XV_3)
                            Result.Append($"Изменили начальные показания ГВС3: было {aLL_LICS.FKUB1XV_3} стало {saveModelIPU.FKUB1XV_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1XV_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1XV_4 != saveModelIPU.FKUB1XV_4)
                            Result.Append($"Изменили начальные показания ГВС4: было {aLL_LICS.FKUB1XV_4} стало {saveModelIPU.FKUB1XV_4}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_1 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_1 != saveModelIPU.FKUB1OT_1)
                            Result.Append($"Изменили начальные показания ОТП1: было {aLL_LICS.FKUB1OT_1} стало {saveModelIPU.FKUB1OT_1}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_2 != saveModelIPU.FKUB1OT_2)
                            Result.Append($"Изменили начальные показания ОТП2: было {aLL_LICS.FKUB1OT_2} стало {saveModelIPU.FKUB1OT_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_3 != saveModelIPU.FKUB1OT_3)
                            Result.Append($"Изменили начальные показания ОТП3: было {aLL_LICS.FKUB1OT_3} стало {saveModelIPU.FKUB1OT_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB1OT_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB1OT_4 != saveModelIPU.FKUB1OT_4)
                            Result.Append($"Изменили начальные показания ОТП4: было {aLL_LICS.FKUB1OT_4} стало {saveModelIPU.FKUB1OT_4}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XVS != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XVS != saveModelIPU.FKUB2XVS)
                            Result.Append($"Изменили конечные показания ГВС1: было {aLL_LICS.FKUB2XVS} стало {saveModelIPU.FKUB2XVS}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XV_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XV_2 != saveModelIPU.FKUB2XV_2)
                            Result.Append($"Изменили конечные показания ГВС2: было {aLL_LICS.FKUB2XV_2} стало {saveModelIPU.FKUB2XV_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XV_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XV_3 != saveModelIPU.FKUB2XV_3)
                            Result.Append($"Изменили конечные показания ГВС3: было {aLL_LICS.FKUB2XV_3} стало {saveModelIPU.FKUB2XV_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2XV_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {

                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2XV_4 != saveModelIPU.FKUB2XV_4)
                            Result.Append($"Изменили конечные показания ГВС4: было {aLL_LICS.FKUB2XV_4} стало {saveModelIPU.FKUB2XV_4}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_1 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_1 != saveModelIPU.FKUB2OT_1)
                            Result.Append($"Изменили конечные показания ОТП1: было {aLL_LICS.FKUB2OT_1} стало {saveModelIPU.FKUB2OT_1}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_2 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_2 != saveModelIPU.FKUB2OT_2)
                            Result.Append($"Изменили конечные показания ОТП2: было {aLL_LICS.FKUB2OT_2} стало {saveModelIPU.FKUB2OT_2}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_3 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_3 != saveModelIPU.FKUB2OT_3)
                            Result.Append($"Изменили конечные показания ОТП3: было {aLL_LICS.FKUB2OT_3} стало {saveModelIPU.FKUB2OT_3}  \r\n");

                    }
                }
                if (saveModelIPU.FKUB2OT_4 != null)
                {
                    using (var DbLIC = new DbLIC())
                    {
                        ALL_LICS aLL_LICS = DbLIC.ALL_LICS.Where(x => x.F4ENUMELS == saveModelIPU.FULL_LIC).FirstOrDefault();
                        if (aLL_LICS.FKUB2OT_4 != saveModelIPU.FKUB2OT_4)
                            Result.Append($"Изменили конечные показания ОТП4: было {aLL_LICS.FKUB2OT_4} стало {saveModelIPU.FKUB2OT_4}  \r\n");

                    }
                }
                IPU_COUNTERS IPU_COUNTERS = db.IPU_COUNTERS.Where(x => x.ID_PU == saveModelIPU.IdPU).FirstOrDefault();
                if (IPU_COUNTERS.FACTORY_NUMBER_PU != saveModelIPU.NumberPU && !string.IsNullOrEmpty(saveModelIPU.NumberPU)) Result.Append($"Изменили номер ПУ: было {IPU_COUNTERS.FACTORY_NUMBER_PU} стало {saveModelIPU.NumberPU} \r\n");
                if (IPU_COUNTERS.DATE_CHECK != saveModelIPU.DATE_CHECK && saveModelIPU.DATE_CHECK != null) Result.Append($"Изменили дату поверки ПУ: было {IPU_COUNTERS.DATE_CHECK}  стало {saveModelIPU.DATE_CHECK}  \r\n");
                if (IPU_COUNTERS.DATE_CHECK_NEXT != saveModelIPU.DATE_CHECK_NEXT && saveModelIPU.DATE_CHECK_NEXT != null) Result.Append($"Изменили дату следующей поверки ПУ: было {IPU_COUNTERS.DATE_CHECK_NEXT} стало {saveModelIPU.DATE_CHECK_NEXT}  \r\n");
                if (IPU_COUNTERS.MODEL_PU != saveModelIPU.MODEL_PU && !string.IsNullOrEmpty(saveModelIPU.MODEL_PU)) Result.Append($"Изменили модель ПУ: было {IPU_COUNTERS.MODEL_PU} стало {saveModelIPU.MODEL_PU}  \r\n");
                if (IPU_COUNTERS.SEALNUMBER != saveModelIPU.SEALNUMBER && !string.IsNullOrEmpty(saveModelIPU.SEALNUMBER)) Result.Append($"Изменили номер пломбы: было {IPU_COUNTERS.SEALNUMBER} стало {saveModelIPU.SEALNUMBER}  \r\n");
                if (IPU_COUNTERS.INSTALLATIONDATE != saveModelIPU.INSTALLATIONDATE && saveModelIPU.INSTALLATIONDATE != null) Result.Append($"Изменили дату установки: было {IPU_COUNTERS.INSTALLATIONDATE} стало {saveModelIPU.INSTALLATIONDATE}  \r\n");
                if (IPU_COUNTERS.TYPEOFSEAL != saveModelIPU.TYPEOFSEAL && !string.IsNullOrEmpty(saveModelIPU.NumberPU)) Result.Append($"Изменили тип пломбы: было {IPU_COUNTERS.TYPEOFSEAL} стало {saveModelIPU.TYPEOFSEAL}  \r\n");
                if (IPU_COUNTERS.DESCRIPTION != saveModelIPU.DESCRIPTION && !string.IsNullOrEmpty(saveModelIPU.TYPEOFSEAL)) Result.Append($"{saveModelIPU.DESCRIPTION}\r\n");
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
