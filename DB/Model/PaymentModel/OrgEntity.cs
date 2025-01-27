using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Model.PaymentModel
{
    /// <summary>
    /// Организация или ИП
    /// </summary>
    public class OrgEntity : AppBaseEntity
    {
        /// <summary>
        /// Полное наименование
        /// </summary>
        [MaxLength(250, ErrorMessage = "Cокращенное наименование должен быть размером до 250 символов")]
        public string Name { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        [MaxLength(100, ErrorMessage = "Cокращенное наименование должен быть размером до 250 символов")]
        public string ShortName { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [MaxLength(13, ErrorMessage = "ОГРН должен быть размером до 13 символов")]
        public string Ogrn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        [MaxLength(9, ErrorMessage = "КПП должен быть размером до 9 символов")]
        public string Kpp { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [MaxLength(12, ErrorMessage = "ИНН должен быть размером до 12 символов")]
        public string Inn { get; set; }

        /// <summary>
        /// Корреспондентский счет
        /// </summary>
        [MaxLength(50, ErrorMessage = "Корреспондентский счет должен быть размером до 12 символов")]
        public string CorrespondentAccount { get; set; }

        /// <summary>
        /// ГУИД ГИС ЖКХ
        /// </summary>
        public string OrigamiGuid { get; set; }

        /// <summary>
        /// Признак действующей организации
        /// </summary>
        public bool HasWork { get; set; } = true;

        /// <summary>
        /// Приоритет организации при работе с ГИС ЖКХ
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// Поддержка ОДПУ
        /// </summary>
        public bool IsSupportOdpu { get; set; }

        /// <summary>
        /// Способ проверки платежа при загрузки
        /// </summary>
        public ChoiceChecking Checking { get; set; } 

        /// <summary>
        /// Вариант способа проверки платежа при загрузки
        /// </summary>
        public enum ChoiceChecking
        {
            /// <summary>
            /// Не задано
            /// </summary>
            None = 0,

            /// <summary>
            /// РБР Т+
            /// </summary>
            RbrTPlus = 1,

            /// <summary>
            /// ОРИГАМИ
            /// </summary>
            Origami = 2,
        }
    }
}
