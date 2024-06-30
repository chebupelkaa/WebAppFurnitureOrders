using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ProgressCount { get; set; }
        public double TotalCost { get; set; }
        public string Status { get; set; }
        public int ClientId { get; set; }
        public ClientDTO Client { get; set; }
        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }
    }
}
