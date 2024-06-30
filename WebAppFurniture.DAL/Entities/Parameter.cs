using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppFurniture.DAL.Entities
{
    public class Parameter
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public List<ParameterProduct> ParameterProducts { get; set; } = new();
    }
}
