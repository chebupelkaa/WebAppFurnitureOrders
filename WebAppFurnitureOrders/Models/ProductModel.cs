using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double? Cost { get; set; }
        public int ProductGroupId { get; set; }
        public ProductGroupModel ProductGroup { get; set; }
        public List<OrderModel> Orders { get; set; } = new();
        public ProviderModel Provider { get; set; }
        public int ProviderId { get; set; }
        public List<ParameterProductModel> ParameterProducts { get; set; } = new();
        public List<WarehouseProductModel> WarehouseProducts { get; set; } = new();
    }
}
