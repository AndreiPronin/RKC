using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "Flat", Schema = "Address")]
    public class FlatMkd
    {
        [Key]
        public string FullLic { get; set; }
        public string System { get; set; }
        public string ObjectType { get; set; }
        public int AddressId { get; set; }
        public string UniqueApartmentNumber { get; set; }
        public string UniqueRoomNumber { get; set; }
        public string CadastrNumberFlat { get; set; }
        public decimal? CadastrSquare { get; set; }
        public string Els { get; set; }
        public string IdGku { get; set; }
        public DateTime? DateEdit { get; set; }

        /// <summary>
        /// Корневой идентификатор договора (не меняется от версии к версии)
        /// </summary>
        public string ContractGUID { get; set; }

        /// <summary>
        /// Корневой идентификатор устава (не меняется от версии к версии)
        /// </summary>
        public string CharterGUID { get; set; }

        /// <summary>
        /// Идентификатор ЛС в ГИС ЖКХ (при обновлении данных ЛС)
        /// </summary>
        public string AccountGUID { get; set; }

        /// <summary>
        /// Идентификатор жилищно-коммунальной услуги
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public int? LivingPersonsNumber { get; set; }

        /// <summary>
        /// Общая площадь для ЛС
        /// </summary>
        public decimal? PremisesAreaExportType { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public decimal? ResidentialSquare { get; set; }

        /// <summary>
        /// Отапливаемая площадь
        /// </summary>
        public decimal? HeatedArea { get; set; }
    }
}
