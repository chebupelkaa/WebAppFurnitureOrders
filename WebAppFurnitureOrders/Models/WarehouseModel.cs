using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class WarehouseModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        public List<WarehouseProductModel> WarehouseProducts { get; set; } = new();
    }
}
