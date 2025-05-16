namespace TPlusModule.Common.Enums
{
    /// <summary>
    /// Содержит идентификаторы типов расчёта объёма в словаре
    /// </summary>
    public enum VolumeTypes
    {
        /// <summary>
        /// Норматив. Прибор учёта отсутствует
        /// </summary>
        NormativeNoMeterDevice = 0,
        /// <summary>
        /// По прибору учёта. Показания переданы
        /// </summary>
        ByMeterDevice = 1,
        /// <summary>
        /// По среднему. Показания не переданы (не более 3х месяцев)
        /// </summary>
        ByAverage = 2,
        /// <summary>
        /// Норматив. Нет показаний более 3х месяцев
        /// </summary>
        NormativeHasMeterDevice = 3,
        /// <summary>
        /// Расчёт по данной услуге не производится
        /// </summary>
        NoVolume = 10
    }
}
