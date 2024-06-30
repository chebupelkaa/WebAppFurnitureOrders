using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.DAL.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Material { get; set; }
        public string Color { get; set; }
        public string ImageUrl{ get; set; }
        public List<WarehouseProduct> WarehouseProducts { get; set; } = new();
    }
}
