using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class WarehouseProductDTO
    {
        public int Id { get; set; }
        public int WarehouseNumber { get; set; }
        public int WarehouseId { get; set; }
        public WarehouseDTO Warehouse { get; set; }
        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }
    }
}
