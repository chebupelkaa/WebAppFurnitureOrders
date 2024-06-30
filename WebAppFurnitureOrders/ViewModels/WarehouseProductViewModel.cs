using WebAppFurnitureOrders.Models;

namespace WebAppFurnitureOrders.ViewModels
{
    public class WarehouseProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string ProductGroup { get; set; }
        public string Provider { get; set; }
        public int Count { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }

        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int WarehouseProductId { get; set; }
        public int WarehouseNumber { get; set; }
        public int WarehouseId { get; set; }
    }
}
