using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.Interfaces
{
    public interface IParameterProductService : IService<ParameterProductDTO>
    {
        public Task<IEnumerable<ParameterProductDTO>> GetByProductIdAsync(int productId);

        public Task<ParameterProductDTO> GetParameterProductByParametersAsync(int productId,int parameterId);
    }
}
