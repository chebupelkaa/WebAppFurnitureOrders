using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class ProviderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<ProductDTO> Products { get; set; } = new();
    }
}
