using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class WarehouseDTO
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        public List<WarehouseProductDTO> WarehouseProducts { get; set; } = new();
    }
}
