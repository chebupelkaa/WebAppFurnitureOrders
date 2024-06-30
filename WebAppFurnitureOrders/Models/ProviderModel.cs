using System.Text.Json.Serialization;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurnitureOrders.Models
{
    public class ProviderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
      
        public List<ProductModel> Products { get; set; } = new();
        
    }
}
