using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double? Cost { get; set; }
        public int ProductGroupId { get; set; }
        public ProductGroupDTO ProductGroup { get; set; }
        public List<OrderDTO> Orders { get; set; } = new();
        public ProviderDTO Provider { get; set; }
        public int ProviderId { get; set; }
        public List<ParameterProductDTO> ParameterProducts { get; set; } = new();
        public List<WarehouseProductDTO> WarehouseProducts { get; set; } = new();
    }
}
