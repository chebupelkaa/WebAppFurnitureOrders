using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class ProductStatsDTO
    {
        public ProductDTO Product { get; set; }
        public int OrderCount { get; set; }
        public double TotalOrderAmount { get; set; }
    }
}
