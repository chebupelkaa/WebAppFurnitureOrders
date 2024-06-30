using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ProductStatsModel
    {
        public ProductModel Product { get; set; }
        public int OrderCount { get; set; }
        public double TotalOrderAmount { get; set; }
    }
}
