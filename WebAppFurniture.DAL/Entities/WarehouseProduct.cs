using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.DAL.Entities
{
    public class WarehouseProduct
    {
        public int Id { get; set; }
        public int WarehouseNumber { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
