using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.DAL.Entities
{
    public class ParameterProduct
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public Parameter Parameter { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
