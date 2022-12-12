using AppCache;
using BE.ApiT_;
using BE.Service;
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
        byte[] CreateEBDAll(DateTime dateTime);
        byte[] CreateEbdMkd(DateTime dateTime);
        byte[] CreateEbdFlatliving(DateTime dateTime);
        byte[] CreateEbdFlatNotliving(DateTime dateTime);
        void UpdateLastLoadEbd(DateTime dateTime);
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
        public byte[] CreateEBDAll(DateTime dateTime)
        {
            if (!_cacheApp.isLock(KeyCasheLock))
            {
                var date = DateTime.Now.Date.AddMonths(-1);
                objects Flat = new objects();
                Flat.Objects = new List<Object>();
                objects Mkd = new objects();
                Mkd.Objects = new List<Object>();
                _cacheApp.AddProgress(KeyCasheload, "0");
                _cacheApp.Add(KeyCasheLock, nameof(CreateEBDAll));
                byte[] buffer = CreateEbdMkd(dateTime);
                buffer = buffer.Concat(CreateEbdFlatliving(dateTime)).ToArray();
                buffer = buffer.Concat(CreateEbdFlatNotliving(dateTime)).ToArray();   
                return buffer;
            }
            return new byte[0];
        }
        public byte[] CreateEbdMkd(DateTime dateTime)
        {
            if (!_cacheApp.isLock(KeyCasheLock))
            {
                objects Flat = new objects();
                Flat.Objects = new List<Object>();
                objects Mkd = new objects();
                Mkd.Objects = new List<Object>();
                _cacheApp.AddProgress(KeyCasheload, "0");
                _cacheApp.Add(KeyCasheLock, nameof(CreateEbdMkd));
                using (var db = new DbTPlus())
                {
                    IQueryable<MKD> Mkd_ = db.MKD.Where(x => x.date_edit.Value >= dateTime);
                    var Data = Mkd_.ToList();
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            var obj = new Object();
                            obj.system = Item.system.Replace("\v", "").Trim();
                            obj.object_type = Item.object_type.Replace("\v", "").Trim();
                            obj.object_id = $@"RBR{Item.object_id?.ToString().Replace("\v", "").Trim()}";
                            obj.object_disable = "false";
                            //obj.CadastralNumber = "";
                            //obj.fias = Item.fias;
                            //obj.guid_enrgblng = " ";
                            //obj.buildYear = "";
                            //obj.floors = "";
                            //obj.vid_blgu = "";
                            //obj.wall = "";
                            obj.square_all = Item.square_object_all?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            obj.square_cold = Item.square_cold_all?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            obj.square_mop_all = Item.square_mop_all?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            //obj.id_dogovor_iku = "";
                            //obj.otopl_7_12 = "";
                            //obj.ist_tpls = "";
                            //obj.guid_tplu = "";
                            //obj.warning_house = "";
                            //obj.obgtie = "";
                            obj.address = new Address();
                            //obj.address.OKATO = "";
                            //obj.address.KLADR = "";
                            //obj.address.OKTMO = "";
                            //obj.address.PostalCode = "";
                            obj.address.Region = "58";
                            obj.address.City = new City();
                            obj.address.District = new District();
                            obj.address.Street = new Street();
                            obj.address.Level1 = new Level1();
                            obj.address.Apartment = new Apartment();
                            obj.address.City.Name = "Пенза";
                            obj.address.City.Type = "г";
                            //obj.address.District.Name = "";
                            //obj.address.District.Type = "";
                            obj.address.Street.Name = Item.street.Replace("\v", "").Trim();
                            obj.address.Street.Type = "ул";
                            obj.address.Level1.Type = "д";
                            obj.address.Level1.Value = Item.home.Replace("\v", "").Trim();
                            obj.address.Note = $@"Российская Федерация, Пензенская область, г.Пенза, ул. {Item.street.Trim().Replace("\v", "")}, дом №{Item.home.Trim().Replace("\v", "")}";
                            obj.ODPU_EE = new ODPU_EE();
                            obj.ODPU_EE.status = Item.gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_HOT_W = new IPU_HOT_W();
                            obj.IPU_HOT_W.status = Item.ipu_gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_OTOPL = new IPU_OTOPL();
                            obj.IPU_OTOPL.status = Item.ipu_otp.ToLower().Contains("да") ? "true" : "false";
                            lock (Mkd.Objects)
                            {
                                Mkd.Objects.Add(obj);
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    });
                    #endregion
                }
                var mkd = Serialize(Mkd);
                byte[] buffer = Encoding.Default.GetBytes(mkd);
                return buffer;
            }
            return null;
        }
        public byte[] CreateEbdFlatliving(DateTime dateTime)
        {
            if (!_cacheApp.isLock(KeyCasheLock))
            {
                objects Flat = new objects();
                Flat.Objects = new List<Object>();
                _cacheApp.AddProgress(KeyCasheload, "0");
                _cacheApp.Add(KeyCasheLock, nameof(CreateEBDAll));
                using (var db = new DbTPlus())
                {
                    IQueryable<FLAT> flat_ = db.FLAT.Where(x => x.date_edit.Value >= dateTime && !x.giloe.ToLower().Contains("не"));
                    var Data = flat_.ToList();
                    var ListError = new List<string>();
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            var obj = new Object();
                            obj.system = Item.system_.Replace("", "").Trim();
                            obj.object_type = Item.object_t.Replace("", "").Trim();
                            obj.object_id = $@"RBR{Item.object_id.Replace("", "").Trim()}";
                            obj.parent_id = $@"RBR{Item.parent_id.ToString().Replace("", "").Trim()}";
                            obj.object_disable = "false";
                            obj.CadastralNumber = Item.cadastral_number?.ToString().Replace("", "").Trim();
                            obj.fias = Item.fias.Replace("", "").Trim();
                            //obj.guid_enrgblng = "";
                            //obj.vid_blgu = "";
                            obj.square_all = Item.square_all?.ToString().Replace("", "").Replace(",", ".").Trim();
                            obj.square_cold = Item.s_notp.Replace("", "").Replace(",", ".").Trim();
                            //obj.guid_tplu = " ";
                            obj.subject = Item.fio.Replace("", "").Trim();
                            obj.giloe = Item.giloe.ToLower().Contains("не") ? "false" : "true";
                            obj.address = new Address();
                            //obj.address.OKATO = "";
                            //obj.address.KLADR = "";
                            //obj.address.OKTMO = "";
                            //obj.address.PostalCode = "";
                            obj.address.Region = "58";
                            obj.address.City = new City();
                            obj.address.District = new District();
                            obj.address.Street = new Street();
                            obj.address.Level1 = new Level1();
                            obj.address.Level2 = new Level2();
                            obj.address.Apartment = new Apartment();
                            obj.address.City.Name = "Пенза";
                            obj.address.City.Type = "г";
                            //obj.address.District.Name = "";
                            //obj.address.District.Type = "";
                            obj.address.Street.Name = Item.street.Replace("", "").Trim();
                            obj.address.Street.Type = "ул";
                            obj.address.Level1.Type = "д";
                            obj.address.Level1.Value = Item.home.Replace("", "").Trim();
                            //obj.address.Level2.Value = "";
                            //obj.address.Level2.Type = "";
                            obj.address.Apartment.Value = Item.apartment.Replace("", "").Trim();
                            obj.address.Apartment.Type = "кв";
                            obj.address.Note = $@"Российская Федерация, Пензенская область, г.Пенза, ул. {Item.street.Trim().Replace("", "").Trim()}, дом №{Item.home.Trim().Replace("", "").Trim()}, кв. {Item.apartment.Trim().Replace("", "").Trim()}";
                            obj.ODPU_EE = new ODPU_EE();
                            obj.ODPU_EE.status = Item.gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_HOT_W = new IPU_HOT_W();
                            obj.IPU_HOT_W.status = Item.ipu_gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_OTOPL = new IPU_OTOPL();
                            obj.IPU_OTOPL.status = Item.ipu_otp.ToLower().Contains("да") ? "true" : "false";
                            lock (Flat.Objects)
                            {
                                Flat.Objects.Add(obj);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                    #endregion
                }
                var flat = Serialize(Flat);
                byte[] buffer = Encoding.Default.GetBytes(flat);
                return buffer;
            }
            return new byte[0];
        }
        public byte[] CreateEbdFlatNotliving(DateTime dateTime)
        {
            if (!_cacheApp.isLock(KeyCasheLock))
            {
                objects Flat = new objects();
                Flat.Objects = new List<Object>();
                _cacheApp.AddProgress(KeyCasheload, "0");
                _cacheApp.Add(KeyCasheLock, nameof(CreateEBDAll));
                using (var db = new DbTPlus())
                {
                    IQueryable<FLAT> flat_ = db.FLAT.Where(x => x.date_edit.Value >= dateTime && x.giloe.ToLower().Contains("не"));
                    var Data = flat_.ToList();
                    var ListError = new List<string>();
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            var obj = new Object();
                            obj.system = Item.system_.Replace("", "").Trim();
                            obj.object_type = Item.object_t.Replace("", "").Trim();
                            obj.object_id = $@"RBR{Item.object_id.Replace("", "").Trim()}";
                            obj.parent_id = $@"RBR{Item.parent_id.ToString().Replace("", "").Trim()}";
                            obj.object_disable = "false";
                            obj.CadastralNumber = Item.cadastral_number?.ToString().Replace("", "").Trim();
                            obj.fias = Item.fias.Replace("", "").Trim();
                            //obj.guid_enrgblng = "";
                            //obj.vid_blgu = "";
                            obj.square_all = Item.square_all?.ToString().Replace("", "").Replace(",", ".").Trim();
                            obj.square_cold = Item.s_notp.Replace("", "").Replace(",", ".").Trim();
                            //obj.guid_tplu = " ";
                            obj.subject = Item.fio.Replace("", "").Trim();
                            obj.giloe = Item.giloe.ToLower().Contains("не") ? "false" : "true";
                            obj.address = new Address();
                            //obj.address.OKATO = "";
                            //obj.address.KLADR = "";
                            //obj.address.OKTMO = "";
                            //obj.address.PostalCode = "";
                            obj.address.Region = "58";
                            obj.address.City = new City();
                            obj.address.District = new District();
                            obj.address.Street = new Street();
                            obj.address.Level1 = new Level1();
                            obj.address.Level2 = new Level2();
                            obj.address.Apartment = new Apartment();
                            obj.address.City.Name = "Пенза";
                            obj.address.City.Type = "г";
                            //obj.address.District.Name = "";
                            //obj.address.District.Type = "";
                            obj.address.Street.Name = Item.street.Replace("", "").Trim();
                            obj.address.Street.Type = "ул";
                            obj.address.Level1.Type = "д";
                            obj.address.Level1.Value = Item.home.Replace("", "").Trim();
                            //obj.address.Level2.Value = "";
                            //obj.address.Level2.Type = "";
                            obj.address.Apartment.Value = Item.apartment.Replace("", "").Trim();
                            obj.address.Apartment.Type = "кв";
                            obj.address.Note = $@"Российская Федерация, Пензенская область, г.Пенза, ул. {Item.street.Trim().Replace("", "").Trim()}, дом №{Item.home.Trim().Replace("", "").Trim()}, кв. {Item.apartment.Trim().Replace("", "").Trim()}";
                            obj.ODPU_EE = new ODPU_EE();
                            obj.ODPU_EE.status = Item.gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_HOT_W = new IPU_HOT_W();
                            obj.IPU_HOT_W.status = Item.ipu_gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_OTOPL = new IPU_OTOPL();
                            obj.IPU_OTOPL.status = Item.ipu_otp.ToLower().Contains("да") ? "true" : "false";
                            lock (Flat.Objects)
                            {
                                Flat.Objects.Add(obj);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                    #endregion
                }
                var flat = Serialize(Flat);
                byte[] buffer = Encoding.Default.GetBytes(flat);
                return buffer;
            }
            return new byte[0];
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

        public void UpdateLastLoadEbd(DateTime dateTime)
        {
            using(var db = new ApplicationDbContext())
            {
                var res = db.Flags.Find(((int)EnumFlags.LastLoadEbd));
                if (dateTime >= res.DateTime || res.DateTime == null)
                {
                    res.DateTime = dateTime;
                    db.SaveChanges();
                }
            }
        }
    }
}
