using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class ParameterProductDTO
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public ParameterDTO Parameter { get; set; }
        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }
    }
}
