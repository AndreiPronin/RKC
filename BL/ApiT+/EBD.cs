using AppCache;
using BE.ApiT_;
using BL.Counters;
using BL.Extention;
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
using Object = BE.ApiT_.Object;

namespace BL.ApiT_
{
    public interface IEBD
    {
        string CreateEBDAll();
    }
    public class EBD:IEBD
    {
        private string KeyCasheload = "PROGRESS:LOAD:EBD";
        private string KeyCasheLock = "LOAD:LOCK:EBD";
        private readonly ICacheApp _cacheApp;
        private readonly IPersonalData _personalData;
        private readonly ICounter _counter;
        public EBD(ICacheApp cacheApp, IPersonalData personalData, ICounter counter)
        {
            _cacheApp = cacheApp;
            _personalData = personalData;
            _counter = counter;
        }
        public string CreateEBDAll()
        {
            if (!_cacheApp.isLock(KeyCasheLock))
            {
                objects objects = new objects();
                _cacheApp.AddProgress(KeyCasheload, "0");
                _cacheApp.Add(KeyCasheLock, nameof(CreateEBDAll));
                using(var db = new ApplicationDbContext())
                {
                    IQueryable<PersData> persDatas = db.PersData.Where(x=>x.Main == true);
                    var Data = persDatas.ToList();
                    var ListError = new List<string>();
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try { 
                        var DataPers = _personalData.GetPersonalInformation(Item.Lic);
                            if (DataPers.Count > 0)
                            {
                                var persData = DataPers.First();
                                var obj = new Object();
                                obj.system = " ";
                                obj.object_type = "FLAT";
                                obj.object_id = Item.Lic;
                                obj.parent_id = " ";
                                obj.object_disable = "false";
                                obj.CadastralNumber = " ";
                                obj.fias = persData.Fias;
                                obj.guid_enrgblng = " ";
                                obj.vid_blgu = " ";
                                obj.square_all = persData.Square.ToString();
                                obj.square_cold = persData.S_NOTP.ToString();
                                obj.guid_tplu = " ";
                                obj.subject = $@"{persData.FirstName} {persData.LastName} {persData.MiddleName}";
                                obj.giloe = "true";
                                obj.address = new Address();
                                obj.address.OKATO = " ";
                                obj.address.KLADR = " ";
                                obj.address.OKTMO = " ";
                                obj.address.PostalCode = " ";
                                obj.address.Region = "58";
                                obj.address.City = new City();
                                obj.address.District = new District();
                                obj.address.Street = new Street();
                                obj.address.Level1 = new Level1();
                                obj.address.Level2 = new Level2();
                                obj.address.Apartment = new Apartment();
                                obj.address.City.Name = "Пенза";
                                obj.address.City.Type = "г";
                                obj.address.District.Name = " ";
                                obj.address.District.Type = " ";
                                obj.address.Street.Name = persData.Street;
                                obj.address.Street.Type = "ул";
                                obj.address.Level1.Type = "д";
                                obj.address.Level1.Value = persData.House;
                                obj.address.Level2.Value = " ";
                                obj.address.Level2.Type = " ";
                                obj.address.Apartment.Value = persData.Flat;
                                obj.address.Apartment.Type = "кв";
                                obj.address.Note = $@"Российская Федерация, Пензенская область, г.Пенза, ул. {persData.Street}, дом №{persData.House}, кв. {persData.Flat}";
                                //obj.IPU_EE = new IPU_EE();
                                //obj.IPU_EE.status = "true";
                                var counter = _counter.DetailInfroms(persData.full_lic);
                                obj.IPU_HOT_W = new IPU_HOT_W();
                                obj.IPU_HOT_W.status = counter.Where(x => x.TYPE_PU.Contains("ГВС") && x.CLOSE_ != false).ToList().Count > 0 ? "true" : "false";
                                obj.IPU_OTOPL = new IPU_OTOPL();
                                obj.IPU_OTOPL.status = counter.Where(x => x.TYPE_PU.Contains("ОТП") && x.CLOSE_ != false).ToList().Count > 0 ? "true" : "false";
                                try
                                {
                                    objects.Objects.Add(obj);
                                }
                                catch (Exception e)
                                {
                                    ListError.Add(Item.Lic);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            ListError.Add(Item.Lic);
                        }
                    });
                    #endregion
                }


                var res = Serialize(objects);
                string path = @"F:\test.txt";   // путь к файлу

                string text = res; // строка для записи

                // запись в файл
                using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] buffer = Encoding.Default.GetBytes(text);
                    // запись массива байтов в файл
                    fstream.Write(buffer, 0, buffer.Length);
                }
                return Serialize(objects);
            }
            return null;
        }
        private string Serialize(objects value)
        {
            if (value == null) return string.Empty;

            var xmlSerializer = new XmlSerializer(value.GetType());

            using (var stringWriter = new Utf8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, value);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
