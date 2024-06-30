using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.DTO
{
    public class ParameterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public List<ParameterProductDTO> ParameterProducts { get; set; } = new();
    }
}
