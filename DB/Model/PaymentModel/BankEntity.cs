using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB.Model.PaymentModel
{
    /// <summary>
    /// Банк или иная платежная организация
    /// </summary>
    public class BankEntity : AppBaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

    }
}
