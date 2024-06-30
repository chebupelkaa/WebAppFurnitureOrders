using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IParameterService : IService<ParameterDTO>
    {
        public Task<ParameterDTO> GetParameterByNameAndValueAsync(string name,string value);

    }
}
