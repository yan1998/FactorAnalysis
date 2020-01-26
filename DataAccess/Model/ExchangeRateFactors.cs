using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Model
{
    public class ExchangeRateFactors
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Дата снятия показаний
        public DateTime Date { get; set; }

        // Курс Доллара
        public decimal ExchangeRateUSD { get; set; }

        // Курс Евро
        public decimal ExchangeRateEUR { get; set; }

        // Учетная (кредитная) ставка
        public double CreditRate { get; set; }

        // Показатель ВВП
        public long GDPIndicator { get; set; }

        // Показатель импорта товаров
        public double ImportIndicator { get; set; }

        // Показатель экспорта товаров
        public double ExportIndicator { get; set; }

        // Индекс инфляции (індекс споживчих цін)
        public double InflationIndex { get; set; }
    }
}
