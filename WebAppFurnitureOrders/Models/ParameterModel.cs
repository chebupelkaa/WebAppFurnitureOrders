using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ParameterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public List<ParameterProductModel> ParameterProducts { get; set; } = new();
    }
}
