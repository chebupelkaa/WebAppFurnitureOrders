using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ParameterProductModel
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public ParameterModel Parameter { get; set; }
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
    }
}
