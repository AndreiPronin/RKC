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
using System.Text.RegularExpressions;
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
        byte[] CreateDirectFlat();
        byte[] CreateDirectMkd();
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
            var patern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
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
                    List<MKD> Mkd_ = db.Database.SqlQuery<MKD>($"SELECT * FROM [dbo].[EBD_MKD]('{dateTime.ToString("yyyy-MM-dd")}')").ToList();
                    var Data = Mkd_.ToList();
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            var obj = new Object();
                            obj.system = Item.system;
                            obj.object_type = Item.objectType;
                            obj.object_id = $@"RBR{Item.objectId}";
                            obj.object_disable = Item.object_disable.ToLower().Contains("да") ? "true" : "false";
                            //obj.CadastralNumber = "";
                            obj.fias = string.IsNullOrEmpty(Item.fias) ? "" : Regex.IsMatch(Item.fias, patern) ? Item.fias.Replace("", "").Trim() : "";
                            //obj.guid_enrgblng = "";
                            //obj.buildYear = "";
                            //obj.floors = "";
                            //obj.vid_blgu = "";
                            //obj.wall = "";
                            obj.square_object_all = Item.squareObjectAll is null || Item.squareObjectAll ==0 ? "0" : Item.squareObjectAll?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            obj.square_cold_all = Item.squareColdAll is null || Item.squareColdAll ==0 ? "0" : Item.squareColdAll?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            obj.square_mop_all = Item.squareMopAll?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            //obj.id_dogovor_iku = "";
                            //obj.otopl_7_12 = "";
                            //obj.ist_tpls = "";
                            //obj.guid_tplu = "";
                            //obj.warning_house = "";
                            //obj.obgtie = "";
                            obj.warning_house = "false";
                            obj.obgtie = "false";
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
                            obj.ODPU_HOT_W = new ODPU_HOT_W();
                            obj.ODPU_HOT_W.status = Item.ipuGvs.ToLower().Contains("да") ? "true" : "false";
                            obj.ODPU_OTOPL = new ODPU_OTOPL();
                            obj.ODPU_OTOPL.status = Item.ipuOtp.ToLower().Contains("да") ? "true" : "false";
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
        public byte[] CreateDirectMkd()
        {
            var patern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
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
                    List<DirectMkd> Mkd_ = db.Database.SqlQuery<DirectMkd>($"SELECT * FROM [dbo].[DirectMkd]").ToList();
                    var Data = Mkd_.ToList();
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            var obj = new Object();
                            obj.system = Item.system;
                            obj.object_type = Item.objectType;
                            obj.object_id = $@"RBR{Item.objectId}";
                            obj.object_disable = Item.object_disable.ToLower().Contains("да") ? "true" : "false";
                            //obj.CadastralNumber = "";
                            obj.fias = string.IsNullOrEmpty(Item.fias) ? "" : Regex.IsMatch(Item.fias, patern) ? Item.fias.Replace("", "").Trim() : "";
                            //obj.guid_enrgblng = "";
                            //obj.buildYear = "";
                            //obj.floors = "";
                            //obj.vid_blgu = "";
                            //obj.wall = "";
                            obj.square_object_all = Item.squareObjectAll is null || Item.squareObjectAll == 0 ? "0" : Item.squareObjectAll?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            obj.square_cold_all = Item.squareColdAll is null || Item.squareColdAll == 0 ? "0" : Item.squareColdAll?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            obj.square_mop_all = Item.squareMopAll?.ToString().Replace("\v", "").Replace(",", ".").Trim();
                            //obj.id_dogovor_iku = "";
                            //obj.otopl_7_12 = "";
                            //obj.ist_tpls = "";
                            //obj.guid_tplu = "";
                            //obj.warning_house = "";
                            //obj.obgtie = "";
                            obj.warning_house = "false";
                            obj.obgtie = "false";
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
                            obj.ODPU_HOT_W = new ODPU_HOT_W();
                            obj.ODPU_HOT_W.status = Item.ipuGvs.ToLower().Contains("да") ? "true" : "false";
                            obj.ODPU_OTOPL = new ODPU_OTOPL();
                            obj.ODPU_OTOPL.status = Item.ipuOtp.ToLower().Contains("да") ? "true" : "false";
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
        public byte[] CreateDirectFlat()
        {
            if (!_cacheApp.isLock(KeyCasheLock))
            {
                objects Flat = new objects();
                Flat.Objects = new List<Object>();
                _cacheApp.AddProgress(KeyCasheload, "0");
                _cacheApp.Add(KeyCasheLock, nameof(CreateEBDAll));
                using (var db = new DbTPlus())
                {
                    List<DirectFlat> flat_ = db.Database.SqlQuery<DirectFlat>($"SELECT * FROM [dbo].[DirectFlat]").ToList();
                    var Data = flat_.ToList();
                    var ListError = new List<string>();
                    var patern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            var obj = new Object();
                            obj.system = Item.system.Replace("", "").Trim();
                            obj.object_type = Item.objectT.Replace("", "").Trim();
                            obj.object_id = $@"RBR{Item.objectId.Replace("", "").Trim()}";
                            obj.parent_id = $@"RBR{Item.parentId.ToString().Replace("", "").Trim()}";
                            obj.object_disable = Item.object_disable.ToLower().Contains("да") ? "true" : "false";
                            obj.CadastralNumber = Item.cadastralNumber?.ToString().Replace("", "").Trim();
                            obj.fias = string.IsNullOrEmpty(Item.fias) ? "" : Regex.IsMatch(Item.fias, patern) ? Item.fias.Replace("", "").Trim() : "";
                            //obj.guid_enrgblng = "";
                            //obj.vid_blgu = "";
                            obj.square_all = Item.squareAll is null ? "0" : Item.squareAll?.ToString().Replace("", "").Replace(",", ".").Trim();
                            obj.square_cold = "0";
                            //obj.guid_tplu = " ";
                            obj.subject = Item.fio?.Replace("", "").Trim();
                            obj.giloe = !string.IsNullOrEmpty(Item.giloe) ? Item.giloe.ToLower().Contains("не") ? "false" : "true" : "false";
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
                            obj.IPU_EE = new IPU_EE();
                            obj.IPU_EE.status = Item.gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_HOT_W = new IPU_HOT_W();
                            obj.IPU_HOT_W.status = Item.ipuGvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_OTOPL = new IPU_OTOPL();
                            obj.IPU_OTOPL.status = Item.ipuOtp.ToLower().Contains("да") ? "true" : "false";
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
                    List<FLAT> flat_ = db.Database.SqlQuery<FLAT>($"SELECT * FROM [dbo].[EBD_FLAT]('{dateTime.ToString("yyyy-MM-dd")}','1')").ToList();
                    var Data = flat_.ToList();
                    var ListError = new List<string>();
                    var patern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            var obj = new Object();
                            obj.system = Item.system.Replace("", "").Trim();
                            obj.object_type = Item.objectT.Replace("", "").Trim();
                            obj.object_id = $@"RBR{Item.objectId.Replace("", "").Trim()}";
                            obj.parent_id = $@"RBR{Item.parentId.ToString().Replace("", "").Trim()}";
                            obj.object_disable = Item.object_disable.ToLower().Contains("да") ? "true" : "false";
                            obj.CadastralNumber = Item.CadstraNumber?.ToString().Replace("", "").Trim();
                            obj.fias = string.IsNullOrEmpty(Item.fias) ? "" : Regex.IsMatch(Item.fias, patern) ? Item.fias.Replace("", "").Trim() : "";
                            //obj.guid_enrgblng = "";
                            //obj.vid_blgu = "";
                            obj.square_all = Item.squareAll is null ? "0" : Item.squareAll?.ToString().Replace("", "").Replace(",", ".").Trim();
                            obj.square_cold = "0";
                            //obj.guid_tplu = " ";
                            obj.subject = Item.fio?.Replace("", "").Trim();
                            obj.giloe = !string.IsNullOrEmpty(Item.giloe) ? Item.giloe.ToLower().Contains("не") ? "false" : "true" : "false";
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
                            obj.IPU_EE = new IPU_EE();
                            obj.IPU_EE.status = Item.gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_HOT_W = new IPU_HOT_W();
                            obj.IPU_HOT_W.status = Item.ipuGvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_OTOPL = new IPU_OTOPL();
                            obj.IPU_OTOPL.status = Item.ipuOtp.ToLower().Contains("да") ? "true" : "false";
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
        #region Comment
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
                    List<FLAT> flat_ = db.Database.SqlQuery<FLAT>($"SELECT * FROM [dbo].[EBD_FLAT]('{dateTime.ToString("yyyy-MM-dd")}','0')").ToList();
                    var Data = flat_.ToList();
                    var ListError = new List<string>();
                    var patern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
                    #region
                    Parallel.ForEach(Data, Item =>
                    {
                        try
                        {
                            
                            var obj = new Object();
                            obj.system = Item.system.Replace("", "").Trim();
                            obj.object_type = Item.objectT.Replace("", "").Trim();
                            obj.object_id = $@"RBR{Item.objectId.Replace("", "").Trim()}";
                            obj.parent_id = $@"RBR{Item.parentId.ToString().Replace("", "").Trim()}";
                            obj.object_disable = Item.object_disable.ToLower().Contains("да") ? "true" : "false";
                            obj.CadastralNumber = Item.CadstraNumber?.ToString().Replace("", "").Trim();
                            obj.fias = string.IsNullOrEmpty(Item.fias) ? "" : Regex.IsMatch(Item.fias, patern) ? Item.fias.Replace("", "").Trim() : "";
                            //obj.guid_enrgblng = "";
                            //obj.vid_blgu = "";
                            obj.square_all = Item.squareAll is null ? "0" : Item.squareAll?.ToString().Replace("", "").Replace(",", ".").Trim();
                            obj.square_cold = "0";
                            //obj.guid_tplu = " ";
                            obj.subject = Item.fio.Replace("", "").Trim();
                            obj.giloe = !string.IsNullOrEmpty(Item.giloe) ? Item.giloe.ToLower().Contains("не") ? "false" : "true" : "false";
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
                            obj.IPU_EE = new IPU_EE();
                            obj.IPU_EE.status = Item.gvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_HOT_W = new IPU_HOT_W();
                            obj.IPU_HOT_W.status = Item.ipuGvs.ToLower().Contains("да") ? "true" : "false";
                            obj.IPU_OTOPL = new IPU_OTOPL();
                            obj.IPU_OTOPL.status = Item.ipuOtp.ToLower().Contains("да") ? "true" : "false";
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
        #endregion
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
