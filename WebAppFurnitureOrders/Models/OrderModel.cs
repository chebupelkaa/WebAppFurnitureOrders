using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ProgressCount { get; set; }
        public double TotalCost { get; set; }
        public string Status { get; set; }
        public int ClientId { get; set; }
        public ClientModel Client { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}
