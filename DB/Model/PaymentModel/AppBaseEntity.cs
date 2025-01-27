using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Model.PaymentModel
{
    /// <summary>
    /// Базовый класс для сущности в БД
    /// </summary>
    public abstract class AppBaseEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public DateTime DtCreate { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата обновления записи
        /// </summary>
        public DateTime DtUpdate { get; set; } = DateTime.Now;

    }
}
