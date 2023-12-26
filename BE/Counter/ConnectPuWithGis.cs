using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    public class ConnectPuWithGis
    {
        /// <summary>
        /// Заводской (серийный) номер ПУ
        /// </summary>
        public string MeteringDeviceNumber { get; set; }
        /// <summary>
        /// Идентификатор версии ПУ в ГИС ЖКХ
        /// </summary>
        public Guid MeteringDeviceVersionGUID { get; set; }
        /// <summary>
        /// Номер прибора учета в ГИС ЖКХ
        /// </summary>
        public string MeteringDeviceGISGKHNumber { get; set; }
        /// <summary>
        /// Идентификатор ПУ в ГИС ЖКХ
        /// </summary>
        public Guid MeteringDeviceRootGUID { get; set; }
    }
    public class ConnectPuWithGisResponse : ConnectPuWithGis
    {
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } 
        /// <summary>
        /// Ошибка или не ошибка
        /// </summary>
        public bool Error { get; set; }
    }
}
