using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public class CourtDocumentScans
    {
        public int Id { get; set; }
        public int CourtGeneralInformId { get; set; }
        /// <summary>
        /// Наименование документа
        /// </summary>
        public string CourtDocumentScansName { get; set; }
        /// <summary>
        /// Путь для документа
        /// </summary>
        public string DocumentPath { get; set; }
        /// <summary>
        /// Дата загрузки документа
        /// </summary>
        public DateTime? DocumentDateUpload { get; set; }
        /// <summary>
        /// Исполнитель
        /// </summary>
        public string Executor { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
