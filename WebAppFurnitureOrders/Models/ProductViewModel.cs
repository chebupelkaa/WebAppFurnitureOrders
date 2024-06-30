using System.Text.Json.Serialization;

namespace WebAppFurnitureOrders.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public string Type { get; set; }
        public int ProductGroupId { get; set; }
        public ProductGroupModel ProductGroup { get; set; }
        public ProviderModel Provider { get; set; }
        public int ProviderId { get; set; }
   
        public List<string> Colors { get; set; }
  
        public List<string> Materials { get; set; }
       
        public List<string> ImageUrls { get; set; }

    }
}
