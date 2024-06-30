using System.Text.Json.Serialization;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ProductGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<ProductModel> Products { get; set; } = new();
    }
}
