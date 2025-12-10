using IntroductionToWebAPIs.BaseEntities;
using System.Text.Json.Serialization;

namespace IntroductionToWebAPIs.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        public Guid UnitId { get; set; }
        [JsonIgnore]
        public Units? Unit { get; set; }
        public Guid SupplierId { get; set; }
        [JsonIgnore]
        public Supplier? Supplier { get; set; }
        public Guid WarehouseId { get; set; }
        [JsonIgnore]
        public Warehouse? Warehouse { get; set; }
        public int Quantity { get; set; }
        public List<Price> Prices { get; set; } = new();
    }
}
