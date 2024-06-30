using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double? Cost { get; set; }
        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public List<Order> Orders { get; set; } = new();
        public Provider Provider { get; set; }
        public int ProviderId { get; set; }
        public List<ParameterProduct> ParameterProducts { get; set; } = new();
        public List<WarehouseProduct> WarehouseProducts { get; set; } = new();
    }
}
