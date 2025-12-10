using IntroductionToWebAPIs.BaseEntities;

namespace IntroductionToWebAPIs.Entity
{
    public class Units : BaseEntity
    {
        public string Name { get; set; } = null!; // Наименование единицы измерения (например, "килограмм", "литр", "штука" и т.д.)
        public string Description { get; set; } = null!; // Описание единицы измерения
        public string Abbreviation { get; set; } = null!; // Сокращенное обозначение единицы измерения (например, "кг", "л", "шт" и т.д.)

        // Это коэффициент пересчёта в базовую единицу
        // Например: 1 мешок = 50 кг → Coefficient = 50
        //           1 упаковка = 25 кг → Coefficient = 25
        //           1 кг = 1 кг → Coefficient = 1
        public decimal Coefficient { get; set; } = 1;
    }
}
