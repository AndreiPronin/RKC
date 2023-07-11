using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE.ApiT_
{
    
    public class objects
    {
        public objects()
        {
            Objects = new List<Object>();
        }
        [XmlElement("object")]
        public List<BE.ApiT_.Object> Objects { get; set; }
    }

    public class Object
    {
        public Object()
        {
        }
        [XmlElement("system")]
        public string system { get; set; }
        [XmlElement("object_type")]
        public string object_type { get; set; }
        [XmlElement("object_id")]
        public string object_id { get; set; }
        [XmlElement("object_disable")]
        public string object_disable { get; set; }
        [XmlElement("CadastralNumber")]
        public string CadastralNumber { get; set; }
        [XmlElement("fias")]
        public string fias { get; set; }
        [XmlElement("guid_enrgblng")]
        public string guid_enrgblng { get; set; }
        [XmlElement("buildYear")]
        public string buildYear { get; set; }
        [XmlElement("floors")]
        public string floors { get; set; }
        [XmlElement("vid_blgu")]
        public string vid_blgu { get; set; }
        [XmlElement("wall")]
        public string wall { get; set; }
        [XmlElement("square_object_all")]
        public string square_object_all { get; set; }
        [XmlElement("square_mop_all")]
        public string square_mop_all { get; set; }
        [XmlElement("square_cold_all")]
        public string square_cold_all { get; set; }
        [XmlElement("id_dogovor_iku")]
        public string id_dogovor_iku { get; set; }
        [XmlElement("otopl_7_12")]
        public string otopl_7_12 { get; set; }
        [XmlElement("ist_tpls")]
        public string ist_tpls { get; set; }
        [XmlElement("guid_tplu")]
        public string guid_tplu { get; set; }
        [XmlElement("warning_house")]
        public string warning_house { get; set; }
        [XmlElement("obgtie")]
        public string obgtie { get; set; }
        [XmlElement("address")]
        public Address address { get; set; }
        [XmlElement("ODPU_EE")]
        public ODPU_EE ODPU_EE { get; set; }
        [XmlElement("parent_id")]
        public string parent_id { get; set; }
        [XmlElement("square_all")]
        public string square_all { get; set; }
        [XmlElement("square_cold")]
        public string square_cold { get; set; }
        [XmlElement("subject")]
        public string subject { get; set; }
        [XmlElement("giloe")]
        public string giloe { get; set; }

        [XmlElement("IPU_EE")]
        public IPU_EE IPU_EE { get; set; }
        [XmlElement("IPU_HOT_W")]
        public IPU_HOT_W IPU_HOT_W { get; set; }
        [XmlElement("IPU_COLD_W")]
        public IPU_COLD_W IPU_COLD_W { get; set; }
        [XmlElement("IPU_OTOPL")]
        public IPU_OTOPL IPU_OTOPL { get; set; }
        [XmlElement("ODPU_HOT_W")]
        public ODPU_HOT_W ODPU_HOT_W { get; set; }
        [XmlElement("ODPU_COLD_W")]
        public ODPU_COLD_W ODPU_COLD_W { get; set; }
        [XmlElement("ODPU_OTOPL")]
        public ODPU_OTOPL ODPU_OTOPL { get; set; }
    }
    public class Address
    {
        [XmlElement("OKATO")]
        public string OKATO { get; set; }
        [XmlElement("KLADR")]
        public string KLADR { get; set; }
        [XmlElement("OKTMO")]
        public string OKTMO { get; set; }
        [XmlElement("PostalCode")]
        public string PostalCode { get; set; }
        [XmlElement("Region")]
        public string Region { get; set; }
        [XmlElement("City")]
        public City City { get; set; }
        [XmlElement("District")]
        public District District { get; set; }
        [XmlElement("Street")]
        public Street Street { get; set; }
        [XmlElement("Level1")]
        public Level1 Level1 { get; set; }
        
        //////////////////////////////////////    FLAT //////////////////
        [XmlElement("Level2")]
        public Level2 Level2 { get; set; }
        [XmlElement("Apartment")]
        public Apartment Apartment { get; set; }
        [XmlElement("Note")]
        public string Note { get; set; }

    }
    public class ODPU_EE
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
    public class ODPU_HOT_W
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
    public class ODPU_COLD_W
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
    public class ODPU_OTOPL
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
    public class City
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Type")]
        public string Type { get; set; }
    }
    public class District
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Type")]
        public string Type { get; set; }
    }
    public class Street
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Type")]
        public string Type { get; set; }
    }
    public class Level1
    {
        [XmlAttribute("Value")]
        public string Value { get; set; }
        [XmlAttribute("Type")]
        public string Type { get; set; }
    }
    public class Level2
    {
        
        [XmlAttribute("Type")]
        public string Type { get; set; }
        [XmlAttribute("Value")]
        public string Value { get; set; }

    }
    public class Apartment
    {
        
        [XmlAttribute("Type")]
        public string Type { get; set; }
        [XmlAttribute("Value")]
        public string Value { get; set; }

    }
    ////////////////////FLAT////////////////////
    public class IPU_EE
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
    public class IPU_HOT_W
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
    public class IPU_COLD_W
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
    public class IPU_OTOPL
    {
        [XmlElement("status")]
        public string status { get; set; }
    }
}
