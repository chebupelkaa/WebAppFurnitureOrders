using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class WarehouseProductModel
    {
        public int Id { get; set; }
        public int WarehouseNumber { get; set; }
        public int WarehouseId { get; set; }
        public WarehouseModel Warehouse { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}
